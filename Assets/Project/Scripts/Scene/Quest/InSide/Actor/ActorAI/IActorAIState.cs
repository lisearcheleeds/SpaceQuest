using RoboQuest;
using UnityEngine;

namespace AloneSpace.InSide
{
    public interface IActorAIState
    {
        ActorAIState ActorAIState { get; }
        ActorAIState Update(ActorAIHandler actorAIHandler);
    }
}