using UnityEngine;

namespace ShapeComparing.Shapes
{
    public class BoxColliderShape : ColliderShape<BoxCollider2D>
    {
        public BoxColliderShape(BoxCollider2D collider) : base(collider)
        { }

        public override bool IsPointInsideShape(Vector3 point)
        {
            var colliderCenter = _collider.transform.position;
            var sizePositiveTransformed = _collider.transform.TransformPoint(_collider.size * 0.5f);
            var sizeNegativeTransformed = _collider.transform.TransformPoint(-_collider.size * 0.5f);

            var isXInsideBox = point.x <= sizePositiveTransformed.x && point.x >= sizeNegativeTransformed.x;
            var isYInsideBox = point.y <= sizePositiveTransformed.y && point.y >= sizeNegativeTransformed.y;

            return isXInsideBox && isYInsideBox;
        }
    }
}