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
            if (0 < weaponData.WeaponStateData.ReloadRemainTime)
            {
                // リロード中
                weaponData.WeaponStateData.ReloadRemainTime = Math.Max(0, weaponData.WeaponStateData.ReloadRemainTime - deltaTime);

                if (weaponData.WeaponStateData.ReloadRemainTime == 0)
                {
                    weaponData.WeaponStateData.ResourceIndex = 0;
                }
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
                var targetPosition = weaponData.WeaponStateData.TargetData.Position;
                var targetRelativePosition = targetPosition - weaponData.BasePositionData.Position; 
                targetDirection = targetRelativePosition.normalized;
                
                // ターゲットが移動する場合は移動先に回転
                if (weaponData.WeaponStateData.TargetData is IMovingModuleHolder targetMovingModuleHolder)
                {
                    var targetMoveDelta = targetMovingModuleHolder.MovingModule.MoveDelta;
                    var relativeVelocity = targetMoveDelta - (targetDirection * weaponData.ParameterVO.Speed * deltaTime);
                    if (relativeVelocity != Vector3.zero)
                    {
                        var targetDistance = targetRelativePosition.magnitude;
                        var collisionTime = targetDistance / relativeVelocity.magnitude;
                        var targetTimedRelativePosition = (targetPosition + targetMoveDelta * collisionTime) - weaponData.BasePositionData.Position; 
                        targetDirection = targetTimedRelativePosition.normalized;
                    }
                }
            }
            
            // TODO: なんかもっと軽い方法ないか考える
            weaponData.WeaponStateData.OffsetRotation = Quaternion.RotateTowards(
                weaponData.WeaponStateData.OffsetRotation,
                Quaternion.Inverse(weaponData.WeaponHolder.Rotation) * Quaternion.LookRotation(targetDirection),
                deltaTime * 150.0f);
        }

        void Execute()
        {
            if (!weaponData.WeaponStateData.IsExecutable)
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
                
                MessageBus.Instance.CreateWeaponEffectData.Broadcast(
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
                weaponData.WeaponStateData.ReloadRemainTime == 0
                && weaponData.WeaponStateData.ResourceIndex != 0;
            
            // 実行可能か
            weaponData.WeaponStateData.IsExecutable = 
                weaponData.WeaponStateData.ReloadRemainTime == 0
                && weaponData.WeaponStateData.ResourceIndex < weaponData.ActorPartsWeaponParameterVO.WeaponResourceMaxCount;
        }
    }
}