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

        void AdjustRotate(float deltaTime)
        {
            var targetDirection = weaponData.WeaponStateData.LookAtDirection;
            if (weaponData.WeaponStateData.TargetData != null)
            {
                var outputPosition = GetOutputPosition();
                var targetPosition = weaponData.WeaponStateData.TargetData.Position;
                var targetRelativePosition = targetPosition - outputPosition.Position;
                targetDirection = targetRelativePosition.normalized;

                // ターゲットが移動する場合は移動先に回転
                if (weaponData.WeaponStateData.TargetData is IMovingModuleHolder targetMovingModuleHolder)
                {
                    var catchUpToDirection = RotateHelper.GetCatchUpToDirection(
                        targetMovingModuleHolder.MovingModule.MovementVelocity,
                        targetPosition,
                        targetDirection * weaponData.VO.Speed * deltaTime,
                        outputPosition.Position);

                    if (catchUpToDirection.HasValue)
                    {
                        targetDirection = catchUpToDirection.Value;
                    }
                }
            }

            // TODO: なんかもっと軽い方法ないか考える
            weaponData.WeaponStateData.OffsetRotation = Quaternion.RotateTowards(
                weaponData.WeaponStateData.OffsetRotation,
                Quaternion.Inverse(weaponData.WeaponHolder.Rotation) * Quaternion.LookRotation(targetDirection),
                deltaTime * 150.0f);
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
                && weaponData.WeaponStateData.ResourceIndex < weaponData.WeaponSpecVO.WeaponResourceMaxCount;
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
                var outputPosition = GetOutputPosition();
                var rotation = outputPosition.Rotation * weaponData.WeaponStateData.OffsetRotation;

                MessageBus.Instance.CreateWeaponEffectData.Broadcast(
                    weaponData,
                    outputPosition,
                    rotation,
                    weaponData.WeaponStateData.TargetData);

                weaponData.WeaponStateData.ResourceIndex++;
                weaponData.WeaponStateData.FireTime += weaponData.VO.FireRate;
            }
        }

        IPositionData GetOutputPosition()
        {
            if (weaponData.WeaponFeedback == null)
            {
                return weaponData.WeaponHolder;
            }

            var outputIndex = weaponData.WeaponStateData.ResourceIndex % weaponData.WeaponFeedback.OutputPositionData.Length;
            return weaponData.WeaponFeedback.OutputPositionData[outputIndex];
        }
    }
}