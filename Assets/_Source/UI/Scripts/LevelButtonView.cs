using System;
using Levels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonView : MonoBehaviour
{
    [SerializeField] private GameObject _openedPart;
    [SerializeField] private GameObject _closedPart;

    [Space]
    [SerializeField] private TMP_Text _levelNumber;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _button;
    [SerializeField] private GameObject[] _stars;

    private Level _currentData;

    public Action<Level> Selected;

    public void Init(Level data, int levelIndex)
    {
        _currentData = data;

        _openedPart.SetActive(data.CurrentState != Level.State.Closed);
        _closedPart.SetActive(data.CurrentState == Level.State.Closed);

        _icon.sprite = data.Icon;
        _levelNumber.text = levelIndex.ToString();

        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].gameObject.SetActive(data.EarnedStars >= (i + 1));
        }
    }

    private void OnEnable() => _button.onClick.AddListener(OnClick);
    private void OnDisable() => _button.onClick.RemoveListener(OnClick);
    private void OnClick() => Selected?.Invoke(_currentData);
}