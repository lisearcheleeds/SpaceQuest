﻿using System;
using System.Collections.Generic;

namespace AloneSpace
{
    public abstract class CollisionEventEffectSenderModule : IModule<HashSet<CollisionEventEffectReceiverModule>>, ICollisionEventModule
    {
        public Guid InstanceId { get; }

        protected CollisionEventEffectSenderModule(Guid instanceId)
        {
            InstanceId = instanceId;
        }

        public void ActivateModule()
        {
            MessageBus.Instance.RegisterCollisionEffectSenderModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.UnRegisterCollisionEffectSenderModule.Broadcast(this);
        }

        public abstract void OnUpdateModule(float deltaTime, HashSet<CollisionEventEffectReceiverModule> receiverList);
    }
}