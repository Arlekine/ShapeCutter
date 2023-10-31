using System;
using UnityEngine;

namespace DoodleJump
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class TypedTrigger<T> : MonoBehaviour where T : Component
    {
        public event Action<T> TriggerEnter;
        public event Action<T> TriggerExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            var targetComponent = other.GetComponent<T>();

            if (targetComponent != null)
            {
                TriggerEnter?.Invoke(targetComponent);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var targetComponent = other.GetComponent<T>();

            if (targetComponent != null)
            {
                TriggerExit?.Invoke(targetComponent);
            }
        }
    }
}