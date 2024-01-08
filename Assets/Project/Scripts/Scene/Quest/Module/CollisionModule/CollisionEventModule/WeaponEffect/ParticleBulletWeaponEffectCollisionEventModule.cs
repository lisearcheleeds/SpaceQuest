using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class ParticleBulletWeaponEffectCollisionEventModule : CollisionEventModule
    {
        ParticleBulletWeaponEffectData effectData;

        public ParticleBulletWeaponEffectCollisionEventModule(Guid instanceId, ParticleBulletWeaponEffectData effectData, CollisionShape collisionShape) : base(instanceId, effectData, collisionShape)
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
                    MessageBus.Instance.Temp.NoticeCollisionEventEffectData.Broadcast(new CollisionEventEffectData(Sender, theirCollision.Receiver));
                }
            }
        }
    }
}
