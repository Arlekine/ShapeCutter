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

    private bool _isShowed;

    public override bool IsShowed => _isShowed;
    
    private void Awake()
    {
        if (_hideOnAwake)
            HideInstantly();
    }
    
    public override void Show()
    {
        _isShowed = true;
        _target.DOScale(_showScale, _animationTime).SetEase(_animationEase);
    }

    public override void Hide()
    {
        _isShowed = false;
        _target.DOScale(_hideScale, _animationTime).SetEase(_animationEase);
    }

    public override void ShowInstantly()
    {
        _isShowed = true;
        _target.localScale = Vector3.one *_showScale;
    }

    public override void HideInstantly()
    {
        _isShowed = false;
        _target.localScale = Vector3.one * _hideScale;
    }
}