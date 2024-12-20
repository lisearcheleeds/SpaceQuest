﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

            AdjustRotate(deltaTime);
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
                weaponData.BulletMakerWeaponStateData.BurstResourceIndex = 0;
            }
        }

        void AdjustRotate(float deltaTime)
        {
            // FIXME: すべての銃口に対応していない（撃つたびに銃口がかわり、基準も変わる
            var currentOutputPosition = GetOutputPosition();
            var outputDirection = currentOutputPosition.Rotation * Vector3.forward;
            var targetDirection = outputDirection;

            if (weaponData.WeaponStateData.TargetData != null && weaponData.WeaponStateData.IsTargetInAngle)
            {
                var targetPosition = weaponData.WeaponStateData.TargetData.Position;
                var targetRelativePosition = targetPosition - currentOutputPosition.Position;
                var targetRelativeDirection = targetRelativePosition.normalized;

                if (weaponData.VO.IsPredictiveShoot && weaponData.WeaponStateData.TargetData is IMovingModuleHolder targetMovingModuleHolder)
                {
                    // 移動してるなら弾速と合わせて移動先を予測する
                    // ParticleなのでSpeedにdelta timeは不要
                    var catchUpToDirection = RotateHelper.GetCatchUpToDirection(
                        targetMovingModuleHolder.MovingModule.MovementVelocity,
                        targetPosition,
                        targetRelativeDirection * weaponData.VO.BulletWeaponEffectSpecVO.Speed * deltaTime,
                        currentOutputPosition.Position);

                    if (catchUpToDirection.HasValue)
                    {
                        targetRelativeDirection = catchUpToDirection.Value;
                    }
                }
                
                targetDirection = targetRelativeDirection;
            }

            // 砲塔回転
            weaponData.WeaponStateData.OffsetRotation = 
                Quaternion.LookRotation(targetDirection) * Quaternion.Inverse(currentOutputPosition.Rotation);
        }

        void UpdateState()
        {
            // リソースが無いか
            weaponData.WeaponStateData.IsEmptyResource = weaponData.VO.MagazineSize <= weaponData.WeaponStateData.ResourceIndex;
            
            // リロード可能か
            weaponData.WeaponStateData.IsReloadable =
                weaponData.WeaponStateData.ReloadRemainTime == 0
                && weaponData.WeaponStateData.ResourceIndex != 0;

            // 実行可能か
            weaponData.WeaponStateData.IsExecutable =
                weaponData.WeaponStateData.ReloadRemainTime == 0
                && weaponData.WeaponStateData.ResourceIndex < weaponData.VO.MagazineSize
                && weaponData.BulletMakerWeaponStateData.BurstResourceIndex < weaponData.VO.BurstSize;

            var targetData = weaponData.WeaponStateData.TargetData;
            if (targetData != null)
            {
                var currentOutputPosition = GetOutputPosition();
                
                // 有効射程か
                var targetSqrDistance = Vector3.SqrMagnitude(targetData.Position - currentOutputPosition.Position);
                var effectiveSqrDistance = weaponData.VO.BulletWeaponEffectSpecVO.EffectiveDistance *
                                           weaponData.VO.BulletWeaponEffectSpecVO.EffectiveDistance;
                weaponData.WeaponStateData.IsTargetInRange = targetSqrDistance < effectiveSqrDistance;
             
                // 有効射角か   
                var outputDirection = currentOutputPosition.Rotation * Vector3.forward;
                var targetPosition = weaponData.WeaponStateData.TargetData.Position;
                var targetRelativePosition = targetPosition - currentOutputPosition.Position;
                var targetRelativeDirection = targetRelativePosition.normalized;
                weaponData.WeaponStateData.IsTargetInAngle =
                    Vector3.Angle(outputDirection, targetRelativeDirection) < weaponData.VO.AngleOfFire;
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
                for (var i = 0; i < weaponData.VO.ShotCount; i++)
                {
                    var outputPosition = GetOutputPosition();
                    var rotation = outputPosition.Rotation * weaponData.WeaponStateData.OffsetRotation;

                    var accuracyRandomVector = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * (1.0f / weaponData.VO.Accuracy);
                    rotation = rotation * Quaternion.LookRotation(Vector3.forward + accuracyRandomVector);

                    MessageBus.Instance.Data.CreateWeaponEffectData.Broadcast(
                        weaponData.VO.BulletWeaponEffectSpecVO,
                        new BulletWeaponEffectCreateOptionData(
                            weaponData,
                            outputPosition,
                            rotation,
                            weaponData.WeaponStateData.TargetData));

                    weaponData.WeaponStateData.ResourceIndex++;
                    weaponData.BulletMakerWeaponStateData.BurstResourceIndex++;
                }

                weaponData.WeaponStateData.FireTime += weaponData.VO.FireRate;
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
