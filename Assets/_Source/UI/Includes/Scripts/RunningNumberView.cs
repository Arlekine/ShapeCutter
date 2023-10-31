using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class RunningNumberView : MonoBehaviour
{
    [SerializeField] private TMP_Text _numberText;
    [SerializeField] private float _fillTime = 1f;

    private Coroutine _currentRoutine;

    [EditorButton]
    public void ShowNumber(int startNumber, int targetNumber, string textFormat)
    {
        if (_currentRoutine != null)
            StopCoroutine(_currentRoutine);

        _currentRoutine = StartCoroutine(CoinsRoutine(startNumber, targetNumber, textFormat));
    }

    private IEnumerator CoinsRoutine(int startNumber, int targetNumber, string textFormat)
    {
        float coinsPerSecond = (float)targetNumber / _fillTime;
        float number = startNumber;

        while (number < targetNumber)
        {
            number += coinsPerSecond * Time.deltaTime;
            number = Mathf.Min(targetNumber, number);

            _numberText.text = String.Format(textFormat, ((int)number).ToString());

            yield return null;
        }
    }
}
