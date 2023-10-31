using ShapeComparing.Shapes;
using UnityEngine;

public class ShapeHolder : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D _polygonCollider;

    public Shape GetShape()
    {
        return new PolygonColliderShape(_polygonCollider);
    }
}
