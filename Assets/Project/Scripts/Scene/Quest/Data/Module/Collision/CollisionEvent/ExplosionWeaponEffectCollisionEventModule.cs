using System;
using System.Collections.Generic;

namespace AloneSpace
{
    public class ExplosionWeaponEffectCollisionEventModule : CollisionEventModule
    {
        ExplosionWeaponEffectData effectData;

        public ExplosionWeaponEffectCollisionEventModule(Guid instanceId, ExplosionWeaponEffectData effectData, CollisionShape collisionShape) : base(instanceId, effectData, collisionShape)
        {
            this.effectData = effectData;
        }

        public override void OnUpdateModule(float deltaTime, HashSet<CollisionEventModule> theirCollisions)
        {
            foreach (var theirCollision in theirCollisions)
            {
                if (effectData.PlayerInstanceId == (theirCollision.Holder as IPlayer)?.PlayerInstanceId)
                {
                    // TODO: もうちょっと綺麗に書く
                    continue;;
                }

                if (theirCollision.Receiver != null)
                {
                    MessageBus.Instance.NoticeCollisionEventEffectData.Broadcast(new CollisionEventEffectData(Sender, theirCollision.Receiver));
                }
            }
        }
    }
}