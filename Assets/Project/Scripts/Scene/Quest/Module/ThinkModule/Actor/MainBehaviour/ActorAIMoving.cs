using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ActorAIMoving : IActorAIState
    {
        public ActorAIState Update(ActorData actorData, float deltaTime)
        {
            if (actorData.ActorStateData.MoveTarget == null)
            {
                return ActorAIState.Check;
            }
            
            MessageBus.Instance.ActorCommandForwardBoosterPowerRatio.Broadcast(actorData.InstanceId, 0.5f);
            MessageBus.Instance.ActorCommandYawBoosterPowerRatio.Broadcast(actorData.InstanceId, 0.1f);
            
            return ActorAIState.Moving;
        }
    }
}