using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public interface IActorAIState
    {
        ActorAIState ActorAIState { get; }
        ActorAIState Update(QuestData questData, ActorData actorData, float deltaTime);
    }
}