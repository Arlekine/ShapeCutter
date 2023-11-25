using System;
using Levels;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    [SerializeField] private UiShowingAnimation _showingAnimation;
    [SerializeField] private StarsAnimation _starsShowAnimation;

    [Space]
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _restartButton;

    public Action OnNext;
    public Action OnRestart;

    public void Show(LevelResult result)
    {
        var starsToShow = (int)result;
        _starsShowAnimation.Show(starsToShow);

        _showingAnimation.Show();
    }

    public void Hide()
    {
        _showingAnimation.Hide();
    }

    private void OnEnable()
    {
        _nextButton.onClick.AddListener(OnNextClick);
        _restartButton.onClick.AddListener(OnRestartClick);
    }

    private void OnDisable()
    {
        _nextButton.onClick.RemoveListener(OnNextClick);
        _restartButton.onClick.RemoveListener(OnRestartClick);
    }

    private void OnNextClick()
    {
        OnNext?.Invoke();
    }

    private void OnRestartClick()
    {
        OnRestart?.Invoke();
    }
}