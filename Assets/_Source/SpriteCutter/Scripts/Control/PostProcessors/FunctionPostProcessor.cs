using System;

namespace UnitySpriteCutter.Control.PostProcessing
{
    internal class FunctionPostProcessor : CutPostProcessor
    {
        private Func<SpriteCutterOutput, SpriteCutterOutput> _postProcessor;

        public FunctionPostProcessor(Func<SpriteCutterOutput, SpriteCutterOutput> postProcessor)
        {
            _postProcessor = postProcessor;
        }

        public override SpriteCutterOutput PostProcessCut(SpriteCutterOutput output)
        {
            return _postProcessor(output);
        }
    }
}