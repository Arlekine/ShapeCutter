using System.Collections;
using DG.Tweening;
using DottedDrawer;
using ShapeComparing;
using ShapeComparing.Shapes;
using UISystem;
using UnityEngine;
using UnityEngine.UI;
using UnitySpriteCutter;
using UnitySpriteCutter.Control.PostProcessing;

namespace Levels
{
    public class PrecisionLevelRootRoot : LevelRoot
    {
        [SerializeField] private ShapeCuttingTask _shapeCuttingTask;
        [SerializeField] private Cuttable _startingCuttable;
        [SerializeField] private PolygonCollider2D _targetPolygon;
        [SerializeField] private ShapeColorHolder _colorRandomizer;

        [Header("Comparing")]
        [SerializeField] private ShapeComparer _shapeComparer;
        [SerializeField] private Transform _comparePoint;
        [SerializeField] private float _finalMoveTime;
        [SerializeField] private float _finalOffsetTime = 3f;
        [SerializeField] private float _finalOffsetTimeIfShapeDestroyed = 0.3f;

        [Space] 
        [SerializeField] private float _forceAfterCut = 5f;
        [SerializeField] private float _sizeToClearCuttable = 0.35f;

        [Header("Result")]
        [SerializeField] private float _precisionToPerfect = 0.95f;
        [SerializeField] private float _precisionToGood = 0.9f;
        [SerializeField] private float _precisionToPass = 0.85f;

        [Header("UI Elements")] 
        [SerializeField] private PrecisionLevelUI _precisionLevelUIPrefab;
        [SerializeField] private LevelNameView _levelNamePrefab;

        private ITutorialAnimation _pressAnimation;
        private PrecisionLevelUI _precisionLevelUI;
        private LevelNameView _levelName;
        private UnlinearValueProgress _unlinearValueProgress;
        private TutorialHolder _tutorialHolder;

        private int _cutTimes;

        public override void Init(UI ui, TutorialHolder tutorial)
        {
            _ui = ui;
            _tutorialHolder = tutorial;
             var postProcessor = new ComplexCutPostProcessor();

            postProcessor.PostProcessors.Add(new BigPartStaticRigidbodyPostProcessor());
            postProcessor.PostProcessors.Add(new AddingForcePostProcessor(_forceAfterCut));

            if (tutorial.Data.IsStepFinished(TutorialStep.PRECISION_CHECK) == false)
            {
                postProcessor.PostProcessors.Add(new FunctionPostProcessor((input) =>
                {
                    _cutTimes++;

                    if (_cutTimes >= 2)
                    {
                        _pressAnimation = tutorial.CreatePressTutorial(_precisionLevelUI.TutorialPoint, Vector2.zero);
                        _pressAnimation.Play();
                    }

                    return input;
                }));
            }

            _shapeCuttingTask.Init();
            _shapeCuttingTask.StartNewCutting(new ShapeCuttingTask.ShapeCuttingInput() {CutPostProcessor = postProcessor, StartingCuttable = _startingCuttable, TargetPolygon = _targetPolygon, ColorRandomizer = _colorRandomizer});
            _shapeCuttingTask.BaseDestroyed += () => {
                _precisionLevelUI.CheckButton.onClick.RemoveListener(CompleteLevel);
                StartCoroutine(ShapeDestroyedRoutine());
            };

            var normalizedPass = (_precisionToPass / _precisionToPerfect);
            var normalizedGood = (_precisionToGood / _precisionToPerfect);

            _unlinearValueProgress = new UnlinearValueProgress(0.5f, normalizedPass);

            _precisionLevelUI = ui.CreateUIElement(_precisionLevelUIPrefab, UIType.Level, this);
            _precisionLevelUI.CheckButton.onClick.AddListener(CompleteLevel);

            _precisionLevelUI.Progress.Init(0.5f, _unlinearValueProgress.GetProgressForValue(normalizedGood));
            _precisionLevelUI.ProgressShowingAnimation.HideInstantly();

            _levelName = _ui.CreateUIElement(_levelNamePrefab, UIType.Level, this);
            _levelName.Show();
            _levelName.Continued += OnLevelStart;
        }

        private void OnLevelStart()
        {
            _levelName.Continued -= OnLevelStart;
            _ui.DestroyElement(_levelName, 0.5f, this);
        }


        private void CompleteLevel()
        {
            if (_tutorialHolder.Data.IsStepFinished(TutorialStep.PRECISION_CHECK) == false)
            {
                _pressAnimation?.StopAndDestroy();
                _tutorialHolder.Data.FinishedStep(TutorialStep.PRECISION_CHECK);
            }

            _precisionLevelUI.CheckButton.onClick.RemoveListener(CompleteLevel);
            StartCoroutine(CompletionRoutine());
        }

        private IEnumerator CompletionRoutine()
        {
            var result = _shapeCuttingTask.Finish();

            var finalSequence = DOTween.Sequence();

            var targetShapeOffset =result.TargetShape.transform.position - result.TargetShape.bounds.center;

            finalSequence.Join(result.FinalCuttable.transform.DOMove(_comparePoint.position + result.FinalCuttable.RealCenterOffset, _finalMoveTime).SetEase(Ease.InCubic));
            finalSequence.Join(result.TargetShape.transform.DOMove(_comparePoint.position + targetShapeOffset, _finalMoveTime).SetEase(Ease.InCubic));

            yield return finalSequence.WaitForCompletion();

            _precisionLevelUI.ProgressShowingAnimation.Show();

            var progress = _unlinearValueProgress.GetProgressForValue(Mathf.Clamp01(result.ComparingResult / _precisionToPerfect));

            _precisionLevelUI.Progress.SetValue(progress);
            _precisionLevelUI.FinalPercentShower.Show(result.ComparingResult);

            yield return new WaitForSeconds(_finalOffsetTime);

            var levelResult = LevelResult.Loose;

            if (result.ComparingResult >= _precisionToPerfect)
                levelResult = LevelResult.Perfect;
            else if (result.ComparingResult >= _precisionToGood)
                levelResult = LevelResult.Good;
            else if (result.ComparingResult >= _precisionToPass)
                levelResult = LevelResult.Pass;

            Completed?.Invoke(levelResult);
        }

        private IEnumerator ShapeDestroyedRoutine()
        {
            yield return new WaitForSeconds(_finalOffsetTimeIfShapeDestroyed);
            Completed?.Invoke(LevelResult.Loose);
        }
    }

    public class UnlinearValueProgress
    {
        private float _centerProgress;
        private float _centerValue;
        private float _goodValue;
        private float _perfectValue;

        public UnlinearValueProgress(float centerProgress, float centerValue)
        {
            _centerProgress = centerProgress;
            _centerValue = centerValue;
        }

        public float GetProgressForValue(float value)
        {
            var lp = value < _centerValue
                ? Mathf.InverseLerp(0f, _centerValue, value)
                : Mathf.InverseLerp(_centerValue, 1f, value);

            var progress = value < _centerValue
                ? Mathf.Lerp(0f, _centerProgress, lp)
                : Mathf.Lerp(_centerProgress, 1f, lp);

            return progress;
        }
    }
}