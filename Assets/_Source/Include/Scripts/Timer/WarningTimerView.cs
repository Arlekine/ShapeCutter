using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public sealed class WarningTimerView : MonoBehaviour
{
    private const string TimerFormat = "{0:00}:{1:00}";

    [SerializeField] private TMP_Text _timerText;

    [Space]
    [SerializeField] private float _bigScale;
    [SerializeField] private float _smallScale;
    [SerializeField] private float _circleTime;

    private Timer _timer;
    private Sequence _pulsatingAnimation; 
    private float _warningStartMoment;

    [EditorButton]
    public void Init(Timer timer, float warningStartTime)
    {
        _timer = timer;
        _warningStartMoment = warningStartTime;
    }

    private void Update()
    {
        if (_timer.TimeLeft.Seconds <= _warningStartMoment && _pulsatingAnimation == null)
        {
            _pulsatingAnimation = DOTween.Sequence();
            _pulsatingAnimation.Append(_timerText.transform.DOScale(_bigScale, _circleTime * 0.5f));
            _pulsatingAnimation.Append(_timerText.transform.DOScale(_smallScale, _circleTime * 0.5f));
            _pulsatingAnimation.SetLoops(-1);
        }

        _timerText.text = String.Format(TimerFormat, _timer.TimeLeft.Minutes, _timer.TimeLeft.Seconds);
    }

    private void OnDestroy()
    {
        _pulsatingAnimation?.Kill();
    }
}
