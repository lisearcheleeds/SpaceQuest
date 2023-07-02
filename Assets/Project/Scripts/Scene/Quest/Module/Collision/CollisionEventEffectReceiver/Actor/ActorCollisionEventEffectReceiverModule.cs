using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class ActorCollisionEventEffectReceiverModule : CollisionEventEffectReceiverModule
    {
        ActorData actorData;

        List<Guid> receivedDamageSender = new List<Guid>();

        public ActorCollisionEventEffectReceiverModule(Guid instanceId, ActorData actorData) : base(instanceId)
        {
            this.actorData = actorData;
        }

        public override void OnUpdateModule(float deltaTime, HashSet<CollisionEventEffectSenderModule> senderList)
        {
            foreach (var sender in senderList)
            {
                if (sender is IDamageCollisionEventEffectSenderModule damageSender)
                {
                    // 一つのWeaponEffectDataからは1回のみ処理する
                    if (!receivedDamageSender.Contains(damageSender.WeaponEffectData.InstanceId))
                    {
                        receivedDamageSender.Add(damageSender.WeaponEffectData.InstanceId);

                        var damageEventData = new DamageEventData(
                            damageSender.WeaponData,
                            damageSender.WeaponEffectData,
                            actorData,
                            damageSender.EffectedDamageValue);
                        MessageBus.Instance.NoticeDamageEventData.Broadcast(damageEventData);
                    }
                }
            }
        }
    }
}
