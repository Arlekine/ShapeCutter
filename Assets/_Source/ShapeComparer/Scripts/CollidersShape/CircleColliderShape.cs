using UnityEngine;

namespace ShapeComparing.Shapes
{
    public class CircleColliderShape : ColliderShape<CircleCollider2D>
    {
        public CircleColliderShape(CircleCollider2D collider) : base(collider)
        { }

        public override bool IsPointInsideShape(Vector3 point)
        {
            var colliderPosition = _collider.transform.position;
            var colliderScale = _collider.transform.lossyScale;

            var colliderCenter = colliderPosition + new Vector3(_collider.offset.x, _collider.offset.y, colliderPosition.z);
            var biggestScaleSize = colliderScale.x > colliderScale.y ? colliderScale.x : colliderScale.y;

            return Vector3.Distance(point, colliderCenter) <= _collider.radius * biggestScaleSize;
        }
    }
}