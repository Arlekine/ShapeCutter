using System.Collections;
using UnityEngine;

public class StarsAnimation : MonoBehaviour
{
    [SerializeField] private UiShowingAnimation[] _stars;
    [SerializeField] private SoundPlayer _soundPlayer;
    [SerializeField] private float _naturalDelay;
    [SerializeField] private float _delayBetweenStars;

    public void Show(int starsToShow)
    {
        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].Hide();
        }

        StartCoroutine(ShowRoutine(starsToShow));
    }

    private IEnumerator ShowRoutine(int starsToShow)
    {
        yield return new WaitForSeconds(_naturalDelay);

        for (int i = 0; i < starsToShow; i++)
        {
            _stars[i].Show();
            _soundPlayer.Play();
             yield return new WaitForSeconds(_delayBetweenStars);
        }
    }
}