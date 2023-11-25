using System;
using Lean.Touch;
using UnityEngine;

namespace UnitySpriteCutter.Control
{
    public class LeanTouchLineInput : MonoBehaviour, ILineInput
    {
        private Vector2 _pressStart;
        private Vector2 _lastPointerPosition;

        private LeanFinger _currentFinger;

        public event Action<Vector2> Pressed;
        public event Action<Vector2, Vector2> Released;

        public bool IsPressed => _currentFinger != null;

        public Vector2 CurrentPointerPosition
        {
            get
            {
                if (IsPressed == false)
                    throw new Exception("Can't get pointer position, because pointer isn't pressed");

                return _currentFinger.LastScreenPosition;
            }
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            _currentFinger = null;

            LeanTouch.OnFingerDown += FingerPress;
            LeanTouch.OnFingerUp += FingerUp;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerDown -= FingerPress;
            LeanTouch.OnFingerUp -= FingerUp;
        }

        private void FingerPress(LeanFinger finger)
        {
            if (_currentFinger == null && finger.StartedOverGui == false)
            {
                _currentFinger = finger;
                _pressStart = Input.mousePosition;
                Pressed?.Invoke(_pressStart);
            }
        }

        private void FingerUp(LeanFinger finger)
        {
            if (_currentFinger == finger)
            {
                _currentFinger = null;
                Released?.Invoke(_pressStart, Input.mousePosition);
            }
        }
    }
}