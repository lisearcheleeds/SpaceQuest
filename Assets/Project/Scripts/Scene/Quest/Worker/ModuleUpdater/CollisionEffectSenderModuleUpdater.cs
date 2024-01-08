using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class CollisionEffectSenderModuleUpdater
    {
        QuestData questData;

        Dictionary<Guid, CollisionEventEffectSenderModule> moduleList = new Dictionary<Guid, CollisionEventEffectSenderModule>();
        Dictionary<Guid, HashSet<CollisionEventEffectReceiverModule>> collideReceiverThisFrame = new Dictionary<Guid, HashSet<CollisionEventEffectReceiverModule>>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.Module.RegisterCollisionEffectSenderModule.AddListener(RegisterCollisionEffectSenderModule);
            MessageBus.Instance.Module.UnRegisterCollisionEffectSenderModule.AddListener(UnRegisterCollisionEffectSenderModule);
            MessageBus.Instance.Temp.NoticeCollisionEventEffectData.AddListener(NoticeCollisionEventEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.Module.RegisterCollisionEffectSenderModule.RemoveListener(RegisterCollisionEffectSenderModule);
            MessageBus.Instance.Module.UnRegisterCollisionEffectSenderModule.RemoveListener(UnRegisterCollisionEffectSenderModule);
            MessageBus.Instance.Temp.NoticeCollisionEventEffectData.RemoveListener(NoticeCollisionEventEffectData);
        }

        public void UpdateModule(float deltaTime)
        {
            if (questData == null)
            {
                return;
            }

            foreach (var kv in collideReceiverThisFrame)
            {
                moduleList[kv.Key].OnUpdateModule(deltaTime, kv.Value);
                kv.Value.Clear();
            }

            collideReceiverThisFrame.Clear();
        }

        void RegisterCollisionEffectSenderModule(CollisionEventEffectSenderModule collisionEventEffectSenderModule)
        {
            moduleList.Add(collisionEventEffectSenderModule.InstanceId, collisionEventEffectSenderModule);
        }

        void UnRegisterCollisionEffectSenderModule(CollisionEventEffectSenderModule collisionEventEffectSenderModule)
        {
            moduleList.Remove(collisionEventEffectSenderModule.InstanceId);
        }

        void NoticeCollisionEventEffectData(CollisionEventEffectData effectData)
        {
            // Senderごとに管理する
            if (!collideReceiverThisFrame.ContainsKey(effectData.SenderModule.InstanceId))
            {
                collideReceiverThisFrame[effectData.SenderModule.InstanceId] = new HashSet<CollisionEventEffectReceiverModule>();
            }

            collideReceiverThisFrame[effectData.SenderModule.InstanceId].Add(effectData.ReceiverModule);
        }
    }
}
