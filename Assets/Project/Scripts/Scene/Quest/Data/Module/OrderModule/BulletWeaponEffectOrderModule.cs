using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AloneSpace
{
    public class BulletWeaponEffectOrderModule : IOrderModule
    {
        BulletWeaponEffectData effectData;
        bool isFirstUpdate;

        public BulletWeaponEffectOrderModule(BulletWeaponEffectData bulletWeaponEffectData)
        {
            this.effectData = bulletWeaponEffectData;
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

                var accuracyRandomVector = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * (1.0f / effectData.VO.Accuracy);
                effectData.MovingModule.SetMovementVelocity(effectData.Rotation * (Vector3.forward + accuracyRandomVector).normalized * effectData.VO.Speed * deltaTime);
            }

            effectData.CurrentLifeTime += deltaTime;
            if (effectData.CurrentLifeTime > effectData.LifeTime)
            {
                effectData.IsAlive = false;
                effectData.DeactivateModules();
                MessageBus.Instance.ReleaseWeaponEffectData.Broadcast(effectData);
                return;
            }

            if (effectData.CollisionEventEffectReceiverModuleList.Count != 0)
            {
                effectData.IsAlive = false;
                effectData.DeactivateModules();
                MessageBus.Instance.ReleaseWeaponEffectData.Broadcast(effectData);
                return;
            }
        }
    }
}