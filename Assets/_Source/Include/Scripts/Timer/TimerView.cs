using System;
using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviour
{
    private const string TimerFormat = "{0:00}:{1:00}";

    [SerializeField] private TMP_Text _timerText;

    private Timer _timer;

    [EditorButton]
    public void Init(Timer timer)
    {
        _timer = timer;
    }

    private void Update()
    {
        _timerText.text = String.Format(TimerFormat, _timer.TimeLeft.Minutes, _timer.TimeLeft.Seconds);
    }
}