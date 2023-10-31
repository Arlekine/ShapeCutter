using System;
using UnityEngine;

namespace UnitySpriteCutter.Control.PostProcessing
{
    internal abstract class SizeLessActionPostProcessor : CutPostProcessor
    {
        private float _squareToDestroy = 0.35f;

        protected abstract Action<Cuttable> OnSizeLess { get; }

        public SizeLessActionPostProcessor(float squareToDestroy)
        {
            _squareToDestroy = squareToDestroy;
        }

        public override SpriteCutterOutput PostProcessCut(SpriteCutterOutput output)
        {
            if (output.firstSide.BoundsSquare <= _squareToDestroy)
                OnSizeLess?.Invoke(output.firstSide);

            if (output.secondSide.BoundsSquare <= _squareToDestroy)
                OnSizeLess?.Invoke(output.secondSide);

            return output;
        }
    }
}