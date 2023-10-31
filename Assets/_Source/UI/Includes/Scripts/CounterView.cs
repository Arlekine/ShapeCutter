using TMPro;
using UnityEngine;

public class CounterView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void SetCounter(int current)
    {
        _text.text = current.ToString();
    }
}