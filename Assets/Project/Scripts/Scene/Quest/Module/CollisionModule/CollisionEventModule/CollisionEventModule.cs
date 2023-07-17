using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public abstract class CollisionEventModule : IModule<HashSet<CollisionEventModule>>, ICollisionEventModule
    {
        public Guid InstanceId { get; }

        public ICollisionEventModuleHolder Holder { get; }
        public CollisionEventEffectSenderModule Sender => senderHolder?.CollisionEventEffectSenderModule;
        public CollisionEventEffectReceiverModule Receiver => receiverHolder?.CollisionEventEffectReceiverModule;

        // 簡易当たり判定
        public CollisionShape SimplyCollisionShape { get; }

        ICollisionEventEffectSenderModuleHolder senderHolder;
        ICollisionEventEffectReceiverModuleHolder receiverHolder;

        protected CollisionEventModule(Guid instanceId, ICollisionEventModuleHolder holder, CollisionShape collisionShape)
        {
            InstanceId = instanceId;
            Holder = holder;
            senderHolder = holder as ICollisionEventEffectSenderModuleHolder;
            receiverHolder = holder as ICollisionEventEffectReceiverModuleHolder;
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

        public abstract void OnUpdateModule(float deltaTime, HashSet<CollisionEventModule> theirCollisions);
    }
}