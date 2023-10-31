using DG.Tweening;
using UnityEngine;

public class UIScaleAnimation : UiShowingAnimation
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _showScale;
    [SerializeField] private float _hideScale;
    [SerializeField] private float _animationTime;
    [SerializeField] private Ease _animationEase;
    [SerializeField] private bool _hideOnAwake;
    
    public override bool IsShowed { get; }
    
    private void Awake()
    {
        if (_hideOnAwake)
            HideInstantly();
    }
    
    public override void Show()
    {
        _target.DOScale(_showScale, _animationTime).SetEase(_animationEase);
    }

    public override void Hide()
    {
        _target.DOScale(_hideScale, _animationTime).SetEase(_animationEase);
    }

    public override void ShowInstantly()
    {
        _target.localScale = Vector3.one *_showScale;
    }

    public override void HideInstantly()
    {
        _target.localScale = Vector3.one * _hideScale;
    }
}