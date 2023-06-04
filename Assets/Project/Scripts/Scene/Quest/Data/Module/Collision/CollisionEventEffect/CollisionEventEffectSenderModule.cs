using System;
using System.Collections.Generic;

namespace AloneSpace
{
    public class CollisionEventEffectSenderModule : IModule<List<CollisionEventEffectReceiverModule>>, ICollisionEventModule
    {
        public Guid InstanceId { get; }

        public CollisionEventEffectSenderModule(Guid instanceId)
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

        public void OnUpdateModule(float deltaTime, List<CollisionEventEffectReceiverModule> receiverList)
        {
        }
    }
}