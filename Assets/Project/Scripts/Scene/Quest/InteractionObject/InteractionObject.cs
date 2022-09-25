using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public abstract class InteractionObject : CacheableGameObject, IInteractionObject
    {
        public Guid InstanceId => InteractData.InstanceId;
        
        public string Text => InteractData.Text;

        public abstract InteractionType InteractionType { get; }

        public abstract IInteractData InteractData { get; }

        protected Vector3 GetPlaceOffsetHeight()
        {
            var totalBounds = new Bounds(Vector3.zero, Vector3.zero);
            foreach (var collider in GetComponentsInChildren<Collider>())
            {
                totalBounds.Encapsulate(collider.bounds);
            }

            return Vector3.up * totalBounds.size.y * 0.5f;
        }
        
        void Update()
        {
            if (transform.hasChanged)
            {
                transform.hasChanged = false;
                InteractData.SetPosition(transform.position);
            }
        }
    }
}