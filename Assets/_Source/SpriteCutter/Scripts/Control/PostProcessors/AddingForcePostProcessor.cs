using UnityEngine;

namespace UnitySpriteCutter.Control.PostProcessing
{
    internal class AddingForcePostProcessor : CutPostProcessor
    {
        private float _cuttedForce = 5f;

        public AddingForcePostProcessor(float cuttedForce)
        {
            _cuttedForce = cuttedForce;
        }

        public override SpriteCutterOutput PostProcessCut(SpriteCutterOutput output)
        {
            var firstCenter = output.firstSide.RealCenter;
            var secondCenter = output.secondSide.RealCenter;

            var oldRigidbody = output.firstSide.GetOrAddRigidbody();
            var newRigidbody = output.secondSide.GetOrAddRigidbody();

            newRigidbody.AddForce((secondCenter - firstCenter).normalized * _cuttedForce, ForceMode2D.Impulse);
            oldRigidbody.AddForce((firstCenter - secondCenter).normalized * _cuttedForce, ForceMode2D.Impulse);

            return output;
        }
    }
}