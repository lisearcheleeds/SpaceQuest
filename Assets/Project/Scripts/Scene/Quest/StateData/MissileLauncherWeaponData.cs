using System;
using AloneSpace;
using UnityEditor;
using UnityEngine;

namespace AloneSpace
{
    public class MissileLauncherWeaponData : WeaponData
    {
        ActorPartsWeaponMissileLauncherParameterVO actorPartsWeaponMissileLauncherParameterVO;

        ITargetData targetData;

        float fireTime;
        float reloadTime;
        int resourceIndex;
        
        public override Guid InstanceId { get; }
        
        public MissileLauncherWeaponData(ActorPartsWeaponMissileLauncherParameterVO actorPartsWeaponMissileLauncherParameterVO)
        {
            InstanceId = Guid.NewGuid();
            this.actorPartsWeaponMissileLauncherParameterVO = actorPartsWeaponMissileLauncherParameterVO;
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
            return reloadTime == 0 && resourceIndex != 0;
        }
        
        public override void Reload()
        {
            resourceIndex = 0;
            reloadTime += actorPartsWeaponMissileLauncherParameterVO.ReloadTime;
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
                return;
            }

            if (IsExecutable(targetData))
            {
                MessageBus.Instance.ExecuteTriggerWeapon.Broadcast(this, targetData, 1.0f);
                
                resourceIndex++;
                fireTime += actorPartsWeaponMissileLauncherParameterVO.LaunchIntervalTime;
            }

            targetData = null;
        }
    }
}