using System;
using DG.Tweening;
using UnityEngine;
using UnitySpriteCutter;

namespace Levels
{
    public abstract class CuttableShower : MonoBehaviour
    {
        [SerializeField] private Cuttable _cuttablePrefab;

        public (Tween animation, Cuttable cuttable) ShowNewCuttable(Transform parent)
        {
            var newCuttable = CreateNewCuttable(parent);
            return (AnimateEntrance(newCuttable), newCuttable);
        }

        public Tween RemoveCuttable(Cuttable cuttable)
        {
            var tween = AnimateExit(cuttable);
            tween.onComplete += () => { Destroy(cuttable.gameObject, 0.1f); };
            return tween;
        }

        private Cuttable CreateNewCuttable(Transform parent)
        {
            return Instantiate(_cuttablePrefab, parent);
        }

        protected abstract Tween AnimateEntrance(Cuttable cuttable);
        protected abstract Tween AnimateExit(Cuttable cuttable);
    }
}