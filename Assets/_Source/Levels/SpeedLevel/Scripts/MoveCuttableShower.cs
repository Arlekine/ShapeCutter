using DG.Tweening;
using UnityEngine;
using UnitySpriteCutter;

namespace Levels
{
    public class MoveCuttableShower : CuttableShower
    {
        [SerializeField] private Transform _centerPoint;
        [SerializeField] private Transform _fromPoint;
        [SerializeField] private Transform _toPoint;

        [Space]
        [SerializeField] private float _moveTime;

        protected override Tween AnimateEntrance(Cuttable cuttable)
        {
            cuttable.transform.position = _fromPoint.position;
            return cuttable.transform.DOMove(_centerPoint.position, _moveTime);
        }

        protected override Tween AnimateExit(Cuttable cuttable)
        {
            return cuttable.transform.DOMove(_toPoint.position, _moveTime);
        }
    }
}