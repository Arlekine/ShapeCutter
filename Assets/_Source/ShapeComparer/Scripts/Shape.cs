using System;
using UnityEngine;

namespace ShapeComparing.Shapes
{
    public abstract class Shape
    {
        public abstract Vector2 BoundsSize  { get; }
        public abstract Vector2 BoundsCenter { get; }
        public abstract bool IsPointInsideShape(Vector3 point);

        public float BoundsSquare => BoundsSize.x * BoundsSize.y;
    }
}