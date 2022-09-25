using AloneSpace;

namespace AloneSpace
{
    public class ActorAIIdle : IActorAIState
    {
        public ActorAIState ActorAIState => ActorAIState.Idle;
        
        public ActorAIState Update(QuestData questData, ActorData actorData, float deltaTime)
        {
            return ActorAIState.Check;
        }
    }
}