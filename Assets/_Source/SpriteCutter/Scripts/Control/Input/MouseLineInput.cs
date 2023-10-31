using System;
using UnityEngine;

namespace UnitySpriteCutter.Control
{
    public class MouseLineInput : MonoBehaviour, ILineInput
    {
        private Vector2 _pressStart;
        private Vector2 _lastPointerPosition;

        public event Action<Vector2> Pressed;
        public event Action<Vector2, Vector2> Released;

        public bool IsPressed { get; private set; }

        public Vector2 CurrentPointerPosition 
        {
            get
            {
                if (IsPressed == false)
                    throw new Exception("Can't get pointer position, because pointer isn't pressed");

                return _lastPointerPosition;
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

        private void LateUpdate()
        {
            if (IsPressed == false && Input.GetMouseButtonDown(0))
            {
                IsPressed = true;
                _pressStart = Input.mousePosition;
                Pressed?.Invoke(_pressStart);
            }

            _lastPointerPosition = Input.mousePosition;
            
            if (IsPressed && Input.GetMouseButtonUp(0))
            {
                IsPressed = false;
                Released?.Invoke(_pressStart, Input.mousePosition);
            }
        }
    }
}