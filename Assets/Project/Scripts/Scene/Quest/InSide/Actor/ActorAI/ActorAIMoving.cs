using System.Linq;
using RoboQuest;
using UnityEngine;

namespace AloneSpace.InSide
{
    public class ActorAIMoving : IActorAIState
    {
        public ActorAIState ActorAIState => ActorAIState.Moving;
        
        public ActorAIState Update(ActorAIHandler actorAIHandler)
        {
            if (!actorAIHandler.ActorPathFinder.HasPath)
            {   
                return ActorAIState.BeginMove;
            }

            actorAIHandler.ActorPathFinder.Update(actorAIHandler.ActorData.Position);
            var nextPosition = actorAIHandler.ActorPathFinder.NextPosition;
            
            if (nextPosition.HasValue)
            {
                var relativePosition = nextPosition.Value - actorAIHandler.ActorData.Position;
                var wayDirection = new Vector3(relativePosition.x, 0, relativePosition.z).normalized;
                actorAIHandler.RequestMove = wayDirection;

                var actorForward = actorAIHandler.ActorData.Rotation * Vector3.forward;
                actorAIHandler.RequestRotate = new Vector3(0, Vector3.Cross(actorForward, wayDirection).y > 0 ? 1.0f : -1.0f, 0);
                
                // すごい適当
                var distance = relativePosition.magnitude;

                if (actorAIHandler.ActorPathFinder.IsNextEndPosition)
                {
                    actorAIHandler.Throttle = Mathf.Clamp(
                        distance / actorAIHandler.ActorData.ActorSpecData.MovingSpeed, 
                        1.0f / actorAIHandler.ActorData.ActorSpecData.MovingSpeed,
                        1.0f);
                }
                else
                {
                    actorAIHandler.Throttle = Mathf.Clamp(
                        distance / actorAIHandler.ActorData.ActorSpecData.MovingSpeed, 
                        0.5f,
                        1.0f);
                }
            }

            var nextOrder = actorAIHandler.ActorData.GetNextInteractOrder();
            var nextOrderObject = actorAIHandler.InteractionObjects.FirstOrDefault(x => x.InteractData == nextOrder);

            if (nextOrder == null || nextOrderObject == null)
            {
                return ActorAIState.Check;
            }

            if (nextOrderObject.InteractData.IsInteractionRange(actorAIHandler.ActorData.Position))
            {
                return ActorAIState.Interact;
            }
            else
            {
                return ActorAIState.Moving;
            }
        }
    }
}