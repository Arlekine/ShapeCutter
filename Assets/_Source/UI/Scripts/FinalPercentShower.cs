using System;
using UnityEngine;

public class FinalPercentShower : MonoBehaviour
{
    private const string PercentFormat = "{0} %";

    [SerializeField] private RunningNumberView _runningNumberView;
    [SerializeField] private UiShowingAnimation _showingAnimation;

    [EditorButton]
    public void Show(float percent)
    {
        if (percent < 0 || percent > 1)
            throw new ArgumentException($"{nameof(percent)} should be between 0 and 1");

        _showingAnimation.HideInstantly();
        _showingAnimation.Show();

        _runningNumberView.ShowNumber(0, Mathf.RoundToInt(percent * 100), PercentFormat);
    }

    [EditorButton]
    public void Hide()
    {
        _showingAnimation.Hide();
    }
}