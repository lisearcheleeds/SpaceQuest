using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class RifleWeaponData : WeaponData
    {
        ActorPartsWeaponRifleParameterVO actorPartsWeaponRifleParameterVO;

        ITargetData targetData;

        float fireTime;
        float reloadTime;
        int resourceIndex;
        
        public override Guid InstanceId { get; }
        
        public RifleWeaponData(ActorPartsWeaponRifleParameterVO actorPartsWeaponRifleParameterVO)
        {
            InstanceId = Guid.NewGuid();
            this.actorPartsWeaponRifleParameterVO = actorPartsWeaponRifleParameterVO;
        }

        public override float GetAvailability()
        {
            if (reloadTime != 0)
            {
                return 0.0f;
            }

            return 1.0f;
        }

        public override bool IsReloadable()
        {
            return reloadTime == 0 && (resourceIndex == 0 || resourceIndex != 0);
        }

        public override void Reload()
        {
            resourceIndex = 0;
            reloadTime += actorPartsWeaponRifleParameterVO.ReloadTime;
        }

        public override bool IsExecutable(ITargetData targetData)
        {
            return reloadTime == 0 && fireTime == 0 && resourceIndex != 0 && targetData != null;
        }

        public override void Execute(ITargetData targetData)
        {
            this.targetData = targetData;
        }

        public override void OnLateUpdate(float deltaTime)
        {
            if (0 < fireTime)
            {
                // 連射中
                fireTime = Math.Max(0, fireTime - deltaTime);
            }

            if (0 < reloadTime)
            {
                // リロード中
                reloadTime = Math.Max(0, reloadTime - deltaTime);
            }

            if (IsExecutable(targetData))
            {
                MessageBus.Instance.ExecuteTriggerWeapon.Broadcast(this, targetData, 1.0f);
                
                resourceIndex++;
                fireTime += actorPartsWeaponRifleParameterVO.FireRate;
            }

            targetData = null;
        }
    }
}