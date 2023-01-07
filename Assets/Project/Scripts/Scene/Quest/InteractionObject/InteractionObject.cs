using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class InteractionObject : CacheableGameObject, IInteractionObject
    {
        public IInteractData InteractData { get; private set; }

        public void SetInteractData(IInteractData interactData)
        {
            InteractData = interactData;
        }

        protected override void OnRelease()
        {
            InteractData.SetPosition(transform.position);
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