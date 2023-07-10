using System;
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
            var targetDirection = weaponData.WeaponStateData.LookAtDirection;
            if (weaponData.WeaponStateData.TargetData != null)
            {
                // とりあえずまずはターゲットの位置の情報を設定
                var outputPosition = GetOutputPosition();
                var targetPosition = weaponData.WeaponStateData.TargetData.Position;
                var targetRelativePosition = targetPosition - outputPosition.Position;
                targetDirection = targetRelativePosition.normalized;

                if (weaponData.VO.IsPredictiveShoot && weaponData.WeaponStateData.TargetData is IMovingModuleHolder targetMovingModuleHolder)
                {
                    // ターゲットが移動する場合は移動先の位置の情報を設定
                    var catchUpToDirection = RotateHelper.GetCatchUpToDirection(
                        targetMovingModuleHolder.MovingModule.MovementVelocity,
                        targetPosition,
                        targetDirection * weaponData.VO.BulletWeaponEffectSpecVO.Speed * deltaTime,
                        outputPosition.Position);

                    if (catchUpToDirection.HasValue)
                    {
                        targetDirection = catchUpToDirection.Value;
                    }
                }
            }

            // TODO: なんかもっと軽い方法ないか考える
            // 角度制限
            targetDirection = Quaternion.RotateTowards(
                Quaternion.LookRotation(weaponData.WeaponHolder.Rotation * Vector3.forward),
                Quaternion.LookRotation(targetDirection),
                weaponData.VO.AngleOfFire) * Vector3.forward;

            // 砲塔回転
            weaponData.WeaponStateData.OffsetRotation = Quaternion.RotateTowards(
                weaponData.WeaponStateData.OffsetRotation,
                Quaternion.Inverse(weaponData.WeaponHolder.Rotation) * Quaternion.LookRotation(targetDirection),
                deltaTime * weaponData.VO.TurningSpeed);
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
                && weaponData.WeaponStateData.ResourceIndex < weaponData.VO.MagazineSize
                && weaponData.BulletMakerWeaponStateData.BurstResourceIndex < weaponData.VO.BurstSize;
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

                    MessageBus.Instance.CreateWeaponEffectData.Broadcast(
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
