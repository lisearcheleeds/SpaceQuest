using UnityEngine;

namespace AloneSpace
{
    public class ExplosionWeaponEffectOrderModule : IOrderModule
    {
        ExplosionWeaponEffectData effectData;
        bool isWaited1Frame;

        public ExplosionWeaponEffectOrderModule(ExplosionWeaponEffectData explosionWeaponEffectData)
        {
            this.effectData = explosionWeaponEffectData;
            isWaited1Frame = false;
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
            if (!isWaited1Frame)
            {
                isWaited1Frame = true;
                return;
            }

            // すぐにRelease
            MessageBus.Instance.Creator.ReleaseWeaponEffectData.Broadcast(effectData);
        }
    }
}
