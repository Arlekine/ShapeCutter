using System;
using System.Linq;
using UnityEngine;

namespace UnitySpriteCutter
{
    public class Cuttable : MonoBehaviour
    {
        private Renderer _renderer;
        private Rigidbody2D _rigidbody;

        public Renderer Renderer
        {
            get
            {
                if (_renderer == null)
                    _renderer = GetComponent<Renderer>();
                
                return _renderer;
            }
        }

        public Rigidbody2D Rigidbody
        {
            get
            {
                if (_rigidbody == null)
                    _rigidbody = GetComponent<Rigidbody2D>();

                return _rigidbody;
            }

            private set
            {
                _rigidbody = value;
            }
        }

        public Vector3 RealCenter => Renderer.bounds.center;
        public Vector3 RealCenterOffset => transform.position - Renderer.bounds.center;

        public Vector2 Size => Renderer.bounds.size;

        public float BoundsSquare
        {
            get
            {
                var polyCollider = GetComponent<PolygonCollider2D>();
                if (polyCollider != null)
                {
                    var points = new Vector2[polyCollider.points.Length].ToList();

                    for (int i = 0; i < polyCollider.points.Length; i++)
                    {
                        points[i] = transform.TransformPoint(polyCollider.points[i]);
                    }

                    points.Add(points[0]);

                    var area = Math.Abs(points.Take(points.Count - 1)
                        .Select((p, i) => (points[i + 1].x - p.x) * (points[i + 1].y + p.y))
                        .Sum() / 2);

                    return Mathf.Abs(area);
                }
                
                return Size.x * Size.y;
            }
        }

        public Rigidbody2D GetOrAddRigidbody()
        {
            if (Rigidbody == null)
            {
                Rigidbody = gameObject.AddComponent<Rigidbody2D>();
            }

            return Rigidbody;
        }
    }
}