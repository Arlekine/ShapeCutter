using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatMeshCreation
{
    public class FlatMeshCreator
	{
        public Mesh Create(Vector2[] flatVertices)
        {
            Vector3[] newVertices = new Vector3[flatVertices.Length];
            for (int i = 0; i < flatVertices.Length; i++)
            {
                newVertices[i] = (Vector3)flatVertices[i];
            }

            Mesh result = new Mesh();

            if (newVertices.Length < 3)
            {
                throw new System.Exception("Cannot generate mesh from less than 3 vertices!");
            }

            result.vertices = newVertices;
            result.triangles = GenerateConvexPolygonTrianglesFromVertices(newVertices);

            result.Optimize();
            result.RecalculateNormals();

            return result;
        }
        
        private int[] GenerateConvexPolygonTrianglesFromVertices(Vector3[] vertices)
        {
            if (vertices.Length == 3)
            {
                return new int[] { 0, 1, 2 };
            }

            List<int> result = new List<int>();
            for (int i = 2; i < vertices.Length; i++)
            {
                result.Add(0);
                result.Add(i - 1);
                result.Add(i);
            }

            return result.ToArray();
        }
    }
}
