using AloneSpace;

namespace AloneSpace
{
    public class ActorAICheck : IActorAIState
    {
        public ActorAIState Update(ActorData actorData, float deltaTime)
        {
            foreach (var target in actorData.ActorStateData.AroundTargets)
            {
                if ((target as ActorData)?.PlayerInstanceId != actorData.PlayerInstanceId)
                {
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