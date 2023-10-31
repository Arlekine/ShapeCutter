using UnityEngine;

namespace UnitySpriteCutter.Control.PostProcessing
{
    internal class DynamicRigidbodyPostProcessor : CutPostProcessor
    {
        public override SpriteCutterOutput PostProcessCut(SpriteCutterOutput output)
        {
            var oldRigidbody = output.firstSide.GetOrAddRigidbody();
            var newRigidbody = output.secondSide.GetOrAddRigidbody();

            newRigidbody.velocity = oldRigidbody.velocity;

            newRigidbody.bodyType = RigidbodyType2D.Dynamic;
            oldRigidbody.bodyType = RigidbodyType2D.Dynamic;

            return output;
        }
    }
}