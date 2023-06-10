using System;
using System.Collections.Generic;

namespace AloneSpace
{
    public class ExplosionWeaponEffectCollisionEventEffectSenderModule : CollisionEventEffectSenderModule
    {
        ExplosionWeaponEffectData effectData;

        public ExplosionWeaponEffectCollisionEventEffectSenderModule(Guid instanceId, ExplosionWeaponEffectData effectData) : base(instanceId)
        {
            this.effectData = effectData;
        }

        public override void OnUpdateModule(float deltaTime, HashSet<CollisionEventEffectReceiverModule> receiverList)
        {
            effectData.AddCollisionEventEffectList(receiverList);
        }
    }
}