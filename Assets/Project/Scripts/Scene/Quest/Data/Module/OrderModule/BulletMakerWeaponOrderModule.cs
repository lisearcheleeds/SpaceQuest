using System;
using UnityEngine;

namespace AloneSpace
{
    public class BulletMakerWeaponOrderModule : IOrderModule
    {
        BulletMakerWeaponData weaponData;
        
        public BulletMakerWeaponOrderModule(BulletMakerWeaponData weaponData)
        {
            this.weaponData = weaponData;
        }

        public void ActivateModule()
        {
            MessageBus.Instance.RegisterOrderModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.UnRegisterOrderModule.Broadcast(this);
        }

        public void OnUpdateModule(float deltaTime)
        {
            CheckReload(deltaTime);
            CheckFireRate(deltaTime);
            
            AdjustRotate(deltaTime);
            
            Execute();
            
            UpdateState();
        }

        void CheckReload(float deltaTime)
        {
            if (0 < weaponData.WeaponStateData.ReloadTime)
            {
                // リロード中
                weaponData.WeaponStateData.ReloadTime = Math.Max(0, weaponData.WeaponStateData.ReloadTime - deltaTime);
            }
        }

        void CheckFireRate(float deltaTime)
        {
            if (0 < weaponData.WeaponStateData.FireTime)
            {
                if (weaponData.WeaponStateData.IsExecute)
                {
                    // 実行中でなければ0以下にしない
                    weaponData.WeaponStateData.FireTime = Math.Max(0, weaponData.WeaponStateData.FireTime - deltaTime);
                }
                else
                {
                    weaponData.WeaponStateData.FireTime -= deltaTime;
                }
            }
        }

        void AdjustRotate(float deltaTime)
        {
            var targetDirection = weaponData.WeaponStateData.LookAtDirection;
            if (weaponData.WeaponStateData.TargetData != null)
            {
                targetDirection = (weaponData.BasePositionData.Position - weaponData.WeaponStateData.TargetData.Position).normalized;
            }

            weaponData.WeaponStateData.OffsetRotation = Quaternion.RotateTowards(
                weaponData.BasePositionData.Rotation * weaponData.WeaponStateData.OffsetRotation,
                Quaternion.LookRotation(targetDirection),
                deltaTime);
        }

        void Execute()
        {
            if (weaponData.WeaponStateData.IsExecutable)
            {
                return;
            }

            if (0 < weaponData.WeaponStateData.FireTime)
            {
                return;
            }

            if (weaponData.WeaponStateData.IsExecute)
            {
                var rotation = weaponData.BasePositionData.Rotation * weaponData.WeaponStateData.OffsetRotation;
                
                MessageBus.Instance.ExecuteWeapon.Broadcast(
                    weaponData, 
                    weaponData.BasePositionData, 
                    rotation,
                    weaponData.WeaponStateData.TargetData);
                
                weaponData.WeaponStateData.ResourceIndex++;
                weaponData.WeaponStateData.FireTime += weaponData.ParameterVO.FireRate;
            }
        }

        void UpdateState()
        {
            // リロード可能か
            weaponData.WeaponStateData.IsReloadable = 
                weaponData.WeaponStateData.ReloadTime == 0
                && weaponData.WeaponStateData.ResourceIndex != 0;
            
            // 実行可能か
            weaponData.WeaponStateData.IsExecutable = 
                weaponData.WeaponStateData.ReloadTime == 0
                && weaponData.WeaponStateData.ResourceIndex < weaponData.ActorPartsWeaponParameterVO.WeaponResourceMaxCount;

            if (weaponData.WeaponStateData.IsExecutable)
            {
                weaponData.SetExecute(false);
            }
        }
    }
}