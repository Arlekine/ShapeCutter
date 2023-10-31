using System;
using Object = UnityEngine.Object;

namespace UnitySpriteCutter.Control.PostProcessing
{
    internal class DestroyGameObjectOnSizeLess : SizeLessActionPostProcessor
    {
        public DestroyGameObjectOnSizeLess(float squareToDestroy) : base(squareToDestroy)
        { }

        protected override Action<Cuttable> OnSizeLess => (Cuttable) => Object.Destroy(Cuttable.gameObject);
    }
}