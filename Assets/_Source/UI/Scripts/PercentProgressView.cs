using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PercentProgressView : MonoBehaviour
{
    public enum Status
    {
        Normal,
        Win,
        Loose
    }

    private const string PercentFormat = "{0} %";

    [SerializeField] private RunningNumberView _runningNumberView;
    [SerializeField] private UiShowingAnimation _showingAnimation;

    [Space]
    [SerializeField] private TMP_Text _numberText;
    [SerializeField] private Color _winColor;
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _looseColor;
    [SerializeField] private float _colorChangeTime;

    public void Show(float percent, Status status)
    {
        if (percent < 0 || percent > 1)
            throw new ArgumentException($"{nameof(percent)} should be between 0 and 1");

        _showingAnimation.Show();
        _runningNumberView.ShowNumber((int)(percent * 100), PercentFormat);

        switch (status)
        {
            case Status.Loose:
                _numberText.DOColor(_looseColor, _colorChangeTime);
                break;

            case Status.Win:
                _numberText.DOColor(_winColor, _colorChangeTime);
                break;

            case Status.Normal:
                _numberText.DOColor(_normalColor, _colorChangeTime);
                break;
        }
    }

    public void Hide()
    {
        _showingAnimation.Hide();
    }
}