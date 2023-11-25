using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using ShapeComparing.Shapes;
using UnityEngine;

namespace ShapeComparing
{
    [Serializable]
    public class ShapeComparer
    {
        private class ShapeComparingZone
        {
            private readonly bool[,] _isInsideShape;
            private Vector3 _zoneCenter;
            private Vector2 _zoneSize;

            private float MinBorderX => _zoneCenter.x - _zoneSize.x * 0.5f;
            private float MaxBorderX => _zoneCenter.x + _zoneSize.x * 0.5f;

            private float MinBorderY => _zoneCenter.y - _zoneSize.y * 0.5f;
            private float MaxBorderY => _zoneCenter.y + _zoneSize.y * 0.5f;

            public int Width => _isInsideShape.GetLength(0);
            public int Height => _isInsideShape.GetLength(1);

            public bool this[int x, int y]
            {
                get => _isInsideShape[x, y];
                set => _isInsideShape[x, y] = value;
            }

            public Vector3 GetPositionAtZonePoint(int x, int y)
            {
                var xPosition = Mathf.Lerp(MinBorderX, MaxBorderX, (float)x / (float)Width);
                var yPosition = Mathf.Lerp(MinBorderY, MaxBorderY, (float)y / (float)Height);

                return new Vector3(xPosition, yPosition, _zoneCenter.z);
            }

            public ShapeComparingZone(Vector3 zoneCenter, Vector2 zoneSize, float comparingPointsPerMeter)
            {
                _zoneCenter = zoneCenter;
                _zoneSize = zoneSize;

                var zoneWidth = (int)(zoneSize.x * comparingPointsPerMeter);
                var zoneHeight = (int)(zoneSize.y * comparingPointsPerMeter);

                _isInsideShape = new bool[zoneWidth, zoneHeight];
            }
        }

        [SerializeField] private int _comparingPointsInMeter;
        [SerializeField] private int _maxCalculationsPerFrame = 1000;

        private Coroutine _currentCalculation;

        public float GetShapesSimilarityPercentage(Shape shape1, Shape shape2)
        {
            var zonesSize = shape1.BoundsSquare > shape2.BoundsSquare ? shape1.BoundsSize : shape2.BoundsSize;

            var shapeComparingZone1 = new ShapeComparingZone(shape1.BoundsCenter, zonesSize, _comparingPointsInMeter);
            var shapeComparingZone2 = new ShapeComparingZone(shape2.BoundsCenter, zonesSize, _comparingPointsInMeter);

            WriteShapeIntersectionIntoZone(shape1, shapeComparingZone1);
            WriteShapeIntersectionIntoZone(shape2, shapeComparingZone2);

            float samePointsCount = 0;
            float allPointsCount = 0;

            for (int x = 0; x < shapeComparingZone1.Width; x++)
            {
                for (int y = 0; y < shapeComparingZone1.Height; y++)
                {
                    if (shapeComparingZone1[x, y] || shapeComparingZone2[x, y])
                        allPointsCount++;

                    if (shapeComparingZone1[x, y] && shapeComparingZone2[x, y])
                        samePointsCount++;
                }
            }

            return (samePointsCount / allPointsCount);
        }

        public IEnumerator GetShapesSimilarityPercentageAsync(Shape shape1, Shape shape2, Action<float> onCompared)
        {
            return CalculateSimilarityRoutine(shape1, shape2, onCompared);
        }

        private IEnumerator CalculateSimilarityRoutine(Shape shape1, Shape shape2, Action<float> onCompared)
        {
            var zonesSize = shape1.BoundsSquare > shape2.BoundsSquare ? shape1.BoundsSize : shape2.BoundsSize;

            var shapeComparingZone1 = new ShapeComparingZone(shape1.BoundsCenter, zonesSize, _comparingPointsInMeter);
            var shapeComparingZone2 = new ShapeComparingZone(shape2.BoundsCenter, zonesSize, _comparingPointsInMeter);

            float currentCalculationsCount = 0;

            for (int x = 0; x < shapeComparingZone1.Width; x++)
            {
                for (int y = 0; y < shapeComparingZone1.Height; y++)
                {
                    shapeComparingZone1[x, y] = shape1.IsPointInsideShape(shapeComparingZone1.GetPositionAtZonePoint(x, y));

                    if (currentCalculationsCount > _maxCalculationsPerFrame)
                    {
                        currentCalculationsCount = 0;
                        yield return null;
                    }
                }
            }

            for (int x = 0; x < shapeComparingZone2.Width; x++)
            {
                for (int y = 0; y < shapeComparingZone2.Height; y++)
                {
                    shapeComparingZone2[x, y] = shape2.IsPointInsideShape(shapeComparingZone2.GetPositionAtZonePoint(x, y));

                    if (currentCalculationsCount > _maxCalculationsPerFrame)
                    {
                        currentCalculationsCount = 0;
                        yield return null;
                    }
                }
            }

            float samePointsCount = 0;
            float allPointsCount = 0;

            for (int x = 0; x < shapeComparingZone1.Width; x++)
            {
                for (int y = 0; y < shapeComparingZone1.Height; y++)
                {
                    if (shapeComparingZone1[x, y] || shapeComparingZone2[x, y])
                        allPointsCount++;

                    if (shapeComparingZone1[x, y] && shapeComparingZone2[x, y])
                        samePointsCount++;
                }
            }

            var similarity = samePointsCount / allPointsCount;

            onCompared?.Invoke(similarity);
        }

        private void WriteShapeIntersectionIntoZone(Shape shape, ShapeComparingZone zone)
        {
            for (int x = 0; x < zone.Width; x++)
            {
                for (int y = 0; y < zone.Height; y++)
                {
                    zone[x, y] = shape.IsPointInsideShape(zone.GetPositionAtZonePoint(x, y));
                }
            }
        }
    }
}
