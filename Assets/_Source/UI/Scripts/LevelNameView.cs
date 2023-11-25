using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNameView : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private UiShowingAnimation _showingAnimation;

    public Action Continued;

    public void Show()
    {
        _showingAnimation.HideInstantly();
        _showingAnimation.Show();
    }

    private void OnContinueClicked()
    {
        _showingAnimation.Hide();
        Continued?.Invoke();
    }

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(OnContinueClicked);
    }
}
