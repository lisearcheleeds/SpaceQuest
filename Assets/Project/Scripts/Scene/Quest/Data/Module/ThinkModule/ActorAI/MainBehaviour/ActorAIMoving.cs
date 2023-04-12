using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ActorAIMoving : IActorAIState
    {
        public ActorAIState Update(ActorData actorData, float deltaTime)
        {
            if (actorData.ActorStateData.MoveTarget == null)
            {   
                return ActorAIState.Check;
            }

            // FIXME: AreaId加味する
            // 今はまだゆっくり向いて固定値進むだけ
            /*
            var direction = questData.StarSystemData.GetVector3Position(actorData, actorData.MoveTarget);
            actorData.Rotation = Quaternion.Lerp(actorData.Rotation, Quaternion.LookRotation(direction), 0.1f);
            actorData.Position = actorData.Position + actorData.Rotation * Vector3.forward;
            */
            
            return ActorAIState.Moving;
        }
    }
}