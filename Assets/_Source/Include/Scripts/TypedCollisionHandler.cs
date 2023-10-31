using System;
using UnityEngine;

namespace DoodleJump
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class TypedCollisionHandler<T> : MonoBehaviour where T : Component
    {
        public event Action<T, Collision2D> CollisionEnter;
        public event Action<T, Collision2D> CollisionExit;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var targetComponent = collision.collider.GetComponent<T>();

            if (targetComponent != null)
            {
                CollisionEnter?.Invoke(targetComponent, collision);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            var targetComponent = collision.collider.GetComponent<T>();

            if (targetComponent != null)
            {
                CollisionExit?.Invoke(targetComponent, collision);
            }
        }
    }
}