using System;
using System.Collections.Generic;

namespace AloneSpace
{
    public abstract class CollisionEventEffectReceiverModule : IModule<HashSet<CollisionEventEffectSenderModule>>, ICollisionEventModule
    {
        public Guid InstanceId { get; }

        protected CollisionEventEffectReceiverModule(Guid instanceId)
        {
            InstanceId = instanceId;
        }

        public void ActivateModule()
        {
            MessageBus.Instance.RegisterCollisionEffectReceiverModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.UnRegisterCollisionEffectReceiverModule.Broadcast(this);
        }

        public abstract void OnUpdateModule(float deltaTime, HashSet<CollisionEventEffectSenderModule> senderList);
    }
}
