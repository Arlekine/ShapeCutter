using UnityEngine;

namespace UnitySpriteCutter.Control
{
    [RequireComponent(typeof(LineRenderer))]
    public class MouseInputRepresentationBehaviour : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private ILineInput _lineInput;
        private Vector2 _mouseStart;

        public void Init(ILineInput input)
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineInput = input;
            _lineInput.Pressed += OnPressed;
        }

        private void OnPressed(Vector2 pressScreenPos)
        {
            _mouseStart = Camera.main.ScreenToWorldPoint(pressScreenPos);
        }

        private void OnEnable()
        {
            if (_lineRenderer != null)
                _lineRenderer.enabled = false;
        }

        private void Update()
        {
            if (_lineInput.IsPressed)
            {
                _lineRenderer.enabled = true;

                Vector2 mouseEnd = Camera.main.ScreenToWorldPoint(_lineInput.CurrentPointerPosition);

                _lineRenderer.SetPosition(0, _mouseStart);
                _lineRenderer.SetPosition(1, mouseEnd);
            }
            else
            {
                _lineRenderer.enabled = false;
            }
        }
    }
}
