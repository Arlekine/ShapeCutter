using System;
using UnityEngine;
using UnityEngine.UI;

public class GameCompletePanel : MonoBehaviour
{
    [SerializeField] private UiShowingAnimation _showingAnimation;

    [Space]
    [SerializeField] private Button _homeButton;

    public Action OnHome;

    public void Show()
    {
        _showingAnimation.Show();
    }

    public void Hide()
    {
        _showingAnimation.Hide();
    }

    private void OnEnable()
    {
        _homeButton.onClick.AddListener(OnHomeClick);
    }

    private void OnDisable()
    {
        _homeButton.onClick.RemoveListener(OnHomeClick);
    }

    private void OnHomeClick()
    {
        OnHome?.Invoke();
    }
}