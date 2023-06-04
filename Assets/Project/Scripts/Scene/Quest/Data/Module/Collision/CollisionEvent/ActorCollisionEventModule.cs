using System;
using System.Collections.Generic;

namespace AloneSpace
{
    public class ActorCollisionEventModule : CollisionEventModule
    {
        ActorData actorData;

        public ActorCollisionEventModule(Guid instanceId, ActorData actorData, CollisionShape collisionShape) : base(instanceId, actorData, collisionShape)
        {
            this.actorData = actorData;
        }

        public override void OnUpdateModule(float deltaTime, HashSet<CollisionEventModule> theirCollisions)
        {
        }
    }
}