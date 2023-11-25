using System;
using UnityEngine;

[Serializable]
public class SettingsData
{
    [SerializeField] private bool _soundOn = true;
    [SerializeField] private bool _hapticOn = true;

    public bool SoundOn
    {
        get => _soundOn;
        set => _soundOn = value;
    }

    public bool HapticOn
    {
        get => _hapticOn;
        set => _hapticOn = value;
    }
}