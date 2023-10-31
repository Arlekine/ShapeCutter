using UnityEngine;
using System.Collections.Generic;
using UnitySpriteCutter.Control.PostProcessing;

namespace UnitySpriteCutter.Control
{
    internal class LinecastCutter : MonoBehaviour
    {
        [SerializeField] private LayerMask _cuttableLayer;

        private ILineInput _lineInput;
        private CutPostProcessor _cutPostProcessor;

        public void Init(ILineInput lineInput)
        {
            _lineInput = lineInput;

            _lineInput.Released += OnDrawLine;
        }

        public void SetPostProcessor(CutPostProcessor cutPostProcessor)
        {
            _cutPostProcessor = cutPostProcessor;
        }

        private void OnDrawLine(Vector2 startScreenPos, Vector2 endScreenPos)
        {
            LinecastCut(Camera.main.ScreenToWorldPoint(startScreenPos), Camera.main.ScreenToWorldPoint(endScreenPos), _cuttableLayer.value);
        }

        private void LinecastCut(Vector2 lineStart, Vector2 lineEnd, int layerMask = Physics2D.AllLayers)
        {
            List<Cuttable> gameObjectsToCut = new List<Cuttable>();
            RaycastHit2D[] hits = Physics2D.LinecastAll(lineStart, lineEnd, layerMask);

            foreach (RaycastHit2D hit in hits)
            {
                var cuttable = hit.collider.GetComponent<Cuttable>();
                if (cuttable)
                {
                    gameObjectsToCut.Add(cuttable);
                }
            }

            foreach (var cuttable in gameObjectsToCut)
            {
                SpriteCutter.Cut(new SpriteCutterInput(lineStart, lineEnd, cuttable), (output) => _cutPostProcessor.PostProcessCut(output));
            }
        }
    }
}
