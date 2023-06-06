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
            // なし
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
                    weaponData.VO.MissileWeaponEffectSpecVO,
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
            if (weaponData.WeaponGameObjectHandler == null)
            {
                return weaponData.WeaponHolder;
            }

            var outputIndex = weaponData.WeaponStateData.ResourceIndex % weaponData.WeaponGameObjectHandler.OutputPositionData.Length;
            return weaponData.WeaponGameObjectHandler.OutputPositionData[outputIndex];
        }
    }
}