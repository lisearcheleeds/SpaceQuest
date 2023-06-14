using UnityEngine;

namespace AloneSpace
{
    public class ExplosionWeaponEffectOrderModule : IOrderModule
    {
        ExplosionWeaponEffectData effectData;
        bool isFirstUpdate;

        public ExplosionWeaponEffectOrderModule(ExplosionWeaponEffectData explosionWeaponEffectData)
        {
            this.effectData = explosionWeaponEffectData;
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
            }

            effectData.CurrentLifeTime += deltaTime;
            if (effectData.CurrentLifeTime > effectData.LifeTime)
            {
                MessageBus.Instance.ReleaseWeaponEffectData.Broadcast(effectData);
                return;
            }
        }
    }
}
