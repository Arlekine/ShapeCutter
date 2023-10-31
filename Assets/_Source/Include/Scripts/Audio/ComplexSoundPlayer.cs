using UnityEngine;

public class ComplexSoundPlayer : SoundPlayer
{
    [SerializeField] private SoundPlayer[] _soundPlayers;

    public override AudioSource Play()
    {
        for (int i = 1; i < _soundPlayers.Length; i++)
        {
            _soundPlayers[i].Volume *= Volume; 
            _soundPlayers[i].Play();
        }

        _soundPlayers[0].Volume *= Volume;
        return _soundPlayers[0].Play();
    }
}