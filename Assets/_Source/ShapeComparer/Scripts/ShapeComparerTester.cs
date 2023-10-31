using ShapeComparing.Shapes;
using UnityEngine;

namespace ShapeComparing
{
    public class ShapeComparerTester : MonoBehaviour
    {
        [SerializeField] private ShapeComparer _shapeComparer;

        [Space]
        [SerializeField] private BoxCollider2D _collider1;
        [SerializeField] private BoxCollider2D _collider2;

        [EditorButton]
        private void TestShape()
        {
            var shape1 = new BoxColliderShape(_collider1);
            var shape2 = new BoxColliderShape(_collider2);
            
            var similarity =  _shapeComparer.GetShapesSimilarityPercentage(shape1, shape2);

            print(similarity);
        }
    }
}