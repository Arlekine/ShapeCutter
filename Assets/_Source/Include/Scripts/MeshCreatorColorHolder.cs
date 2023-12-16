using FlatMeshCreation;
using UnityEngine;

public class MeshCreatorColorHolder : ShapeColorHolder
{
    [SerializeField] private MeshFromPolygonColliderCreator _meshCreator;

    public override Color GetColor()
    {
        _meshCreator.CreateMeshOnThisObject();
        return GetComponent<MeshRenderer>().material.color;
    }
}