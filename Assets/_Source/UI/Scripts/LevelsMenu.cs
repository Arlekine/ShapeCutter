using System;
using System.Collections;
using System.Collections.Generic;
using Levels;
using UnityEngine;
using UnityEngine.UI;

public class LevelsMenu : MonoBehaviour
{
    [SerializeField] private RectTransform _stagesParent;
    [SerializeField] private StageView _stageViewPrefab;
    [SerializeField] private ProgressionView _stars;
    [SerializeField] private UiShowingAnimation _showingAnimation;
    [SerializeField] private Button _closeButton;

    private List<StageView> _currentStageViews = new List<StageView>();
    private HorizontalOrVerticalLayoutGroup _layoutGroup;

    public Action<Level> LevelSelected;
    public Action Closed;

    public void Open(List<Stage> stages, int stars, int maxStars)
    {
        _showingAnimation.HideInstantly();
        _showingAnimation.Show();

        _stars.SetCounter(stars, maxStars);

        var startLevelIndex = 0;
        for (int i = 0; i < stages.Count; i++)
        {
            var result = CreateStage(stages[i], i, startLevelIndex);
            startLevelIndex = result.levelIndex;
            _currentStageViews.Add(result.stageView);
        }

        _closeButton.onClick.AddListener(() => Closed?.Invoke());

        StartCoroutine(ResetLayoutRoutine());
    }

    public void Close()
    {
        _showingAnimation.Hide();
    }

    private IEnumerator ResetLayoutRoutine()
    {
        _layoutGroup = _stagesParent.GetComponent<HorizontalOrVerticalLayoutGroup>();
        _layoutGroup.enabled = false;
        yield return null;
        yield return null;
        _layoutGroup.enabled = true;
    }

    private (StageView stageView, int levelIndex) CreateStage(Stage stage, int stageIndex, int currentLevelIndex)
    {
        var newView = Instantiate(_stageViewPrefab, _stagesParent);
        var levelIndex = newView.Init(stage, stageIndex, currentLevelIndex);
        newView.LevelSelected += OnLevelSelected;
        return (newView, levelIndex);
    }

    private void OnLevelSelected(Level level) => LevelSelected?.Invoke(level);

    private void OnDestroy()
    {
        foreach (var stageView in _currentStageViews)
        {
            stageView.LevelSelected -= OnLevelSelected;
        }
    }
}