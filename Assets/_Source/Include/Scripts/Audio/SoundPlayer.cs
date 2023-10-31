using UnityEngine;

public abstract class SoundPlayer : MonoBehaviour
{
    [Range(0, 1)][SerializeField] protected float _volume = 1f;

    public float Volume
    {
        get => _volume;
        set => _volume = value;
    }

    public abstract AudioSource Play();
}