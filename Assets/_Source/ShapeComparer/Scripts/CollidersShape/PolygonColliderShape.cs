using UnityEngine;

namespace ShapeComparing.Shapes
{
    public class PolygonColliderShape : ColliderShape<PolygonCollider2D>
    {
        private Vector2[] _points;

        public PolygonColliderShape(PolygonCollider2D collider) : base(collider)
        {
            _points = new Vector2[collider.points.Length];

            for (int i = 0; i < collider.points.Length; i++)
            {
                _points[i] = _collider.transform.TransformPoint(collider.points[i]);
            }
        }

        public override bool IsPointInsideShape(Vector3 point)
        {
            return MathExtentions.IsPointInsidePolygon(point, _points);
        }
    }
}