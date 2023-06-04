using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AloneSpace
{
    public class BulletWeaponEffectOrderModule : IOrderModule
    {
        BulletWeaponEventEffectData eventEffectData;
        bool isFirstUpdate;

        public BulletWeaponEffectOrderModule(BulletWeaponEventEffectData bulletWeaponEventEffectData)
        {
            this.eventEffectData = bulletWeaponEventEffectData;
            isFirstUpdate = true;
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

                var accuracyRandomVector = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * (1.0f / eventEffectData.VO.Accuracy);
                eventEffectData.MovingModule.SetMovementVelocity(eventEffectData.Rotation * (Vector3.forward + accuracyRandomVector).normalized * eventEffectData.VO.Speed * deltaTime);
            }

            eventEffectData.CurrentLifeTime += deltaTime;
            if (eventEffectData.CurrentLifeTime > eventEffectData.LifeTime)
            {
                eventEffectData.IsAlive = false;
                eventEffectData.DeactivateModules();
                MessageBus.Instance.ReleaseWeaponEffectData.Broadcast(eventEffectData);
            }
        }
    }
}