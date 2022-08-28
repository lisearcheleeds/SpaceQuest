using RoboQuest;

namespace AloneSpace.InSide
{
    public class ActorAICheck : IActorAIState
    {
        public ActorAIState ActorAIState => ActorAIState.Check;
        
        public ActorAIState Update(ActorAIHandler actorAIHandler)
        {
            actorAIHandler.ActorData.SetInteract(null);
            
            foreach (var target in actorAIHandler.Targets)
            {
                if (target.TargetData.IsTargetable
                    && target is Actor targetActor
                    && targetActor.InstanceId != actorAIHandler.ActorData.InstanceId)
                {
                    return ActorAIState.Fight;
                }
            }
            
            if (actorAIHandler.ActorData.InteractOrder.Count != 0)
            {
                return ActorAIState.BeginMove;
            }
            
            return ActorAIState.Idle;
        }
    }
}