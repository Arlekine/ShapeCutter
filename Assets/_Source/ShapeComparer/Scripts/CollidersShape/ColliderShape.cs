using UnityEngine;

namespace ShapeComparing.Shapes
{
    public abstract class ColliderShape<T> : Shape where T : Collider2D
    {
        protected T _collider;

        public ColliderShape(T collider)
        {
            _collider = collider;
        }

        public override Vector2 BoundsSize => _collider.bounds.size;
        public override Vector2 BoundsCenter => _collider.bounds.center;
    }
}