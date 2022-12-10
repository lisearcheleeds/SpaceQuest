using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ActorAIMoving : IActorAIState
    {
        public ActorAIState ActorAIState => ActorAIState.Moving;
        
        public ActorAIState Update(QuestData questData, ActorData actorData, float deltaTime)
        {
            if (actorData.MoveTarget == null)
            {   
                return ActorAIState.Check;
            }

            // FIXME: AreaIndex加味する
            // 今はまだゆっくり向いて固定値進むだけ
            var direction = questData.StarSystemData.GetOffsetPosition(actorData.MoveTarget, actorData);
            actorData.Rotation = Quaternion.Lerp(actorData.Rotation, Quaternion.LookRotation(direction), 0.1f);
            actorData.Position = actorData.Position + actorData.Rotation * Vector3.forward;

            if (actorData.ActorAICache.InteractOrder.IsInteractionRange(actorData))
            {
                return ActorAIState.Interact;
            }
            
            return ActorAIState.Moving;
        }
    }
}