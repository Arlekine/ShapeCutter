using System;
using System.Collections;
using DG.Tweening;
using ShapeComparing;
using UISystem;
using UnityEngine;
using UnityEngine.UI;
using UnitySpriteCutter;
using UnitySpriteCutter.Control.PostProcessing;

namespace Levels
{
    public class SpeedLevelRootRoot : LevelRoot
    {
        [SerializeField] private ShapeCuttingTask _shapeCuttingTask;
        [SerializeField] private PolygonCollider2D[] _tasks;
        [SerializeField] private CuttableShower _cuttableShower;
        [SerializeField] private Timer _timer;
        [SerializeField] private int _timeForLevel;
        [SerializeField] private float _waringStartTime = 15f;
        [SerializeField] private float _timeBeforeNextShape = 1.5f;

        [Header("Comparing")]
        [SerializeField] private ShapeComparer _shapeComparer;
        [Range(0f, 1f)][SerializeField] private float _targetSimilarity = 0.75f;

        [Space]
        [SerializeField] private float _forceAfterCut = 5f;

        [Header("Result")]
        [SerializeField] private float _timeLeftForPerfect = 10f;
        [SerializeField] private float _timeLeftForGood = 5f;

        [Space] 
        [SerializeField] private SoundPlayer _stepSound;

        [Header("UI Elements")]
        [SerializeField] private SpeedLevelUI _speedLevelUIPrefab;
        [SerializeField] private LevelNameView _levelNamePrefab;


        private ComplexCutPostProcessor _cutPostProcessor;

        private SpeedLevelUI _speedLevelUI;
        private LevelNameView _levelName;
        private int _currentTaskIndex;
        private Cuttable _currentCuttable;
        private Coroutine _currentLevelRoutine;

        private float _lastCutSimilarity;

        private float PassedTimeNormalized => (_timeForLevel - (float)_timer.TimePass.TotalSeconds) / _timeForLevel;

        public override void Init(UI ui)
        {
            _ui = ui;
            _shapeCuttingTask.Init();

            _currentLevelRoutine = StartCoroutine(CurrentTaskRoutine());

            _speedLevelUI = ui.CreateUIElement(_speedLevelUIPrefab, UIType.Level, this);

            _timer.StartTimer(_timeForLevel, OnTimerExpired);
            _timer.Pause();

            _speedLevelUI.ProgressionView.SetCounter(0, _tasks.Length);
            _speedLevelUI.TimerView.Init(_timer, _waringStartTime);
            _speedLevelUI.ResetButton.onClick.AddListener(ResetCurrent);
            _speedLevelUI.Progress.Init(0f, _timeLeftForGood / _timeForLevel, _timeLeftForPerfect / _timeForLevel);

            _speedLevelUI.PauseScreen.HideInstantly();

            _speedLevelUI.PauseButton.gameObject.SetActive(true);
            _speedLevelUI.ResumeButton.gameObject.SetActive(false);
            _speedLevelUI.PauseButton.onClick.AddListener(Pause);
            _speedLevelUI.ResumeButton.onClick.AddListener(Resume);

            _levelName = _ui.CreateUIElement(_levelNamePrefab, UIType.Level, this);
            _levelName.Show();
            _levelName.Continued += OnLevelStart;
        }

        private void Pause()
        {
            _timer.Pause();
            _speedLevelUI.PauseButton.gameObject.SetActive(false);
            _speedLevelUI.ResumeButton.gameObject.SetActive(true);
            _speedLevelUI.PauseScreen.Show();
        }

        private void Resume()
        {
            _timer.Continue();
            _speedLevelUI.PauseButton.gameObject.SetActive(true);
            _speedLevelUI.ResumeButton.gameObject.SetActive(false);
            _speedLevelUI.PauseScreen.Hide();
        }

        private void OnLevelStart()
        {
            _levelName.Continued -= OnLevelStart;
            _ui.DestroyElement(_levelName, 0.5f, this);
            _timer.Continue();
        }

        private void Update()
        {
            _speedLevelUI.Progress.SetValue(PassedTimeNormalized);
        }

        private void ResetCurrent()
        {
            StopAllCoroutines();
            _shapeCuttingTask.Cutted -= OnShapeCutted;
            _currentLevelRoutine = StartCoroutine(CurrentTaskRoutine());
            _speedLevelUI.FinalPercentShower.Hide();
        }

        private void NextShape()
        {
            _currentLevelRoutine = StartCoroutine(CurrentTaskRoutine());
            _speedLevelUI.FinalPercentShower.Hide();
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

            var showing = _cuttableShower.ShowNewCuttable(transform);

            _currentCuttable = showing.cuttable;

            _cutPostProcessor = new ComplexCutPostProcessor();

            _cutPostProcessor.PostProcessors.Add(new BigPartStaticRigidbodyPostProcessor());
            _cutPostProcessor.PostProcessors.Add(new AddingForcePostProcessor(_forceAfterCut));

            _shapeCuttingTask.StartNewCutting(new ShapeCuttingTask.ShapeCuttingInput()
            {
                CutPostProcessor = _cutPostProcessor,
                StartingCuttable = _currentCuttable,
                TargetPolygon = _tasks[_currentTaskIndex],
                ColorRandomizer = _currentCuttable.GetComponent<SpriteRendererColorRandomizer>()
            });
            _shapeCuttingTask.StopCutting();

            yield return showing.animation.WaitForCompletion();

            _shapeCuttingTask.ContinueCutting();
            _lastCutSimilarity = 0f;
            _shapeCuttingTask.Cutted += OnShapeCutted;

            while (_lastCutSimilarity < _targetSimilarity)
            {
                yield return null;
            }

            _speedLevelUI.FinalPercentShower.Show(_lastCutSimilarity, PercentProgressView.Status.Win);

            _timer.Pause();
            _shapeCuttingTask.StopCutting();
            _stepSound.Play();
            _shapeCuttingTask.Cutted -= OnShapeCutted;
            _currentTaskIndex++;
            _speedLevelUI.ProgressionView.SetCounter(_currentTaskIndex, _tasks.Length);

            if (_currentTaskIndex >= _tasks.Length)
            {
                StartCoroutine(DelayedActionRoutine(_timeBeforeNextShape, () =>
                {
                    var levelResult = LevelResult.Pass;
                    if (_timer.TimeLeft.Seconds >= _timeLeftForPerfect)
                        levelResult = LevelResult.Perfect;
                    else if (_timer.TimeLeft.Seconds >= _timeLeftForGood)
                        levelResult = LevelResult.Good;

                    Completed?.Invoke(levelResult);
                }));
            }
            else
            {
                StartCoroutine(DelayedActionRoutine(_timeBeforeNextShape, NextShape));
            }
        }

        private IEnumerator DelayedActionRoutine(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);

            action();
        }

        private void OnShapeCutted(Cuttable currentCuttable)
        {
            _currentCuttable = currentCuttable;

            _shapeCuttingTask.GetCurrentSimilarityAsync((newSimilarity) =>
            {
                if (newSimilarity < _lastCutSimilarity)
                {
                    _speedLevelUI.FinalPercentShower.Show(newSimilarity, PercentProgressView.Status.Loose);
                    StartCoroutine(DelayedActionRoutine(_timeBeforeNextShape, ResetCurrent));
                }
                else if (newSimilarity < _targetSimilarity)
                {
                    _speedLevelUI.FinalPercentShower.Show(newSimilarity, PercentProgressView.Status.Normal);
                }

                _lastCutSimilarity = newSimilarity;
            });
        }
    }
}
