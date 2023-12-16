using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DottedDrawer
{
    public class DottedPolygonDrawer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _scrollSpeed;

        private Renderer renderer;
        private Vector2 savedOffset;

        void Start()
        {
            renderer = GetComponent<Renderer>();
        }

        void Update()
        {
            float x = Mathf.Repeat(Time.time * _scrollSpeed, 1);
            Vector2 offset = new Vector2(x, 0);
            renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
        }

        public void SetPolygon(Transform polygonCenter, Vector2[] localPoints)
        {
            transform.parent = polygonCenter;

            transform.localPosition = Vector3.zero + Vector3.back;
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
