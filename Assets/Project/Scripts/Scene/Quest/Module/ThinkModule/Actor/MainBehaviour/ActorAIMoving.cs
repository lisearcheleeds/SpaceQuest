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
            
            MessageBus.Instance.Actor.ForwardBoosterPowerRatio.Broadcast(actorData.InstanceId, 0.5f);
            MessageBus.Instance.Actor.YawBoosterPowerRatio.Broadcast(actorData.InstanceId, 0.5f);
            
            return ActorAIState.Moving;
        }
    }
}