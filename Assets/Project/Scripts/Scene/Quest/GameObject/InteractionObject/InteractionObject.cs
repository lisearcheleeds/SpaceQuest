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
            transform.position = InteractData.Position;
        }

        public void OnLateUpdate()
        {
            if (transform.position != InteractData.Position)
            {
                transform.position = InteractData.Position;
            }
        }
    }
}
