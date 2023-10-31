using UnityEngine;

namespace ShapeComparing.Shapes
{
    public class PolygonColliderShape : ColliderShape<PolygonCollider2D>
    {
        public PolygonColliderShape(PolygonCollider2D collider) : base(collider)
        { }

        public override bool IsPointInsideShape(Vector3 point)
        {
            var globalPoints = new Vector2[_collider.points.Length];

            for (int i = 0; i < _collider.points.Length; i++)
            {
                globalPoints[i] = _collider.transform.TransformPoint(_collider.points[i]);
            }

            return MathExtentions.IsPointInsidePolygon(point, globalPoints);
        }
    }
}