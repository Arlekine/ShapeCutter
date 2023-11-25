using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsView : MonoBehaviour
{
    [SerializeField] private SettingsButton _soundButton;
    [SerializeField] private SettingsButton _hapticButton;

    private Settings _settings;

    public void Init(Settings settings)
    {
        _settings = settings;
        _soundButton.Init(settings.SoundOn);
        _hapticButton.Init(settings.HapticOn);
    }

    private void OnEnable()
    {
        _soundButton.StateChanged += OnSoundChanged;
        _hapticButton.StateChanged += OnHapticOn;
    }

    private void OnDisable()
    {
        _soundButton.StateChanged -= OnSoundChanged;
        _hapticButton.StateChanged -= OnHapticOn;
    }

    private void OnSoundChanged(bool newState)
    {
        _settings.SetSound(newState);
    }

    private void OnHapticOn(bool newState)
    {
        _settings.SetHaptic(newState);
    }
}