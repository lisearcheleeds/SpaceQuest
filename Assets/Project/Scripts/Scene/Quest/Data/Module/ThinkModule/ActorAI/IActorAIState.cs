using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public interface IActorAIState
    {
        ActorAIState Update(ActorData actorData, float deltaTime);
    }
}