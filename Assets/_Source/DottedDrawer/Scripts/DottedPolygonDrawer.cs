using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DottedDrawer
{
    public class DottedPolygonDrawer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        public void SetPolygon(Transform polygonCenter, Vector2[] localPoints)
        {
            transform.parent = polygonCenter;

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            _lineRenderer.positionCount = localPoints.Length;

            for (int i = 0; i < localPoints.Length; i++)
            {
                _lineRenderer.SetPosition(i, new Vector3(localPoints[i].x, localPoints[i].y, 0f));
            }
        }
    }
}
