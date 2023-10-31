using DG.Tweening;
using UnityEngine;

public class ComplexUIAnimation : UiShowingAnimation
{
    [SerializeField] private UiShowingAnimation[] _animations;
    
    public override bool IsShowed {
        get 
        {
            foreach (var showingAnimation in _animations)
            {
                if (!showingAnimation.IsShowed)
                    return false;
            }

            return true;
        }
    }

    public override void Show()
    {
        foreach (var uiShowingAnimation in _animations)
        {
            uiShowingAnimation.Show();
        }
    }

    public override void Hide()
    {
        foreach (var uiShowingAnimation in _animations)
        {
            uiShowingAnimation.Hide();
        }
    }

    public override void ShowInstantly()
    {
        foreach (var uiShowingAnimation in _animations)
        {
            uiShowingAnimation.ShowInstantly();
        }
    }

    public override void HideInstantly()
    {
        foreach (var uiShowingAnimation in _animations)
        {
            uiShowingAnimation.HideInstantly();
        }
    }
}