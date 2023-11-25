using UnityEngine;

namespace UnitySpriteCutter
{
    [RequireComponent(typeof(Renderer))]
    public class Cuttable : MonoBehaviour
    {
        private Renderer _renderer;
        private Rigidbody2D _rigidbody;

        public Renderer Renderer
        {
            get
            {
                if (_renderer == null)
                    _renderer = GetComponent<Renderer>();
                
                return _renderer;
            }
        }

        public Rigidbody2D Rigidbody
        {
            get
            {
                if (_rigidbody == null)
                    _rigidbody = GetComponent<Rigidbody2D>();

                return _rigidbody;
            }

            private set
            {
                _rigidbody = value;
            }
        }

        public Vector3 RealCenter => Renderer.bounds.center;
        public Vector3 RealCenterOffset => transform.position - Renderer.bounds.center;
        public Vector2 Size => Renderer.bounds.size;
        public float BoundsSquare => Size.x * Size.y;

        public Rigidbody2D GetOrAddRigidbody()
        {
            if (Rigidbody == null)
            {
                Rigidbody = gameObject.AddComponent<Rigidbody2D>();
            }

            return Rigidbody;
        }
    }
}