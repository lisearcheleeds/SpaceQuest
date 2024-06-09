using System;
using UnityEngine;

namespace AloneSpace
{
    public class MissileMakerWeaponOrderModule : IOrderModule
    {
        MissileMakerWeaponData weaponData;

        public MissileMakerWeaponOrderModule(MissileMakerWeaponData weaponData)
        {
            this.weaponData = weaponData;
        }

        public void ActivateModule()
        {
            MessageBus.Instance.Module.RegisterOrderModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.Module.UnRegisterOrderModule.Broadcast(this);
        }

        public void OnUpdateModule(float deltaTime)
        {
            CheckReload(deltaTime);
            CheckFireRate(deltaTime);
            CheckBurst();

            UpdateState();

            Execute();

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

        void CheckBurst()
        {
            if (!weaponData.WeaponStateData.IsExecute)
            {
                weaponData.MissileMakerWeaponStateData.BurstResourceIndex = 0;
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
                weaponData.WeaponStateData.TargetData != null
                && weaponData.WeaponStateData.ReloadRemainTime == 0
                && weaponData.WeaponStateData.ResourceIndex < weaponData.VO.MagazineSize
                && weaponData.MissileMakerWeaponStateData.BurstResourceIndex < weaponData.VO.BurstSize;
            
            // 有効射程か
            if (weaponData.WeaponStateData.TargetData != null)
            {
                var currentOutputPosition = GetOutputPosition();

                var targetSqrDistance = Vector3.SqrMagnitude(weaponData.WeaponStateData.TargetData.Position - currentOutputPosition.Position);
                var effectiveSqrDistance = weaponData.VO.MissileWeaponEffectSpecVO.EffectiveDistance *
                                           weaponData.VO.MissileWeaponEffectSpecVO.EffectiveDistance;
                weaponData.WeaponStateData.IsTargetInRange = targetSqrDistance < effectiveSqrDistance;
                
                // 有効射角か   
                var outputDirection = currentOutputPosition.Rotation * Vector3.forward;
                var targetPosition = weaponData.WeaponStateData.TargetData.Position;
                var targetRelativePosition = targetPosition - currentOutputPosition.Position;
                var targetRelativeDirection = targetRelativePosition.normalized;
                weaponData.WeaponStateData.IsTargetInAngle =
                    Vector3.Angle(outputDirection, targetRelativeDirection) < weaponData.VO.LockOnAngle;
            }
            else
            {
                weaponData.WeaponStateData.IsTargetInRange = false;
                weaponData.WeaponStateData.IsTargetInAngle = false;
            }
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
                weaponData.WeaponStateData.FireTime += weaponData.VO.FireRate;

                for (var i = 0; i < weaponData.VO.ShotCount; i++)
                {
                    var outputPosition = GetOutputPosition();
                    var launchDirection = weaponData.VO.HorizontalLaunch
                        ? outputPosition.Rotation * Vector3.up
                        : outputPosition.Rotation * Vector3.forward;

                    MessageBus.Instance.Data.CreateWeaponEffectData.Broadcast(
                        weaponData.VO.MissileWeaponEffectSpecVO,
                        new MissileWeaponEffectCreateOptionData(
                            weaponData,
                            outputPosition,
                            weaponData.WeaponStateData.TargetData,
                            launchDirection * weaponData.VO.LaunchSpeed));

                    weaponData.WeaponStateData.ResourceIndex++;
                    weaponData.MissileMakerWeaponStateData.BurstResourceIndex++;
                }
            }
        }

        IPositionData GetOutputPosition()
        {
            if (weaponData.WeaponGameObjectHandler == null)
            {
                return weaponData.WeaponHolder;
            }

            var outputIndex = weaponData.WeaponStateData.ResourceIndex % weaponData.WeaponGameObjectHandler.OutputPositionData.Length;
            return weaponData.WeaponGameObjectHandler.OutputPositionData[outputIndex];
        }
    }
}
