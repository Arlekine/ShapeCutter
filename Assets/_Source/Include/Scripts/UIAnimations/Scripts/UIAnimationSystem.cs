using System;
using System.Collections;
using UnityEngine;

public class UIAnimationSystem : UiShowingAnimation
{
    [Serializable]
    private class AnimationStep
    {
        [SerializeField] private UiShowingAnimation[] _animations;
        [SerializeField] private float _stepDelay;

        public float StepDelay => _stepDelay;

        public void Show()
        {
            foreach (var uiShowingAnimation in _animations)
            {
                uiShowingAnimation.Show();
            }
        }

        public void ShowInstantly()
        {
            foreach (var uiShowingAnimation in _animations)
            {
                uiShowingAnimation.ShowInstantly();
            }
        }

        public void Hide()
        {
            foreach (var uiShowingAnimation in _animations)
            {
                uiShowingAnimation.Hide();
            }
        }

        public void HideInstantly()
        {
            foreach (var uiShowingAnimation in _animations)
            {
                uiShowingAnimation.HideInstantly();
            }
        }
    }

    [SerializeField] private AnimationStep[] _showAnimations;
    [SerializeField] private AnimationStep[] _hideAnimations;

    private Coroutine _currentAnimation;
    private bool _isShowed;

    public override bool IsShowed => _isShowed;

    public override void Show()
    {
        _isShowed = true;

        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
            _currentAnimation = null;
        }

        _currentAnimation = StartCoroutine(AnimationRoutine(_showAnimations, (step) => step.Show()));
    }

    public override void Hide()
    {
        _isShowed = false;

        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
            _currentAnimation = null;
        }
        
        _currentAnimation = StartCoroutine(AnimationRoutine(_hideAnimations, (step) => step.Hide()));
    }

    public override void ShowInstantly()
    {
        _isShowed = true;
        foreach (var step in _showAnimations)
        {
            step.ShowInstantly();
        }

        foreach (var step in _hideAnimations)
        {
            step.ShowInstantly();
        }
    }

    public override void HideInstantly()
    {
        _isShowed = false;
        foreach (var uiShowingAnimation in _showAnimations)
        {
            uiShowingAnimation.HideInstantly();
        }

        foreach (var step in _hideAnimations)
        {
            step.HideInstantly();
        }
    }

    private IEnumerator AnimationRoutine(AnimationStep[] steps, Action<AnimationStep> stepAction)
    {
        foreach (var step in steps)
        {
            stepAction(step);
            yield return new WaitForSeconds(step.StepDelay);
        }
    }
}