using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class BulletWeaponEffectCollisionEventEffectSenderModule : CollisionEventEffectSenderModule
    {
        BulletWeaponEffectData effectData;

        public BulletWeaponEffectCollisionEventEffectSenderModule(Guid instanceId, BulletWeaponEffectData effectData) : base(instanceId)
        {
            this.effectData = effectData;
        }

        public override void OnUpdateModule(float deltaTime, HashSet<CollisionEventEffectReceiverModule> receiverList)
        {
            effectData.AddCollisionEventEffectList(receiverList);
        }
    }
}