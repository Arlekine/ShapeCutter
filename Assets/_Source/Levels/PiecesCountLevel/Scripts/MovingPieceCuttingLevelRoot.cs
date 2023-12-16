using UISystem;
using UnityEngine;
using UnitySpriteCutter.Control.PostProcessing;

namespace Levels
{
    public class MovingPieceCuttingLevelRoot : PiecesCountLevelRoot
    {
        [Space]
        [SerializeField] private Transform _leftBorderPoint;
        [SerializeField] private Transform _rightBorderPoint;
        [SerializeField] private float _moveSpeed;

        private Transform _movingTransform;
        private float _currentDirection;

        public override void Init(UI ui, TutorialHolder tutorial)
        {
            base.Init(ui, tutorial);
            _currentDirection = 1;
            _movingTransform = _startCuttable.transform;
        }

        protected override void SetAdditionalCutPostProcessors(ComplexCutPostProcessor complexCutPostProcessor)
        {
            complexCutPostProcessor.PostProcessors.Add(new FunctionPostProcessor((input) =>
            {
                if (_movingTransform == input.firstSide.transform || _movingTransform == input.secondSide.transform)
                {
                    _movingTransform = input.firstSide.BoundsSquare <= input.secondSide.BoundsSquare
                        ? input.secondSide.transform
                        : input.firstSide.transform;
                }

                return input;
            }));
        }

        private void Update()
        {
            if (_movingTransform == null)
                return;

            if (_currentDirection < 0 && _movingTransform.position.x <= _leftBorderPoint.position.x)
                _currentDirection = 1;
            else if (_currentDirection > 0 && _movingTransform.position.x >= _rightBorderPoint.position.x)
                _currentDirection = -1;

            var pos = _movingTransform.position;
            pos.x += _currentDirection * _moveSpeed * Time.deltaTime;
            _movingTransform.position = pos;
        }
    }
}