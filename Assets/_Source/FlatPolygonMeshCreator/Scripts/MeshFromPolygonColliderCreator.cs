using UnityEngine;

namespace FlatMeshCreation
{
    public class MeshFromPolygonColliderCreator : MonoBehaviour
    {
        [SerializeField] private PolygonCollider2D _polygonCollider;
        [SerializeField] private Material[] _materials;

        [EditorButton]
        public void CreateMeshOnThisObject()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

            if (meshFilter == null)
                meshFilter = gameObject.AddComponent<MeshFilter>();

            if (meshRenderer == null)
                meshRenderer = gameObject.AddComponent<MeshRenderer>();

            var meshCreator = new FlatMeshCreator();
            var mesh = meshCreator.Create(_polygonCollider.points);

            meshFilter.mesh = mesh;
            meshRenderer.material = _materials[Random.Range(0, _materials.Length)];
        }
    }
}