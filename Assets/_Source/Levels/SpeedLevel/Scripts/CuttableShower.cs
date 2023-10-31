using System;
using DG.Tweening;
using UnityEngine;
using UnitySpriteCutter;

namespace Levels
{
    public abstract class CuttableShower : MonoBehaviour
    {
        [SerializeField] private Cuttable _cuttablePrefab;

        public (Tween animation, Cuttable cuttable) ShowNewCuttable()
        {
            var newCuttable = CreateNewCuttable();
            return (AnimateEntrance(newCuttable), newCuttable);
        }

        public Tween RemoveCuttable(Cuttable cuttable)
        {
            var tween = AnimateExit(cuttable);
            tween.onComplete += () => { Destroy(cuttable.gameObject); };
            return tween;
        }

        private Cuttable CreateNewCuttable()
        {
            return Instantiate(_cuttablePrefab);
        }

        protected abstract Tween AnimateEntrance(Cuttable cuttable);
        protected abstract Tween AnimateExit(Cuttable cuttable);
    }
}