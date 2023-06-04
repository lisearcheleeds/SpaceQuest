using System;
using UnityEngine;

namespace AloneSpace
{
    public class ModelCollider : MonoBehaviour
    {
        public Collider CurrentCollider { get; private set; }
        public CollisionEventModule CollisionEventModule { get; private set; }
        public int Index { get; private set; }

        Action<int, Collider> onTriggerEnter;
        Action<int, Collider> onTriggerStay;
        Action<int, Collider> onTriggerExit;

        public void Init(CollisionEventModule collisionEventModule, int index, Action<int, Collider> onTriggerEnter, Action<int, Collider> onTriggerStay, Action<int, Collider> onTriggerExit)
        {
            CurrentCollider = GetComponent<Collider>();
            CollisionEventModule = collisionEventModule;
            Index = index;

            this.onTriggerEnter = onTriggerEnter;
            this.onTriggerStay = onTriggerStay;
            this.onTriggerExit = onTriggerExit;

            if (collisionEventModule == null && CurrentCollider != null)
            {
                CurrentCollider.enabled = false;
            }
        }

        void OnTriggerEnter(Collider other) => onTriggerEnter?.Invoke(Index, other);
        void OnTriggerStay(Collider other) => onTriggerStay?.Invoke(Index, other);
        void OnTriggerExit(Collider other) => onTriggerExit?.Invoke(Index, other);
    }
}