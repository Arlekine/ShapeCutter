using System;
using System.Collections.Generic;
using Levels;
using TMPro;
using UnityEngine;

public class StageView : MonoBehaviour
{
    [SerializeField] private RectTransform _buttonsParent;
    [SerializeField] private LevelButtonView _levelButtonPrefab;
    [SerializeField] private RectTransform _blockingScreen;

    [Space]
    [SerializeField] private TMP_Text _stageNumberText;
    [SerializeField] private TMP_Text _stageToOpenText;

    private List<LevelButtonView> _currentButtonViews = new List<LevelButtonView>();

    public Action<Level> LevelSelected;

    public int Init(Stage stage, int stageIndex, int levelsStartIndex)
    {
        Clear();

        _stageNumberText.text = (stageIndex + 1).ToString();

        for (int i = 0; i < stage.Levels.Count; i++)
        {
            _currentButtonViews.Add(CreateButton(stage.Levels[i], levelsStartIndex + i + 1));
        }

        _stageToOpenText.text = stage.StarsToOpenStage.ToString();
        _blockingScreen.gameObject.SetActive(stage.IsOpened == false);
        _blockingScreen.SetAsLastSibling();

        return levelsStartIndex + stage.Levels.Count;
    }

    private LevelButtonView CreateButton(Level level, int number)
    {
        var newButton = Instantiate(_levelButtonPrefab, _buttonsParent);
        newButton.Init(level, number);
        newButton.Selected += OnButtonSelected;
        return newButton;
    }

    private void OnButtonSelected(Level level)
    {
        LevelSelected?.Invoke(level);
    }

    private void Clear()
    {
        foreach (var currentButtonView in _currentButtonViews)
        {
            currentButtonView.Selected -= OnButtonSelected;
            Destroy(currentButtonView.gameObject);
        }

        _currentButtonViews?.Clear();
    }

    private void OnDestroy()
    {
        Clear();
    }
}