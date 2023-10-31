using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LevelResultProgress : MonoBehaviour
{
    [SerializeField] private RectTransform _mainParent;
    [SerializeField] private LevelResultProgressStar _oneStar;
    [SerializeField] private LevelResultProgressStar _twoStar;
    [SerializeField] private LevelResultProgressStar _threeStar;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private float _changeDuration;

    [EditorButton]
    public void Init(float oneStarTargetProgress, float twoStarTargetProgress)
    {
        _oneStar.Init(oneStarTargetProgress, _mainParent.sizeDelta.x);
        _twoStar.Init(twoStarTargetProgress, _mainParent.sizeDelta.x);
        _threeStar.Init(1f, _mainParent.sizeDelta.x);

        _progressSlider.onValueChanged.AddListener(UpdateStars);
    }

    [EditorButton]
    public void SetValue(float normalized)
    {
        if (normalized < 0 || normalized > 1)
            throw new ArgumentException("Value should be between 0 and 1");

        _progressSlider.DOValue(normalized, _changeDuration);
    }

    private void UpdateStars(float value)
    {
        print("!!");
        _oneStar.UpdateSelection(value);
        _twoStar.UpdateSelection(value);
        _threeStar.UpdateSelection(value);
    }
}