using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class ActorCollisionEventEffectReceiverModule : CollisionEventEffectReceiverModule
    {
        ActorData actorData;

        public ActorCollisionEventEffectReceiverModule(Guid instanceId, ActorData actorData) : base(instanceId)
        {
            this.actorData = actorData;
        }

        public override void OnUpdateModule(float deltaTime, HashSet<CollisionEventEffectSenderModule> senderList)
        {
            Debug.Log("ダメージを受けた" + senderList.Count);
        }
    }
}