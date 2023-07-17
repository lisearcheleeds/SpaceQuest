using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class ParticleBulletWeaponEffectCollisionEventEffectSenderModule : CollisionEventEffectSenderModule, IDamageCollisionEventEffectSenderModule
    {
        public WeaponData WeaponData => effectData.WeaponData;
        public WeaponEffectData WeaponEffectData => effectData;

        public float EffectedDamageValue { get; }

        ParticleBulletWeaponEffectData effectData;

        public ParticleBulletWeaponEffectCollisionEventEffectSenderModule(Guid instanceId, ParticleBulletWeaponEffectData effectData) : base(instanceId)
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
