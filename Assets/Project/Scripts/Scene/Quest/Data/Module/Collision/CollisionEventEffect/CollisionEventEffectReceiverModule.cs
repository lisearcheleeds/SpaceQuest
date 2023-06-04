using System;
using System.Collections.Generic;

namespace AloneSpace
{
    public class CollisionEventEffectReceiverModule : IModule<List<CollisionEventEffectSenderModule>>, ICollisionEventModule
    {
        public Guid InstanceId { get; }

        public CollisionEventEffectReceiverModule(Guid instanceId)
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

        public void OnUpdateModule(float deltaTime, List<CollisionEventEffectSenderModule> senderList)
        {
            // MessageBus.Instance.NoticeCollisionEffectData.Broadcast(new CollisionEffectData(sender, receiver));
        }
    }
}