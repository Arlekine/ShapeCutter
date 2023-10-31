using System.Collections;
using DG.Tweening;
using DottedDrawer;
using ShapeComparing;
using ShapeComparing.Shapes;
using UISystem;
using UnityEngine;
using UnityEngine.UI;
using UnitySpriteCutter;
using UnitySpriteCutter.Control;
using UnitySpriteCutter.Control.PostProcessing;

namespace Levels
{
    public class PrecisionLevelRoot : Level
    {
        [SerializeField] private ShapeCuttingTask _shapeCuttingTask;
        [SerializeField] private Cuttable _startingCuttable;
        [SerializeField] private PolygonCollider2D _targetPolygon;

        [Header("Comparing")]
        [SerializeField] private ShapeComparer _shapeComparer;
        [SerializeField] private Transform _comparePoint;
        [SerializeField] private float _finalMoveTime;

        [Space] 
        [SerializeField] private float _forceAfterCut = 5f;
        [SerializeField] private float _sizeToClearCuttable = 0.35f;

        [Header("Result")]
        [SerializeField] private float _precisionToPerfect = 0.95f;
        [SerializeField] private float _precisionToGood = 0.9f;
        [SerializeField] private float _precisionToPass = 0.85f;

        [Header("UI Elements")] 
        [SerializeField] private Button _compareButtonPrefab;

        private DottedPolygonDrawer _polygonDrawer;
        private Button _compareButton;

        public override void Init(UI ui)
        {
            var postProcessor = new ComplexCutPostProcessor();

            postProcessor.PostProcessors.Add(new BigPartStaticRigidbodyPostProcessor());
            postProcessor.PostProcessors.Add(new AddingForcePostProcessor(_forceAfterCut));
            postProcessor.PostProcessors.Add(new DeleteCuttableOnSizeLess(_sizeToClearCuttable));

            _shapeCuttingTask.Init();
            _shapeCuttingTask.StartNewCutting(new ShapeCuttingTask.ShapeCuttingInput() {CutPostProcessor = postProcessor, StartingCuttable = _startingCuttable, TargetPolygon = _targetPolygon});

            _compareButton = ui.CreateUIElement(_compareButtonPrefab, this);
            _compareButton.onClick.AddListener(CompleteLevel);
        }

        private void CompleteLevel()
        {
            _compareButton.onClick.RemoveListener(CompleteLevel);
            _compareButton.gameObject.SetActive(false);
            StartCoroutine(CompletionRoutine());
        }

        private IEnumerator CompletionRoutine()
        {
            var result = _shapeCuttingTask.Finish();

            var finalSequence = DOTween.Sequence();

            finalSequence.Join(result.FinalCuttable.transform.DOMove(_comparePoint.position, _finalMoveTime));
            finalSequence.Join(result.TargetShape.transform.DOMove(_comparePoint.position, _finalMoveTime));

            yield return finalSequence.WaitForCompletion();

            print(result.ComparingResult);

            var levelResult = LevelResult.Loose;

            if (result.ComparingResult >= _precisionToPerfect)
                levelResult = LevelResult.Perfect;
            else if (result.ComparingResult >= _precisionToGood)
                levelResult = LevelResult.Good;
            else if (result.ComparingResult >= _precisionToPass)
                levelResult = LevelResult.Pass;

            Completed?.Invoke(levelResult);
        }
    }
}