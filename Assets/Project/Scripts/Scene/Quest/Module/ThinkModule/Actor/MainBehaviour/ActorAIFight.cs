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
            if (actorData.ActorStateData.MainTarget == null)
            {
                return ActorAIState.Check;
            }

            MessageBus.Instance.Actor.ForwardBoosterPowerRatio.Broadcast(actorData.InstanceId, 0.5f);
            MessageBus.Instance.Actor.YawBoosterPowerRatio.Broadcast(actorData.InstanceId, 0.1f);

            // 武器
            var isExecute = actorData.WeaponData.Values.Any(v => v.WeaponStateData.IsExecutable);
            MessageBus.Instance.Actor.SetWeaponExecute.Broadcast(actorData.InstanceId, isExecute);

            var isReloadable = actorData.WeaponData.Values.All(v => !v.WeaponStateData.IsExecutable && v.WeaponStateData.IsReloadable);
            if (isReloadable)
            {
                MessageBus.Instance.Actor.ReloadWeapon.Broadcast(actorData.InstanceId);
            }

            return ActorAIState.Fight;
        }
    }
}
