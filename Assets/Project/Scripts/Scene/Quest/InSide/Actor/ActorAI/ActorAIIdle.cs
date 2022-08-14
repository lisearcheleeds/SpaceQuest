namespace RoboQuest.Quest.InSide
{
    public class ActorAIIdle : IActorAIState
    {
        public ActorAIState ActorAIState => ActorAIState.Idle;
        
        public ActorAIState Update(ActorAIHandler actorAIHandler)
        {
            return ActorAIState.Idle;
        }
    }
}