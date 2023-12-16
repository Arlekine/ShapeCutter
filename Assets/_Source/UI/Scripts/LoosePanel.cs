using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoosePanel : MonoBehaviour
{
    [SerializeField] private UiShowingAnimation _showingAnimation;

    [Space]
    [SerializeField] private TMP_Text _hasAdText;
    [SerializeField] private TMP_Text _noAdText;

    [Space]
    [SerializeField] private Button _skipButton;
    [SerializeField] private Button _restartButton;

    public Action OnSkip;
    public Action OnRestart;

    public void Show(bool hasSkipOption)
    {
        _skipButton.gameObject.SetActive(hasSkipOption);
        _hasAdText.gameObject.SetActive(hasSkipOption);
        _noAdText.gameObject.SetActive(hasSkipOption == false);

        _showingAnimation.Show();
    }

    public void Hide()
    {
        _showingAnimation.Hide();
    }

    private void OnEnable()
    {
        _skipButton.onClick.AddListener(OnSkipClick);
        _restartButton.onClick.AddListener(OnRestartClick);
    }

    private void OnDisable()
    {
        _skipButton.onClick.RemoveListener(OnSkipClick);
        _restartButton.onClick.RemoveListener(OnRestartClick);
    }

    private void OnSkipClick()
    {
        OnSkip?.Invoke();
    }

    private void OnRestartClick()
    {
        OnRestart?.Invoke();
    }
}