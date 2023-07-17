using System;
using System.Collections.Generic;

namespace AloneSpace
{
    public class MissileWeaponEffectCollisionEventEffectSenderModule : CollisionEventEffectSenderModule, IDamageCollisionEventEffectSenderModule
    {
        public WeaponData WeaponData => effectData.WeaponData;
        public WeaponEffectData WeaponEffectData => effectData;

        public float EffectedDamageValue { get; private set; }

        MissileWeaponEffectData effectData;

        public MissileWeaponEffectCollisionEventEffectSenderModule(Guid instanceId, MissileWeaponEffectData effectData) : base(instanceId)
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
