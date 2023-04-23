using System;
using UnityEngine;

namespace AloneSpace
{
    public class BulletWeaponEffectOrderModule : IOrderModule
    {
        BulletWeaponEffectData bulletWeaponEffectData;
        
        public BulletWeaponEffectOrderModule(BulletWeaponEffectData bulletWeaponEffectData)
        {
            this.bulletWeaponEffectData = bulletWeaponEffectData;
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
            bulletWeaponEffectData.CurrentLifeTime += deltaTime;
            if (bulletWeaponEffectData.CurrentLifeTime > bulletWeaponEffectData.LifeTime || bulletWeaponEffectData.CollisionData.IsCollided)
            {
                bulletWeaponEffectData.IsAlive = false;
                bulletWeaponEffectData.DeactivateModules();
                MessageBus.Instance.ReleaseWeaponEffectData.Broadcast(bulletWeaponEffectData);
            }
        }
    }
}