using AloneSpace;

namespace AloneSpace
{
    public class ActorAICheck : IActorAIState
    {
        public ActorAIState ActorAIState => ActorAIState.Check;
        
        public ActorAIState Update(QuestData questData, ActorData actorData, float deltaTime)
        {
            foreach (var target in actorData.ActorAIStateData.AroundTargets)
            {
                if (target.IsAlive && (target as ActorData)?.PlayerInstanceId != actorData.PlayerInstanceId)
                {
                    return ActorAIState.Fight;
                }
            }
            
            if (actorData.ActorAIStateData.MoveTarget != null)
            {
                return ActorAIState.Moving;
            }
            
            return ActorAIState.Sleep;
        }
    }
}