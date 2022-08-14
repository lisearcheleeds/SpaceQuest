using UnityEngine;

namespace RoboQuest.Quest.InSide
{
    public interface IActorAIState
    {
        ActorAIState ActorAIState { get; }
        ActorAIState Update(ActorAIHandler actorAIHandler);
    }
}