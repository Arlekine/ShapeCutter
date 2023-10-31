using System;
using TMPro;
using UnityEngine;

public class ProgressionView : MonoBehaviour
{
    public const string CounterFormat = "{0}/{1}";

    [SerializeField] private TMP_Text _text;

    public void SetCounter(int current, int max)
    {
        _text.text = String.Format(CounterFormat, current, max);
    }
}