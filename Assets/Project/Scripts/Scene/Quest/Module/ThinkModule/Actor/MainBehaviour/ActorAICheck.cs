using AloneSpace;

namespace AloneSpace
{
    public class ActorAICheck : IActorAIState
    {
        public ActorAIState Update(ActorData actorData, float deltaTime)
        {
            var aroundTargets = MessageBus.Instance.GetActorRelationData.Unicast(actorData.InstanceId);
            foreach (var target in aroundTargets)
            {
                if (target.OtherActorData.PlayerInstanceId != actorData.PlayerInstanceId)
                {
                    MessageBus.Instance.Actor.SetMainTarget.Broadcast(actorData.InstanceId, target.OtherActorData);
                    return ActorAIState.Fight;
                }
            }

            if (actorData.ActorStateData.MoveTarget != null)
            {
                return ActorAIState.Moving;
            }

            return ActorAIState.Sleep;
        }
    }
}
