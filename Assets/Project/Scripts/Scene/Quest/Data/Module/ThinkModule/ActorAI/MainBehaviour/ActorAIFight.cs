using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ActorAIFight : IActorAIState
    {
        public ActorAIState Update(ActorData actorData, float deltaTime)
        {
            // ターゲット確認
            if (actorData.ActorStateData.MainTarget != null)
            {
                var currentTarget = actorData.ActorStateData.AroundTargets.FirstOrDefault(target => actorData.PlayerInstanceId != (target as ActorData)?.PlayerInstanceId);
                if (currentTarget != null)
                {
                    // ターゲット更新
                    actorData.ActorStateData.MainTarget = currentTarget;
                }
                else
                {
                    // 戦闘終了
                    actorData.ActorStateData.MainTarget = null;
                    return ActorAIState.Check;
                }
            }

            // 戦闘中の移動先
            // 今はまだゆっくり向いて固定値進むだけ
            /*
            var direction = questData.StarSystemData.GetVector3Position(actorData, actorData.ActorAIStateData.MainTarget).normalized;
            actorData.Rotation = Quaternion.Lerp(actorData.Rotation, Quaternion.LookRotation(direction), 0.1f);
            actorData.Position = actorData.Position + actorData.Rotation * Vector3.forward;
            */

            // 武器
            foreach (var weaponData in actorData.WeaponData.Values)
            {
                if (weaponData.WeaponStateData.IsExecutable)
                {
                    weaponData.SetExecute(true);
                }
                else
                {
                    if (weaponData.WeaponStateData.IsReloadable)
                    {
                        weaponData.Reload();
                    }
                }
            }
            
            return ActorAIState.Fight;
        }
    }
}