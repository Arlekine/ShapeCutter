using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;

    private bool _currentState;

    public Action<bool> StateChanged;

    public void Init(bool state)
    {
        _currentState = state;
        ChangeVisualState();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(ChangeCurrentState);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ChangeCurrentState);
    }

    private void ChangeCurrentState()
    {
        _currentState = !_currentState;
        ChangeVisualState();
        StateChanged?.Invoke(_currentState);
    }

    private void ChangeVisualState()
    {
        _icon.sprite = _currentState ? _onSprite : _offSprite;
    }
}