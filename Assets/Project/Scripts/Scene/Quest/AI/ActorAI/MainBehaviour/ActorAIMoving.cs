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
            if (actorData.ActorAIStateData.MoveTarget == null)
            {   
                return ActorAIState.Check;
            }

            // FIXME: AreaId加味する
            // 今はまだゆっくり向いて固定値進むだけ
            var direction = questData.StarSystemData.GetOffsetPosition(actorData.ActorAIStateData.MoveTarget, actorData);
            actorData.Rotation = Quaternion.Lerp(actorData.Rotation, Quaternion.LookRotation(direction), 0.1f);
            actorData.Position = actorData.Position + actorData.Rotation * Vector3.forward;
            
            return ActorAIState.Moving;
        }
    }
}