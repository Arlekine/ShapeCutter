using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RunningNumberView : MonoBehaviour
{
    [SerializeField] private TMP_Text _numberText;
    [SerializeField] private float _fillTime = 1f;

    private Coroutine _currentRoutine;
    private int _lastTarget = 0;

    public void ShowNumber(int targetNumber, string textFormat)
    {
        ShowNumber(_lastTarget, targetNumber, textFormat);
    }

    [EditorButton]
    public void ShowNumber(int startNumber, int targetNumber, string textFormat)
    {
        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _lastTarget = targetNumber;

        if (startNumber < targetNumber)
            _currentRoutine = StartCoroutine(IncreasingRoutine(startNumber, targetNumber, textFormat));
        else
            _currentRoutine = StartCoroutine(DecreasingRoutine(startNumber, targetNumber, textFormat));
    }

    private IEnumerator IncreasingRoutine(int startNumber, int targetNumber, string textFormat)
    {
        float coinsPerSecond = (float)(targetNumber - startNumber) / _fillTime;
        float number = startNumber;

        while (number < targetNumber)
        {
            number += coinsPerSecond * Time.deltaTime;
            number = Mathf.Min(targetNumber, number);

            _numberText.text = String.Format(textFormat, ((int)number).ToString());

            yield return null;
        }
    }

    private IEnumerator DecreasingRoutine(int startNumber, int targetNumber, string textFormat)
    {
        float coinsPerSecond = (float)(startNumber - targetNumber) / _fillTime;
        float number = startNumber;

        while (number > targetNumber)
        {
            number -= coinsPerSecond * Time.deltaTime;
            number = Mathf.Max(targetNumber, number);

            _numberText.text = String.Format(textFormat, ((int)number).ToString());

            yield return null;
        }
    }
}
