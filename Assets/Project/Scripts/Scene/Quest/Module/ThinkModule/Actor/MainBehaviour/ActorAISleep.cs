using AloneSpace;

namespace AloneSpace
{
    public class ActorAISleep : IActorAIState
    {
        float sleepTime = 3.0f;
        float currentSleepTime;
        
        public ActorAIState Update(ActorData actorData, float deltaTime)
        {
            currentSleepTime += deltaTime;
            
            if (currentSleepTime < sleepTime)
            {
                return ActorAIState.Sleep;                
            }

            currentSleepTime = 0;
            return ActorAIState.Check;
        }
    }
}