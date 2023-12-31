using System.Collections;
using System.Collections.Generic;
using UISystem;
using UnityEngine;
using UnityEngine.Events;
using UnitySpriteCutter;
using UnitySpriteCutter.Control;
using UnitySpriteCutter.Control.PostProcessing;

namespace Levels
{
    public class PiecesCountLevelRoot : LevelRoot
    {
        public UnityEvent OnLevelStarted;

        [SerializeField] private CuttableTrigger _piecesTrigger;
        [SerializeField] private ParticleSystem _pieceGetFX;
        [SerializeField] protected Cuttable _startCuttable;
        [SerializeField] protected Cuttable[] _additionalStartCuttables;
        [SerializeField] private ShapeColorHolder _colorRandomizer;
        [SerializeField] protected ShapeColorHolder[] _additionalColorRandomizer;

        [Space]
        [SerializeField] private ParticleSystem _partDestractionFX;
        [SerializeField] protected LinecastCutter _linecastCutter;
        [SerializeField] protected MouseInputRepresentationBehaviour _lineView;

        [Header("Result")]
        [SerializeField] private int _piecesForPerfect = 50;
        [SerializeField] private int _piecesForGood = 35;
        [SerializeField] private int _piecesForPass = 15;

        [Space]
        [SerializeField] private SoundPlayer _stepSound;

        [Space]
        [SerializeField] private float _forceAfterCut = 5f;
        [SerializeField] private float _sizeToClearCuttable = 0.35f;

        [Header("UI")] 
        [SerializeField] private PiecesCountLevelUI _levelUIPrefab;
        [SerializeField] private LevelNameView _levelNamePrefab;

        protected ILineInput _lineInput;
        private Color _fxColor;
        private PiecesCountLevelUI _levelUI;
        private LevelNameView _levelName;
        private HashSet<Cuttable> _allPieces = new HashSet<Cuttable>();
        private ComplexCutPostProcessor _cutPostProcessor;
        private int _counter;

        private bool _levelEnded;

        private float OneStarProgress => (float)_piecesForPass / (float)_piecesForPerfect;
        private float TwoStarProgress => (float)_piecesForGood / (float)_piecesForPerfect;
        private float CurrentProgress => (float)_counter / (float)_piecesForPerfect;

        public override void Init(UI ui, TutorialHolder tutorial)
        {
            _ui = ui;
            _lineInput = GetComponent<ILineInput>();

            _allPieces.Add(_startCuttable);

            foreach (var additionalStartCuttable in _additionalStartCuttables)
            {
                _allPieces.Add(additionalStartCuttable);
            }

            _piecesTrigger.TriggerEnter += OnPieceFall;

            _cutPostProcessor = new ComplexCutPostProcessor();

            _cutPostProcessor.PostProcessors.Add(new BigPartStaticRigidbodyPostProcessor());
            _cutPostProcessor.PostProcessors.Add(new AddingForcePostProcessor(_forceAfterCut));
            SetAdditionalCutPostProcessors(_cutPostProcessor);

            _cutPostProcessor.PostProcessors.Add(new FunctionPostProcessor((input) =>
            {
                if (input.firstSide.BoundsSquare <= _sizeToClearCuttable)
                {
                    Destroy(input.firstSide.gameObject);
                    CreateDestroyFX(input.firstSide.RealCenter);
                }
                else
                {
                    _allPieces.Add(input.firstSide);
                }

                if (input.secondSide.BoundsSquare <= _sizeToClearCuttable)
                {
                    Destroy(input.secondSide.gameObject);
                    CreateDestroyFX(input.secondSide.RealCenter);
                }
                else
                {
                    _allPieces.Add(input.secondSide);
                }

                _allPieces.RemoveWhere(x => x == null);

                return input;
            }));

            _fxColor = _colorRandomizer.GetColor();

            foreach (var col in _additionalColorRandomizer)
            {
                col.GetColor();
            }

            _linecastCutter.Init(_lineInput);
            _linecastCutter.SetPostProcessor(_cutPostProcessor);
            _lineView.Init(_lineInput);

            _levelUI = _ui.CreateUIElement(_levelUIPrefab, UIType.Level, this);
            _levelUI.LevelResultProgress.Init(OneStarProgress, TwoStarProgress);
            _levelUI.Counter.SetCounter(0);

            _levelName = _ui.CreateUIElement(_levelNamePrefab, UIType.Level, this);
            _levelName.Show();
            _levelName.Continued += OnLevelStart;
        }

        protected virtual void SetAdditionalCutPostProcessors(ComplexCutPostProcessor complexCutPostProcessor)
        {

        }

        protected virtual void OnLevelStart()
        {
            _levelName.Continued -= OnLevelStart;
            _ui.DestroyElement(_levelName, 0.5f, this);
            OnLevelStarted?.Invoke();
        }

        private void CreateDestroyFX(Vector3 position)
        {
            var fx = Instantiate(_partDestractionFX, position, Quaternion.identity);
            var main = fx.main;
            main.startColor = new ParticleSystem.MinMaxGradient(_fxColor);
        }

        private void OnPieceFall(Cuttable piece)
        {
            _counter++; 
            _pieceGetFX.Play();
            _levelUI.Counter.SetCounter(_counter);
            _levelUI.LevelResultProgress.SetValue(Mathf.Clamp01(CurrentProgress));

            _stepSound.Play();
            Destroy(piece.gameObject);

            _allPieces.Remove(piece);

            if (_levelEnded == false && (_counter >= _piecesForPerfect || _allPieces.Count == 0))
            {
                CompleteLevel();
            }
        }

        private void CompleteLevel()
        {
            _levelEnded = true;
            _lineInput.Disable();

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
}
