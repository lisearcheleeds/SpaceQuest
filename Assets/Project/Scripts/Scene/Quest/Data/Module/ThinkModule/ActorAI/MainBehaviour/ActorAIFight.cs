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

            MessageBus.Instance.ActorCommandForwardBoosterPowerRatio.Broadcast(actorData.InstanceId, 0.5f);
            MessageBus.Instance.ActorCommandYawBoosterPowerRatio.Broadcast(actorData.InstanceId, 0.1f);

            // 武器
            foreach (var weaponData in actorData.WeaponData.Values)
            {
                if (weaponData.WeaponStateData.IsExecutable)
                {
                    MessageBus.Instance.ActorCommandSetWeaponExecute.Broadcast(actorData.InstanceId, true);
                }
                else
                {
                    if (weaponData.WeaponStateData.IsReloadable)
                    {
                        MessageBus.Instance.ActorCommandReloadWeapon.Broadcast(actorData.InstanceId);
                    }
                }
            }
            
            return ActorAIState.Fight;
        }
    }
}