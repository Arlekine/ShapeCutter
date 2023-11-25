using System;
using UnityEngine;

public class Settings
{
    public Action<bool> HapticChanged;
    public Action<bool> SoundChanged;

    private SettingsData _currentData;

    public bool SoundOn => _currentData.SoundOn;
    public bool HapticOn => _currentData.HapticOn;

    public Settings(SettingsData data)
    {
        _currentData = data;

        Haptic.IsHapticActive = _currentData.HapticOn;
        AudioListener.volume = _currentData.SoundOn ? 1f : 0f;
    }
    public void SetHaptic(bool isOn)
    {
        Haptic.IsHapticActive = isOn;

        _currentData.HapticOn = isOn;
        HapticChanged?.Invoke(isOn);
    }

    public void SetSound(bool isOn)
    {
        AudioListener.volume = isOn ? 1f : 0f;

        _currentData.SoundOn = isOn;
        SoundChanged?.Invoke(isOn);
    }
}