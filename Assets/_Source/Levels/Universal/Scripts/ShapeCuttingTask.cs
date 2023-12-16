using System;
using System.Collections;
using System.Collections.Generic;
using DottedDrawer;
using ShapeComparing;
using ShapeComparing.Shapes;
using UnityEngine;
using UnitySpriteCutter;
using UnitySpriteCutter.Control;
using UnitySpriteCutter.Control.PostProcessing;

namespace Levels
{
    public class ShapeCuttingTask : MonoBehaviour
    {
        public class ShapeCuttingInput
        {
            public Cuttable StartingCuttable;
            public PolygonCollider2D TargetPolygon;
            public ComplexCutPostProcessor CutPostProcessor;
            public ShapeColorHolder ColorRandomizer;
        }

        public class ShapeCuttingResult
        {
            public Cuttable FinalCuttable;
            public PolygonCollider2D TargetShape;
            public float ComparingResult;
        }

        [SerializeField] private LinecastCutter _linecastCutter;
        [SerializeField] private MouseInputRepresentationBehaviour _lineView;
        [SerializeField] private ParticleSystem _partDestractionFX;
        [SerializeField] private float _sizeToClearCuttable = 0.35f;

        [Space]
        [SerializeField] private DottedPolygonDrawer _polygonDrawerPrefab;
        [SerializeField] private ShapeComparer _shapeComparer;

        private ILineInput _lineInput;
        private Color _fxColor;
        private Cuttable _currentCuttable;
        private PolygonCollider2D _targetPolygon;
        private DottedPolygonDrawer _polygonDrawer;

        private List<GameObject> _allPieces = new List<GameObject>();

        private Coroutine _currentSimilarityCheck;

        public Action<Cuttable> Cutted;
        public Action BaseDestroyed;

        public void Init()
        {
            _lineInput = GetComponentInChildren<ILineInput>();
            _linecastCutter.Init(_lineInput);
            _lineView.Init(_lineInput);
            _polygonDrawer = Instantiate(_polygonDrawerPrefab);
        }

        public void StartNewCutting(ShapeCuttingInput input)
        {
            input.CutPostProcessor.PostProcessors.Add(new FunctionPostProcessor((input) =>
            {
                _allPieces.Add(input.firstSide.gameObject);
                _allPieces.Add(input.secondSide.gameObject);

                if (input.firstSide.BoundsSquare < input.secondSide.BoundsSquare)
                {
                    _currentCuttable = input.secondSide;
                    Destroy(input.firstSide);
                }
                else
                {
                    _currentCuttable = input.firstSide;
                    Destroy(input.secondSide);
                }

                if (input.firstSide.BoundsSquare <= _sizeToClearCuttable)
                {
                    Destroy(input.firstSide.gameObject);
                    CreateDestroyFX(input.firstSide.RealCenter);
                }

                if (input.secondSide.BoundsSquare <= _sizeToClearCuttable)
                {
                    Destroy(input.secondSide.gameObject);
                    CreateDestroyFX(input.secondSide.RealCenter);
                }

                StartCoroutine(InvokeRoutine());
                return input;
            }));
            
            _fxColor = input.ColorRandomizer.GetColor();
            ClearCurrentPieces();

            _linecastCutter.SetPostProcessor(input.CutPostProcessor);

            _currentCuttable = input.StartingCuttable;
            _polygonDrawer.SetPolygon(input.TargetPolygon.transform, input.TargetPolygon.points);

            _targetPolygon = input.TargetPolygon;

            ContinueCutting();
        }

        public float GetCurrentSimilarity()
        {
            var shape1 = new PolygonColliderShape(_currentCuttable.GetComponent<PolygonCollider2D>());
            var shape2 = new PolygonColliderShape(_targetPolygon);

            return _shapeComparer.GetShapesSimilarityPercentage(shape1, shape2);
        }

        public void GetCurrentSimilarityAsync(Action<float> onCompleted)
        {
            if (_currentSimilarityCheck != null)
            {
                StopCoroutine(_currentSimilarityCheck);
            }

            var shape1 = new PolygonColliderShape(_currentCuttable.GetComponent<PolygonCollider2D>());
            var shape2 = new PolygonColliderShape(_targetPolygon);

            _currentSimilarityCheck = StartCoroutine(_shapeComparer.GetShapesSimilarityPercentageAsync(shape1, shape2, onCompleted));
        }

        public void ClearCurrentPieces()
        {
            foreach (var piece in _allPieces)
            {
                Destroy(piece);
            }
        }

        public void StopCutting()
        {
            _lineInput.Disable();
            _linecastCutter.enabled = false;
            _lineView.enabled = false;
        }

        public void ContinueCutting()
        {
            _lineInput.Enable();
            _linecastCutter.enabled = true;
            _lineView.enabled = true;
        }

        public ShapeCuttingResult Finish()
        {
            StopCutting();

            var shape1 = new PolygonColliderShape(_currentCuttable.GetComponent<PolygonCollider2D>());
            var shape2 = new PolygonColliderShape(_targetPolygon);

            var similarity = _shapeComparer.GetShapesSimilarityPercentage(shape1, shape2);

            return new ShapeCuttingResult() { FinalCuttable = _currentCuttable, TargetShape = _targetPolygon, ComparingResult = similarity};
        }

        private void CreateDestroyFX(Vector3 position)
        {
            var fx = Instantiate(_partDestractionFX, position, Quaternion.identity);
            var main = fx.main;
            main.startColor = new ParticleSystem.MinMaxGradient(_fxColor);
        }

        private IEnumerator InvokeRoutine()
        {
            yield return null;

            if (_currentCuttable != null)
                Cutted?.Invoke(_currentCuttable);
            else
                BaseDestroyed?.Invoke();
        }
    }
}