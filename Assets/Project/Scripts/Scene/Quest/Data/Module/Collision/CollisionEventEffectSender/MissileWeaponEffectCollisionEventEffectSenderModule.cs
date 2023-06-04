using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class MissileWeaponEffectCollisionEventEffectSenderModule : CollisionEventEffectSenderModule
    {
        MissileWeaponEffectData effectData;

        public MissileWeaponEffectCollisionEventEffectSenderModule(Guid instanceId, MissileWeaponEffectData effectData) : base(instanceId)
        {
            this.effectData = effectData;
        }

        public override void OnUpdateModule(float deltaTime, HashSet<CollisionEventEffectReceiverModule> receiverList)
        {
            effectData.AddCollisionEventEffectList(receiverList);
        }
    }
}