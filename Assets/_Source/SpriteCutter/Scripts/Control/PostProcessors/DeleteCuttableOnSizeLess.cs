using System;
using Object = UnityEngine.Object;

namespace UnitySpriteCutter.Control.PostProcessing
{
    internal class DeleteCuttableOnSizeLess : SizeLessActionPostProcessor
    {
        public DeleteCuttableOnSizeLess(float squareToDestroy) : base(squareToDestroy)
        { }

        protected override Action<Cuttable> OnSizeLess => Object.Destroy;
    }
}