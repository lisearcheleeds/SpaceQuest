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
            UpdateState();
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
                        targetRelativeDirection * weaponData.VO.ParticleBulletWeaponEffectSpecVO.Speed,
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
            // リロード可能か
            weaponData.WeaponStateData.IsReloadable =
                weaponData.WeaponStateData.ReloadRemainTime == 0
                && weaponData.WeaponStateData.ResourceIndex != 0;

            // 実行可能か
            weaponData.WeaponStateData.IsExecutable = true;

            var targetData = weaponData.WeaponStateData.TargetData;
            var currentOutputPosition = GetOutputPosition();
            if (targetData != null)
            {
                // 有効射程か
                var targetSqrDistance = Vector3.SqrMagnitude(targetData.Position - currentOutputPosition.Position);
                var effectiveSqrDistance = weaponData.VO.ParticleBulletWeaponEffectSpecVO.EffectiveDistance *
                                           weaponData.VO.ParticleBulletWeaponEffectSpecVO.EffectiveDistance;
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

        IPositionData GetOutputPosition()
        {
            // GameObjectが無いデータ上の存在であれば簡略化
            if (weaponData.WeaponGameObjectHandler == null)
            {
                return weaponData.WeaponHolder;
            }

            var outputIndex = weaponData.WeaponStateData.ResourceIndex % weaponData.WeaponGameObjectHandler.OutputPositionData.Length;
            return weaponData.WeaponGameObjectHandler.OutputPositionData[outputIndex];
        }
    }
}
