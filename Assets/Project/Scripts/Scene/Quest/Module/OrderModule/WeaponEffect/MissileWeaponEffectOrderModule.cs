﻿using System;
using UnityEngine;

namespace AloneSpace
{
    public class MissileWeaponEffectOrderModule : IOrderModule
    {
        MissileWeaponEffectData effectData;

        public MissileWeaponEffectOrderModule(MissileWeaponEffectData missileWeaponEffectData)
        {
            this.effectData = missileWeaponEffectData;
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
            effectData.CurrentLifeTime += deltaTime;

            if (effectData.CurrentLifeTime > effectData.SpecVO.LifeTime)
            {
                MessageBus.Instance.Data.ReleaseWeaponEffectData.Broadcast(effectData);
                return;
            }

            if (effectData.CollisionEventEffectReceiverModuleList.Count != 0)
            {
                // TODO: ダメージ与えたときだけAddCollideCountしたい
                effectData.AddCollideCount();

                MessageBus.Instance.Data.CreateWeaponEffectData.Broadcast(
                    ((MissileMakerWeaponData)effectData.WeaponData).VO.ExplosionWeaponEffectSpecVO,
                    new ExplosionWeaponEffectCreateOptionData(effectData.WeaponData, effectData));

                MessageBus.Instance.Data.ReleaseWeaponEffectData.Broadcast(effectData);
                return;
            }

            if (effectData.TargetData is IReleasableData releasableData)
            {
                if (releasableData.IsReleased)
                {
                    return;
                }
            }

            if (effectData.CurrentLifeTime < effectData.SpecVO.LaunchWaitTime)
            {
                // 待機時間中はLaunchMovementVelocity以外の移動や回転は行わない
                effectData.MovingModule.SetMovementVelocity(effectData.OptionData.LaunchMovementVelocity);
                return;
            }

            // 誘導処理
            var targetDirection = (effectData.TargetData.Position - effectData.Position).normalized;
            var currentDirection = effectData.Rotation * Vector3.forward;
            if (effectData.TargetData is IMovingModuleHolder targetMovingModuleHolder)
            {
                // ターゲットが移動する場合は移動先に回転
                var catchUpToDirection = RotateHelper.GetCatchUpToDirection(
                    targetMovingModuleHolder.MovingModule.MovementVelocity,
                    effectData.TargetData.Position,
                    effectData.Rotation * Vector3.forward * effectData.SpecVO.Speed,
                    effectData.Position);

                if (catchUpToDirection.HasValue)
                {
                    effectData.MovingModule.SetMovementVelocity(currentDirection * effectData.SpecVO.Speed);
                    effectData.MovingModule.SetQuaternionVelocityLHS(
                        Quaternion.AngleAxis(deltaTime * effectData.SpecVO.HomingAngle, Vector3.Cross(currentDirection, catchUpToDirection.Value)));
                }
                else
                {
                    // 何もしない
                }
            }
            else
            {
                // ターゲットが移動しない場合はターゲットの位置に回転
                effectData.MovingModule.SetMovementVelocity(currentDirection * effectData.SpecVO.Speed);
                effectData.MovingModule.SetQuaternionVelocityLHS(Quaternion.AngleAxis(deltaTime * effectData.SpecVO.HomingAngle, Vector3.Cross(currentDirection, targetDirection)));
            }
        }
    }
}
