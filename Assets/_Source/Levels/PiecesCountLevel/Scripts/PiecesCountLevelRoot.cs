using System.Collections.Generic;
using UISystem;
using UnityEngine;
using UnitySpriteCutter;
using UnitySpriteCutter.Control;
using UnitySpriteCutter.Control.PostProcessing;

namespace Levels
{
    public class PiecesCountLevelRoot : Level
    {
        [SerializeField] private CuttableTrigger _piecesTrigger;
        [SerializeField] private Cuttable _startCuttable;

        [Space]
        [SerializeField] private LinecastCutter _linecastCutter;
        [SerializeField] private MouseInputRepresentationBehaviour _lineView;
        [SerializeField] private MouseLineInput _lineInput;

        [Header("Result")]
        [SerializeField] private float _piecesForPerfect = 50f;
        [SerializeField] private float _piecesForGood = 35f;
        [SerializeField] private float _piecesForPass = 15f;

        [Space]
        [SerializeField] private float _forceAfterCut = 5f;
        [SerializeField] private float _sizeToClearCuttable = 0.35f;

        private bool _isMainCuttbaleOver;
        private List<Cuttable> _allPieces = new List<Cuttable>();
        private ComplexCutPostProcessor _cutPostProcessor;
        private int _counter;

        public override void Init(UI ui)
        {
            _allPieces.Add(_startCuttable);
            _piecesTrigger.TriggerEnter += OnPieceFall;

            _cutPostProcessor = new ComplexCutPostProcessor();

            _cutPostProcessor.PostProcessors.Add(new BigPartStaticRigidbodyPostProcessor());
            _cutPostProcessor.PostProcessors.Add(new AddingForcePostProcessor(_forceAfterCut));

            _cutPostProcessor.PostProcessors.Add(new FunctionPostProcessor((input) =>
            {
                if (input.firstSide.BoundsSquare <= _sizeToClearCuttable)
                {
                    Destroy(input.firstSide.gameObject);
                }
                else
                {
                    _allPieces.Add(input.firstSide);
                }

                if (input.secondSide.BoundsSquare <= _sizeToClearCuttable)
                {
                    Destroy(input.secondSide.gameObject);
                }
                else
                {
                    _allPieces.Add(input.secondSide);
                }

                _allPieces.RemoveAll(x => x == null);

                return input;
            }));

            _linecastCutter.Init(_lineInput);
            _linecastCutter.SetPostProcessor(_cutPostProcessor);
            _lineView.Init(_lineInput);
        }

        private void OnPieceFall(Cuttable piece)
        {
            _counter++;
            print(_counter);
            Destroy(piece.gameObject);

            _allPieces.Remove(piece);
            print("Ostalsya: " + _allPieces.Count);

            if (_allPieces.Count == 0)
            {
                CompleteLevel();
            }
        }

        private void CompleteLevel()
        {
            if (_counter >= _piecesForPerfect)
                Completed?.Invoke(LevelResult.Perfect);
            else if (_counter >= _piecesForGood)
                Completed?.Invoke(LevelResult.Good);
            else if (_counter >= _piecesForPass)
                Completed?.Invoke(LevelResult.Pass);
            else
                Completed?.Invoke(LevelResult.Loose);
        }
    }

    public class PiecesCountLevelUI : MonoBehaviour
    {
    }
}
