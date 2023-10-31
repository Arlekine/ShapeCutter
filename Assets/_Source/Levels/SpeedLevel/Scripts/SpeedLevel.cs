using System;
using System.Collections;
using DG.Tweening;
using DottedDrawer;
using ShapeComparing;
using ShapeComparing.Shapes;
using UISystem;
using UnityEngine;
using UnityEngine.UI;
using UnitySpriteCutter;
using UnitySpriteCutter.Control;
using UnitySpriteCutter.Control.PostProcessing;

namespace Levels
{
    public class SpeedLevel : Level
    {
        [SerializeField] private ShapeCuttingTask _shapeCuttingTask;
        [SerializeField] private PolygonCollider2D[] _tasks;
        [SerializeField] private CuttableShower _cuttableShower;
        [SerializeField] private Timer _timer;
        [SerializeField] private int _timeForLevel;
        [SerializeField] private float _waringStartTime = 15f;

        [Header("Comparing")]
        [SerializeField] private ShapeComparer _shapeComparer;
        [Range(0f, 1f)][SerializeField] private float _targetSimilarity = 0.75f;

        [Space]
        [SerializeField] private float _forceAfterCut = 5f;
        [SerializeField] private float _sizeToClearCuttable = 0.35f;

        [Header("Result")]
        [SerializeField] private float _timeLeftForPerfect = 10f;
        [SerializeField] private float _timeLeftForGood = 5f;

        [Header("UI Elements")]
        [SerializeField] private WarningTimerView _timerViewPrefab;
        [SerializeField] private ProgressionView _progressionView;
        [SerializeField] private Button _nextButtonPrefab;
        [SerializeField] private Button _finalButtonPrefab;

        private ComplexCutPostProcessor _cutPostProcessor;

        private int _currentTaskIndex;
        private Cuttable _currentCuttable;
        private Coroutine _currentLevelRoutine;

        private WarningTimerView _timerView;

        public override void Init(UI ui)
        {
            _cutPostProcessor = new ComplexCutPostProcessor();

            _cutPostProcessor.PostProcessors.Add(new BigPartStaticRigidbodyPostProcessor());
            _cutPostProcessor.PostProcessors.Add(new AddingForcePostProcessor(_forceAfterCut));
            _cutPostProcessor.PostProcessors.Add(new DeleteCuttableOnSizeLess(_sizeToClearCuttable));

            _shapeCuttingTask.Init();

            _currentLevelRoutine = StartCoroutine(CurrentTaskRoutine());

            _progressionView = ui.CreateUIElement(_progressionView, this);
            _timerView = ui.CreateUIElement(_timerViewPrefab, this);

            _progressionView.SetCounter(0, _tasks.Length);
            _timer.StartTimer(_timeForLevel, OnTimerExpired);
            _timerView.Init(_timer, _waringStartTime);
        }

        [EditorButton]
        private void ResetCurrent()
        {
            StopAllCoroutines();
            _currentLevelRoutine = StartCoroutine(CurrentTaskRoutine());
        }

        [EditorButton]
        private void NextShape()
        {
            _currentLevelRoutine = StartCoroutine(CurrentTaskRoutine());
        }

        private void OnTimerExpired()
        {
            _shapeCuttingTask.StopCutting();
            Completed?.Invoke(LevelResult.Loose);
        }

        private IEnumerator CurrentTaskRoutine()
        {
            _timer.Continue();
            _shapeCuttingTask.StopCutting();
            _shapeCuttingTask.ClearCurrentPieces();
            if (_currentCuttable != null)
                yield return _cuttableShower.RemoveCuttable(_currentCuttable).WaitForCompletion();

            var showing = _cuttableShower.ShowNewCuttable();

            _currentCuttable = showing.cuttable;
            yield return showing.animation.WaitForCompletion();

            _shapeCuttingTask.StartNewCutting(new ShapeCuttingTask.ShapeCuttingInput()
            {
                CutPostProcessor = _cutPostProcessor,
                StartingCuttable = _currentCuttable,
                TargetPolygon = _tasks[_currentTaskIndex]
            });

            var lastCutSimilarity = 0f;
            _shapeCuttingTask.Cutted += (cuttable) =>
            {
                _currentCuttable = cuttable;

                lastCutSimilarity = _shapeCuttingTask.GetCurrentSimilarity();
            };

            while (lastCutSimilarity < _targetSimilarity)
            {
                yield return null;
            }

            _timer.Pause();
            _shapeCuttingTask.StopCutting();
            _shapeCuttingTask.Cutted = null;
            _currentTaskIndex++;
            _progressionView.SetCounter(_currentTaskIndex, _tasks.Length);

            if (_currentTaskIndex >= _tasks.Length)
            {
                var levelResult = LevelResult.Pass;
                if (_timer.TimeLeft.Seconds >= _timeLeftForPerfect)
                    levelResult = LevelResult.Perfect;
                else if (_timer.TimeLeft.Seconds >= _timeLeftForGood)
                    levelResult = LevelResult.Good;

                Completed?.Invoke(levelResult);
            }
        }
    }
}
