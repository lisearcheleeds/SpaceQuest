using System;
using System.Collections.Generic;

namespace AloneSpace
{
    public class ExplosionWeaponEffectCollisionEventEffectSenderModule : CollisionEventEffectSenderModule, IDamageCollisionEventEffectSenderModule
    {
        public WeaponData WeaponData => effectData.WeaponData;
        public WeaponEffectData WeaponEffectData => effectData;

        public float EffectedDamageValue { get; }

        ExplosionWeaponEffectData effectData;

        public ExplosionWeaponEffectCollisionEventEffectSenderModule(Guid instanceId, ExplosionWeaponEffectData effectData) : base(instanceId)
        {
            this.effectData = effectData;

            EffectedDamageValue = effectData.SpecVO.BaseDamage;
        }

        public override void OnUpdateModule(float deltaTime, HashSet<CollisionEventEffectReceiverModule> receiverList)
        {
            effectData.AddCollisionEventEffectList(receiverList);
        }
    }
}
