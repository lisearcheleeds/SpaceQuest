using System.Linq;
using RoboQuest;
using UnityEngine;

namespace AloneSpace.InSide
{
    public class ActorAIBeginMove : IActorAIState
    {
        public ActorAIState ActorAIState => ActorAIState.BeginMove;
        
        public ActorAIState Update(ActorAIHandler actorAIHandler)
        {
            var nextOrder = actorAIHandler.ActorData.GetNextInteractOrder();
            var nextOrderObject = actorAIHandler.InteractionObjects?.FirstOrDefault(x => x.InteractData == nextOrder);

            if (nextOrder == null || nextOrderObject == null)
            {
                return ActorAIState.Check;
            }

            actorAIHandler.ActorPathFinder.CalculatePath(actorAIHandler.ActorData.Position, nextOrderObject.InteractData.GetClosestPoint(actorAIHandler.ActorData.Position));
            actorAIHandler.ActorPathFinder.Update(actorAIHandler.ActorData.Position);
            
            if (actorAIHandler.ActorPathFinder.HasPath)
            {
                return ActorAIState.Moving;
            }
            
            return ActorAIState.BeginMove;
        }
    }
}