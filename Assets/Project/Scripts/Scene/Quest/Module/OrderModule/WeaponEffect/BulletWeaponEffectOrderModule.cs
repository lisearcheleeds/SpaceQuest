using UnityEngine;

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
            MessageBus.Instance.Module.RegisterOrderModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.Module.UnRegisterOrderModule.Broadcast(this);
        }

        public void OnUpdateModule(float deltaTime)
        {
            if (isFirstUpdate)
            {
                isFirstUpdate = false;
                effectData.MovingModule.SetMovementVelocity(effectData.Rotation * Vector3.forward * effectData.SpecVO.Speed);
            }

            effectData.CurrentLifeTime += deltaTime;
            if (effectData.CurrentLifeTime > effectData.SpecVO.LifeTime)
            {
                MessageBus.Instance.Data.ReleaseWeaponEffectData.Broadcast(effectData);
                return;
            }

            if (effectData.CollideCount < effectData.CollisionEventEffectReceiverModuleList.Count)
            {
                effectData.AddCollideCount();

                if (effectData.SpecVO.Penetration < Random.value)
                {
                    MessageBus.Instance.Data.ReleaseWeaponEffectData.Broadcast(effectData);
                    return;
                }
            }
        }
    }
}
