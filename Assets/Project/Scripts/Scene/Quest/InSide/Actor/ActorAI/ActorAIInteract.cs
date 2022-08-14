using System.Linq;
using UnityEngine;

namespace RoboQuest.Quest.InSide
{
    public class ActorAIInteract : IActorAIState
    {
        public ActorAIState ActorAIState => ActorAIState.Interact;
        
        public ActorAIState Update(ActorAIHandler actorAIHandler)
        {
            var nextOrder = actorAIHandler.ActorData.GetNextInteractOrder();
            var nextOrderObject = actorAIHandler.InteractionObjects.FirstOrDefault(x => x.InteractData == nextOrder);

            if (nextOrder == null || nextOrderObject == null)
            {
                return ActorAIState.Check;
            }

            if (!nextOrderObject.InteractData.IsInteractionRange(actorAIHandler.ActorData.Position))
            {
                actorAIHandler.ActorData.SetInteract(null);

                // ターゲットが存在していてもInteract範囲内になければIdleに戻す
                return ActorAIState.Check;
            }
            
            if (actorAIHandler.ActorData.IsInteracting)
            {
                // ターゲットが存在していてInteract範囲内にあってもInteract中なら何もしない
                return ActorAIState.Interact;
            }
            
            actorAIHandler.ActorData.SetInteract(nextOrder);
            return ActorAIState.Interact;
        }
    }
}