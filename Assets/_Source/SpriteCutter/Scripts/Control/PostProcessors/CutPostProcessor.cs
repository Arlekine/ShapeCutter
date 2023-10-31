using Object = UnityEngine.Object;

namespace UnitySpriteCutter.Control.PostProcessing
{
    public abstract class CutPostProcessor
    {
        public abstract SpriteCutterOutput PostProcessCut(SpriteCutterOutput output);
    }
}