using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class CollisionEventModule : IModule<List<CollisionEventModule>>, ICollisionEventModule
    {
        public Guid InstanceId { get; }

        // 簡易当たり判定
        public CollisionShape SimplyCollisionShape { get; }

        public CollisionEventModule(Guid instanceId, CollisionShape collisionShape)
        {
            InstanceId = instanceId;
            SimplyCollisionShape = collisionShape;
        }

        public void ActivateModule()
        {
            MessageBus.Instance.RegisterCollisionEventModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.UnRegisterCollisionEventModule.Broadcast(this);
        }

        public void OnUpdateModule(float deltaTime, List<CollisionEventModule> ourCollisions)
        {
        }
    }
}