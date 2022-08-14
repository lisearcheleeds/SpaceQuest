using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace RoboQuest.Quest.InSide
{
    public class AreaTransitionObject : InteractionObject
    {
        public override IInteractData InteractData => AreaTransitionInteractData;
        public override InteractionType InteractionType => InteractionType.AreaTransition;

        public AreaTransitionInteractData AreaTransitionInteractData { get; private set; }

        public void Apply(AreaTransitionInteractData areaTransitionInteractData)
        {
            AreaTransitionInteractData = areaTransitionInteractData;
            
            transform.position = areaTransitionInteractData.Position;
            transform.rotation = areaTransitionInteractData.Rotation;
            transform.localScale = new Vector3(areaTransitionInteractData.Scale, 1.0f, 1.0f);
            
            MessageBus.Instance.SendInteractionObject.Broadcast(this, true);
        }

        protected override void OnRelease()
        {
            MessageBus.Instance.SendInteractionObject.Broadcast(this, false);
        }
    }
}
