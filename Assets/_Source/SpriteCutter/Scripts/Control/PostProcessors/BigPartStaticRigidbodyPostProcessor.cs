using UnityEngine;

namespace UnitySpriteCutter.Control.PostProcessing
{
    internal class BigPartStaticRigidbodyPostProcessor : CutPostProcessor
    {
        public override SpriteCutterOutput PostProcessCut(SpriteCutterOutput output)
        {
            var oldRigidbody = output.firstSide.GetOrAddRigidbody();
            var newRigidbody = output.secondSide.GetOrAddRigidbody();

            if (output.firstSide.BoundsSquare > output.secondSide.BoundsSquare)
            {
                newRigidbody.bodyType = RigidbodyType2D.Dynamic;
                newRigidbody.gravityScale = oldRigidbody.gravityScale;
            }
            else
            {
                newRigidbody.bodyType = oldRigidbody.bodyType;
                oldRigidbody.bodyType = RigidbodyType2D.Dynamic;
                newRigidbody.gravityScale = oldRigidbody.gravityScale;
            }

            return output;
        }
    }
}