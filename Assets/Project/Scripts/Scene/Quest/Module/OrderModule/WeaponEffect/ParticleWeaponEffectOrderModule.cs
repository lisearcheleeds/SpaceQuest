﻿namespace AloneSpace
{
    public class ParticleBulletWeaponEffectOrderModule : IOrderModule
    {
        ParticleBulletWeaponEffectData effectData;

        public ParticleBulletWeaponEffectOrderModule(ParticleBulletWeaponEffectData particleBulletWeaponEffectData)
        {
            effectData = particleBulletWeaponEffectData;
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
            // MessageBus.Instance.ReleaseWeaponEffectData.Broadcast(effectData);

            if (effectData.CollideCount < effectData.CollisionEventEffectReceiverModuleList.Count)
            {
                effectData.AddCollideCount();
            }
        }
    }
}
