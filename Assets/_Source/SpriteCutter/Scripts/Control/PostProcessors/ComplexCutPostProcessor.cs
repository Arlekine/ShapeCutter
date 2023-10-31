using System.Collections.Generic;

namespace UnitySpriteCutter.Control.PostProcessing
{
    public class ComplexCutPostProcessor : CutPostProcessor
    {
        public readonly List<CutPostProcessor> PostProcessors = new List<CutPostProcessor>();

        public override SpriteCutterOutput PostProcessCut(SpriteCutterOutput output)
        {
            foreach (var cutPostProcessor in PostProcessors)
            {
                output = cutPostProcessor.PostProcessCut(output);
            }

            return output;
        }
    }
}