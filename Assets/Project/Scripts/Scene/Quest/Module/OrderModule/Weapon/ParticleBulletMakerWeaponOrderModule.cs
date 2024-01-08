using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AloneSpace
{
    public class ParticleBulletMakerWeaponOrderModule : IOrderModule
    {
        ParticleBulletMakerWeaponData weaponData;

        public ParticleBulletMakerWeaponOrderModule(ParticleBulletMakerWeaponData weaponData)
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
            CreateWeaponEffectData();

            AdjustRotate(deltaTime);
        }

        void CreateWeaponEffectData()
        {
            if (weaponData.WeaponStateData.WeaponEffectDataList.Count != 0)
            {
                return;
            }

            // 存在しなければ1つ作る
            // WeaponEffectDataListは毎フレーム更新されるはずなので、0だったらとりあえずCreateWeaponEffectDataして良いはず
            MessageBus.Instance.Data.CreateWeaponEffectData.Broadcast(
                weaponData.VO.ParticleBulletWeaponEffectSpecVO,
                new ParticleBulletWeaponEffectCreateOptionData(
                    weaponData,
                    GetOutputPosition,
                    () => weaponData.WeaponStateData.OffsetRotation,
                    weaponData.WeaponStateData.TargetData));
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
                        targetDirection * weaponData.VO.ParticleBulletWeaponEffectSpecVO.Speed * deltaTime,
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
                Quaternion.LookRotation(targetDirection) * Quaternion.Inverse(GetOutputPosition().Rotation),
                deltaTime * weaponData.VO.TurningSpeed);
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
