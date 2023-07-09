using UnityEngine;

namespace AloneSpace
{
    public class ParticleBulletWeaponEffectOrderModule : IOrderModule
    {
        ParticleBulletWeaponEffectData effectData;
        bool isFirstUpdate;

        public ParticleBulletWeaponEffectOrderModule(ParticleBulletWeaponEffectData particleBulletWeaponEffectData)
        {
            this.effectData = particleBulletWeaponEffectData;
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
                effectData.MovingModule.SetMovementVelocity(effectData.Rotation * Vector3.forward * effectData.SpecVO.Speed * deltaTime);
            }

            effectData.CurrentLifeTime += deltaTime;
            if (effectData.CurrentLifeTime > effectData.SpecVO.LifeTime)
            {
                MessageBus.Instance.ReleaseWeaponEffectData.Broadcast(effectData);
                return;
            }

            if (effectData.CollideCount < effectData.CollisionEventEffectReceiverModuleList.Count)
            {
                effectData.AddCollideCount();

                if (effectData.SpecVO.Penetration < Random.value)
                {
                    MessageBus.Instance.ReleaseWeaponEffectData.Broadcast(effectData);
                    return;
                }
            }
        }
    }
}
