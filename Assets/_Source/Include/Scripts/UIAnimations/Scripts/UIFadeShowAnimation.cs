using System;
using DG.Tweening;
using UnityEngine;

public class UIFadeShowAnimation : UiShowingAnimation
{
    [SerializeField] protected CanvasGroup _canvasGroup;
    [SerializeField] protected float _showTime = 0.3f;
    [SerializeField] protected bool _switchInteractable = true;
    [SerializeField] protected bool _hideOnAwake = true;
    [SerializeField] protected bool _reversed;

    public override bool IsShowed => _canvasGroup.alpha > 0;

    private void Awake()
    {
        if (_hideOnAwake)
            HideInstantly();
    }

    public override void Show()
    {
        SetCanvasGroupInteractable(true);
        _canvasGroup.DOFade(_reversed ? 0f : 1f, _showTime);
    }

    public override void Hide()
    {
        SetCanvasGroupInteractable(false);
        _canvasGroup.DOFade(_reversed ? 1f : 0f, _showTime);
    }

    public override void ShowInstantly()
    {
        SetCanvasGroupInteractable(true);
        _canvasGroup.alpha = _reversed ? 0f : 1f;
    }

    public override void HideInstantly()
    {
        SetCanvasGroupInteractable(false);
        _canvasGroup.alpha = _reversed ? 1f : 0f;
    }

    private void SetCanvasGroupInteractable(bool isInteractable)
    {
        if (_switchInteractable)
        {
            _canvasGroup.interactable = isInteractable;
            _canvasGroup.blocksRaycasts = isInteractable;
        }
    }
}