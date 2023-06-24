using UnityEngine;

namespace AloneSpace
{
    public class ExplosionWeaponEffectOrderModule : IOrderModule
    {
        ExplosionWeaponEffectData effectData;

        public ExplosionWeaponEffectOrderModule(ExplosionWeaponEffectData explosionWeaponEffectData)
        {
            this.effectData = explosionWeaponEffectData;
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
            // すぐにRelease
            MessageBus.Instance.ReleaseWeaponEffectData.Broadcast(effectData);
        }
    }
}
