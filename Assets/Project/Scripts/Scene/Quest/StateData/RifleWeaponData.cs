using System;
using UnityEngine;

namespace RoboQuest.Quest
{
    public class RifleWeaponData : WeaponData
    {
        ActorPartsWeaponRifleParameterVO actorPartsWeaponRifleParameterVO;

        ITargetData targetData;

        float fireTime;
        float reloadTime;
        int resourceIndex;
        ItemVO[] currentResources;
        
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

            if (currentResources == null)
            {
                return 0.0f;
            }

            var resourcesAvailability = ((float)currentResources.Length - resourceIndex) / currentResources.Length;
            return Math.Max(0.0f, resourcesAvailability);
        }

        public override bool IsReloadable()
        {
            return reloadTime == 0 && (resourceIndex == 0 || (currentResources != null && resourceIndex != 0));
        }

        public override void Reload(ItemVO[] resources)
        {
            resourceIndex = 0;
            currentResources = resources;
            reloadTime += actorPartsWeaponRifleParameterVO.ReloadTime;
        }

        public override bool IsExecutable(ITargetData targetData)
        {
            return reloadTime == 0 && fireTime == 0 && resourceIndex < (currentResources?.Length ?? -1) && targetData != null;
        }

        public override void Execute(ITargetData targetData)
        {
            this.targetData = targetData;
        }

        public override void Update(float deltaTime)
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
                MessageBus.Instance.ExecuteTriggerWeapon.Broadcast(this, currentResources[resourceIndex], targetData, 1.0f);
                
                resourceIndex++;
                fireTime += actorPartsWeaponRifleParameterVO.FireRate;
            }

            targetData = null;
        }
    }
}