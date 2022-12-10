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