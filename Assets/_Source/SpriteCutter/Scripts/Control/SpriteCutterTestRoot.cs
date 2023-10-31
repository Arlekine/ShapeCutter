using UnityEngine;
using UnitySpriteCutter.Control.PostProcessing;

namespace UnitySpriteCutter.Control
{
    internal class SpriteCutterTestRoot : MonoBehaviour
    {
        [SerializeField] private LinecastCutter _linecastCutter;
        [SerializeField] private MouseInputRepresentationBehaviour _lineView;
        [SerializeField] private MouseLineInput _lineInput;
        [SerializeField] private float _forceAfterCut = 5f;
        [SerializeField] private float _sizeToClearCuttable = 0.35f;

        private void Awake()
        {
            var postProcessor = new ComplexCutPostProcessor();

            postProcessor.PostProcessors.Add(new BigPartStaticRigidbodyPostProcessor());
            postProcessor.PostProcessors.Add(new AddingForcePostProcessor(_forceAfterCut));
            postProcessor.PostProcessors.Add(new DeleteCuttableOnSizeLess(_sizeToClearCuttable));

            _linecastCutter.Init(_lineInput);
            _linecastCutter.SetPostProcessor(postProcessor);
            _lineView.Init(_lineInput);
        }
    }
}