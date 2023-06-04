using System;
using UnityEngine;

namespace AloneSpace
{
    public class MissileWeaponEffectOrderModule : IOrderModule
    {
        MissileWeaponEventEffectData eventEffectData;
        bool isFirstUpdate;

        public MissileWeaponEffectOrderModule(MissileWeaponEventEffectData missileWeaponEventEffectData)
        {
            this.eventEffectData = missileWeaponEventEffectData;
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

                eventEffectData.MovingModule.SetMovementVelocity(eventEffectData.Rotation * Vector3.forward * eventEffectData.VO.LaunchSpeed * deltaTime);
            }

            eventEffectData.CurrentLifeTime += deltaTime;
            if (eventEffectData.CurrentLifeTime > eventEffectData.LifeTime)
            {
                eventEffectData.IsAlive = false;
                eventEffectData.DeactivateModules();
                MessageBus.Instance.ReleaseWeaponEffectData.Broadcast(eventEffectData);
                return;
            }

            var targetDirection = (eventEffectData.TargetData.Position - eventEffectData.Position).normalized;
            var currentDirection = eventEffectData.Rotation * Vector3.forward;
            if (eventEffectData.TargetData is IMovingModuleHolder targetMovingModuleHolder)
            {
                // ターゲットが移動する場合は移動先に回転
                var catchUpToDirection = RotateHelper.GetCatchUpToDirection(
                    targetMovingModuleHolder.MovingModule.MovementVelocity,
                    eventEffectData.TargetData.Position,
                    eventEffectData.Rotation * Vector3.forward * eventEffectData.VO.Speed * deltaTime,
                    eventEffectData.Position);

                if (catchUpToDirection.HasValue)
                {
                    eventEffectData.MovingModule.SetMovementVelocity(currentDirection * eventEffectData.VO.Speed * deltaTime);
                    eventEffectData.MovingModule.SetQuaternionVelocityLHS(Quaternion.AngleAxis(150.0f * deltaTime, Vector3.Cross(currentDirection, targetDirection)));
                }
                else
                {
                    // 何もしない
                }
            }
            else
            {
                eventEffectData.MovingModule.SetMovementVelocity(currentDirection * eventEffectData.VO.Speed * deltaTime);
                eventEffectData.MovingModule.SetQuaternionVelocityLHS(Quaternion.AngleAxis(150.0f * deltaTime, Vector3.Cross(currentDirection, targetDirection)));
            }
        }
    }
}