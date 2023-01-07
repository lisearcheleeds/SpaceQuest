using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ActorAIFight : IActorAIState
    {
        public ActorAIState ActorAIState => ActorAIState.Fight;
        
        public ActorAIState Update(QuestData questData, ActorData actorData, float deltaTime)
        {
            // ターゲット確認
            if (!actorData.ActorAIStateData.MainTarget?.IsAlive ?? true)
            {
                var currentTarget = actorData.ActorAIStateData.AroundTargets.FirstOrDefault(target => target.IsAlive && actorData.PlayerInstanceId != (target as ActorData)?.PlayerInstanceId);
                if (currentTarget != null)
                {
                    // ターゲット更新
                    actorData.ActorAIStateData.MainTarget = currentTarget;
                }
                else
                {
                    // 戦闘終了
                    actorData.ActorAIStateData.MainTarget = null;
                    return ActorAIState.Check;
                }
            }

            // 戦闘中の移動先
            // 今はまだゆっくり向いて固定値進むだけ
            var direction = questData.StarSystemData.GetOffsetPosition(actorData.ActorAIStateData.MainTarget, actorData).normalized;
            actorData.Rotation = Quaternion.Lerp(actorData.Rotation, Quaternion.LookRotation(direction), 0.1f);
            actorData.Position = actorData.Position + actorData.Rotation * Vector3.forward;

            // 武器
            foreach (var weaponData in actorData.WeaponData)
            {
                var availability = weaponData.GetAvailability();
                if (availability == 0.0f)
                {
                    if (weaponData.IsReloadable())
                    {
                        weaponData.Reload();
                    }

                    continue;
                }

                if (weaponData.IsExecutable(actorData.ActorAIStateData.MainTarget))
                {
                    weaponData.Execute(actorData.ActorAIStateData.MainTarget);
                }
            }
            
            return ActorAIState.Fight;
        }
    }
}