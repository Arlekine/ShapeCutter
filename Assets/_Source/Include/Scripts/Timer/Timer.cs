using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float _currentTargetTime;
    private float _currentTime;
    private bool _isTimerRunning;
    private bool _isPaused;

    private Action _currentExpireEvent;

    public TimeSpan TimeLeft => TimeSpan.FromSeconds((double)(_currentTargetTime - _currentTime));
    public TimeSpan TimePass => TimeSpan.FromSeconds((double)_currentTime);

    public void StartTimer(int seconds, Action onExpired)
    {
        if (_isTimerRunning)
            throw new ArgumentException("Can't start timer while it is running. Stop timer before start it again.");

        _currentTargetTime = seconds;
        _currentTime = 0f;
        _isTimerRunning = true;
        _isPaused = false;
    }

    public void Stop()
    {
        _isTimerRunning = false;
        _currentTime = 0f;
        _currentTargetTime = 0f;
    }

    public void Pause()
    {
        _isPaused = true;
    }

    public void Continue()
    {
        _isPaused = false;
    }

    private void Update()
    {
        if (_isPaused == false && _isTimerRunning)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _currentTargetTime)
            {
                _isTimerRunning = false;
                _isPaused = false;
                _currentExpireEvent?.Invoke();
            }
        }
    }
}
