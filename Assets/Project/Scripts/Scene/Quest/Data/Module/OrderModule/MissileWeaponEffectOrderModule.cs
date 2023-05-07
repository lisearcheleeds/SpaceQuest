using System;
using UnityEngine;

namespace AloneSpace
{
    public class MissileWeaponEffectOrderModule : IOrderModule
    {
        MissileWeaponEffectData effectData;
        bool isFirstUpdate;

        public MissileWeaponEffectOrderModule(MissileWeaponEffectData missileWeaponEffectData)
        {
            this.effectData = missileWeaponEffectData;
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
            if (isFirstUpdate)
            {
                isFirstUpdate = false;

                effectData.MovingModule.SetMovementVelocity(effectData.Rotation * Vector3.forward * effectData.ParameterVO.LaunchSpeed * deltaTime);
            }

            effectData.CurrentLifeTime += deltaTime;
            if (effectData.CurrentLifeTime > effectData.LifeTime || effectData.CollisionData.IsCollided)
            {
                effectData.IsAlive = false;
                effectData.DeactivateModules();
                MessageBus.Instance.ReleaseWeaponEffectData.Broadcast(effectData);
                return;
            }

            var targetDirection = (effectData.TargetData.Position - effectData.Position).normalized;
            var currentDirection = effectData.Rotation * Vector3.forward;
            if (effectData.TargetData is IMovingModuleHolder targetMovingModuleHolder)
            {
                // ターゲットが移動する場合は移動先に回転
                var catchUpToDirection = RotateHelper.GetCatchUpToDirection(
                    targetMovingModuleHolder.MovingModule.MovementVelocity,
                    effectData.TargetData.Position,
                    effectData.Rotation * Vector3.forward * effectData.ParameterVO.Speed * deltaTime,
                    effectData.Position);

                if (catchUpToDirection.HasValue)
                {
                    effectData.MovingModule.SetMovementVelocity(currentDirection * effectData.ParameterVO.Speed * deltaTime);
                    effectData.MovingModule.SetQuaternionVelocityLHS(Quaternion.AngleAxis(150.0f * deltaTime, Vector3.Cross(currentDirection, targetDirection)));
                }
                else
                {
                    // 何もしない
                }
            }
            else
            {
                effectData.MovingModule.SetMovementVelocity(currentDirection * effectData.ParameterVO.Speed * deltaTime);
                effectData.MovingModule.SetQuaternionVelocityLHS(Quaternion.AngleAxis(150.0f * deltaTime, Vector3.Cross(currentDirection, targetDirection)));
            }
        }
    }
}