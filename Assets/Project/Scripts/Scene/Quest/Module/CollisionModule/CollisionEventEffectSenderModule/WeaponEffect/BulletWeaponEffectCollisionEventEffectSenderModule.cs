using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class BulletWeaponEffectCollisionEventEffectSenderModule : CollisionEventEffectSenderModule, IDamageCollisionEventEffectSenderModule
    {
        public WeaponData WeaponData => effectData.WeaponData;
        public WeaponEffectData WeaponEffectData => effectData;

        public float EffectedDamageValue { get; }

        BulletWeaponEffectData effectData;

        public BulletWeaponEffectCollisionEventEffectSenderModule(Guid instanceId, BulletWeaponEffectData effectData) : base(instanceId)
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
