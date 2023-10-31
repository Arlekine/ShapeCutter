using UnityEngine;

public class SingleSoundPlayer : SoundPlayer
{
    [SerializeField] private AudioClip _sound;
    [SerializeField] private bool _loop;

    public bool Loop
    {
        get => _loop;
        set => _loop = value;
    }

    public override AudioSource Play()
    {
        var source = AudioManager.PlaySound(_sound, _loop);
        source.volume = _volume;
        return source;
    }
}