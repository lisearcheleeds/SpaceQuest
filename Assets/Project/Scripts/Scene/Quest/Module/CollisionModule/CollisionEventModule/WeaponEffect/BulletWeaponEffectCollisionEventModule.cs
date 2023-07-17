using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class BulletWeaponEffectCollisionEventModule : CollisionEventModule
    {
        BulletWeaponEffectData effectData;

        public BulletWeaponEffectCollisionEventModule(Guid instanceId, BulletWeaponEffectData effectData, CollisionShape collisionShape) : base(instanceId, effectData, collisionShape)
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
                    continue;
                }

                if (theirCollision.Receiver != null)
                {
                    MessageBus.Instance.NoticeCollisionEventEffectData.Broadcast(new CollisionEventEffectData(Sender, theirCollision.Receiver));
                }
            }
        }
    }
}
