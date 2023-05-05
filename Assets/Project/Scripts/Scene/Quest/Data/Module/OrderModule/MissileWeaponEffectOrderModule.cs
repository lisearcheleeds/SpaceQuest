using System;
using UnityEngine;

namespace AloneSpace
{
    public class MissileWeaponEffectOrderModule : IOrderModule
    {
        MissileWeaponEffectData missileWeaponEffectData;
        
        public MissileWeaponEffectOrderModule(MissileWeaponEffectData missileWeaponEffectData)
        {
            this.missileWeaponEffectData = missileWeaponEffectData;
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
            missileWeaponEffectData.CurrentLifeTime += deltaTime;
            if (missileWeaponEffectData.CurrentLifeTime > missileWeaponEffectData.LifeTime || missileWeaponEffectData.CollisionData.IsCollided)
            {
                missileWeaponEffectData.IsAlive = false;
                missileWeaponEffectData.DeactivateModules();
                MessageBus.Instance.ReleaseWeaponEffectData.Broadcast(missileWeaponEffectData);
                return;
            }

            var targetDiffPosition = missileWeaponEffectData.TargetData.Position - missileWeaponEffectData.Position;
            missileWeaponEffectData.Direction = Vector3.RotateTowards(missileWeaponEffectData.Direction, targetDiffPosition, GetRotateRatio(missileWeaponEffectData.CurrentLifeTime), 0);
            missileWeaponEffectData.MovingModule.SetInertiaTensor(missileWeaponEffectData.Direction * 25.0f);
        }

        float GetRotateRatio(float time)
        {
            return time switch
            {
                _ when time < 1.0f => 0.0f,
                _ when time < 2.0f => 0.0f,
                _ => 0.002f,
            };
        }
    }
}