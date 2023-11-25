using System;
using TMPro;
using UnityEngine;

public sealed class WarningTimerView : MonoBehaviour
{
    private const string TimerFormat = "{0:00}:{1:00}";

    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private PulsingAnimation _animation;

    private Timer _timer;
    private float _warningStartMoment;

    [EditorButton]
    public void Init(Timer timer, float warningStartTime)
    {
        _timer = timer;
        _warningStartMoment = warningStartTime;
    }

    private void Update()
    {
        if (_timer.TimeLeft.Seconds <= _warningStartMoment && _animation.IsPlaying() == false)
        {
            _animation.Play(_timerText.transform);
        }

        _timerText.text = String.Format(TimerFormat, _timer.TimeLeft.Minutes, _timer.TimeLeft.Seconds);
    }
}