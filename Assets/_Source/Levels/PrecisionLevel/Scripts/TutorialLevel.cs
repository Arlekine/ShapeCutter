using System.Collections.Generic;
using UISystem;
using UnityEngine;
using UnitySpriteCutter;
using UnitySpriteCutter.Control;
using UnitySpriteCutter.Control.PostProcessing;

namespace Levels
{
    public class TutorialLevel : LevelRoot
    {
        [SerializeField] private Cuttable _startCuttable;
        [SerializeField] private SpriteRendererColorRandomizer _colorRandomizer;

        [Space]
        [SerializeField] private ParticleSystem _partDestractionFX;
        [SerializeField] private LinecastCutter _linecastCutter;
        [SerializeField] private MouseInputRepresentationBehaviour _lineView;

        [Space]
        [SerializeField] private float _forceAfterCut = 5f;
        [SerializeField] private float _sizeToClearCuttable = 0.35f;

        [Space]
        [SerializeField] private Transform _cutStartPoint;
        [SerializeField] private Transform _cutEndPoint;

        [Header("UI")]
        [SerializeField] private TutorialLevelUI _levelUIPrefab;

        private ITutorialAnimation _lineCutTutorial;
        private ITutorialAnimation _pressTutorial;
        private ILineInput _lineInput;
        private Color _fxColor;
        private TutorialLevelUI _levelUI;
        private ComplexCutPostProcessor _cutPostProcessor;

        public override void Init(UI ui, TutorialHolder tutorial)
        {
            _ui = ui;
            _lineInput = GetComponent<ILineInput>();

            _cutPostProcessor = new ComplexCutPostProcessor();

            _cutPostProcessor.PostProcessors.Add(new BigPartStaticRigidbodyPostProcessor());
            _cutPostProcessor.PostProcessors.Add(new AddingForcePostProcessor(_forceAfterCut));

            _cutPostProcessor.PostProcessors.Add(new FunctionPostProcessor((input) =>
            {
                if (input.firstSide.BoundsSquare <= _sizeToClearCuttable)
                {
                    Destroy(input.firstSide.gameObject);
                    CreateDestroyFX(input.firstSide.RealCenter);
                }

                if (input.secondSide.BoundsSquare <= _sizeToClearCuttable)
                {
                    Destroy(input.secondSide.gameObject);
                    CreateDestroyFX(input.secondSide.RealCenter);
                }

                if (_lineCutTutorial != null)
                {
                    _lineCutTutorial.StopAndDestroy();
                    _lineCutTutorial = null;

                    _levelUI.EndTutorialButton.gameObject.SetActive(true);
                    _levelUI.EndTutorialButton.onClick.AddListener(FinishLevel);
                    _pressTutorial = tutorial.CreatePressTutorial(_levelUI.PressTutorialPoint, Vector2.zero);
                    _pressTutorial.Play();
                }

                return input;
            }));

            _fxColor = _colorRandomizer.Randomize();

            _linecastCutter.Init(_lineInput);
            _linecastCutter.SetPostProcessor(_cutPostProcessor);
            _lineView.Init(_lineInput);

            _levelUI = _ui.CreateUIElement(_levelUIPrefab, UIType.Level, this);
            _levelUI.EndTutorialButton.gameObject.SetActive(false);

            _lineCutTutorial = tutorial.CreateCutTutorial(_cutStartPoint.position, _cutEndPoint.position);
            _lineCutTutorial.Play();
        }

        private void FinishLevel()
        {
            _lineCutTutorial?.StopAndDestroy();
            _pressTutorial?.StopAndDestroy();
            Completed?.Invoke(LevelResult.Perfect);
        }

        private void CreateDestroyFX(Vector3 position)
        {
            var fx = Instantiate(_partDestractionFX, position, Quaternion.identity);
            var main = fx.main;
            main.startColor = new ParticleSystem.MinMaxGradient(_fxColor);
        }
    }
}