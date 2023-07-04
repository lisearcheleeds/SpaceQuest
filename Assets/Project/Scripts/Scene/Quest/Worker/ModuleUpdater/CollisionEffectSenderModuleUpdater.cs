using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class CollisionEffectSenderModuleUpdater
    {
        QuestData questData;

        LinkedList<CollisionEventEffectSenderModule> moduleList = new LinkedList<CollisionEventEffectSenderModule>();

        LinkedList<CollisionEventEffectSenderModule> registerModuleList = new LinkedList<CollisionEventEffectSenderModule>();
        LinkedList<CollisionEventEffectSenderModule> unRegisterModuleList = new LinkedList<CollisionEventEffectSenderModule>();

        Dictionary<Guid, HashSet<CollisionEventEffectReceiverModule>> collideReceiverThisFrame = new Dictionary<Guid, HashSet<CollisionEventEffectReceiverModule>>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.RegisterCollisionEffectSenderModule.AddListener(RegisterCollisionEffectSenderModule);
            MessageBus.Instance.UnRegisterCollisionEffectSenderModule.AddListener(UnRegisterCollisionEffectSenderModule);
            MessageBus.Instance.NoticeCollisionEventEffectData.AddListener(NoticeCollisionEventEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.RegisterCollisionEffectSenderModule.RemoveListener(RegisterCollisionEffectSenderModule);
            MessageBus.Instance.UnRegisterCollisionEffectSenderModule.RemoveListener(UnRegisterCollisionEffectSenderModule);
            MessageBus.Instance.NoticeCollisionEventEffectData.RemoveListener(NoticeCollisionEventEffectData);
        }

        public void UpdateModule(float deltaTime)
        {
            if (questData == null)
            {
                return;
            }

            foreach (var removeModule in unRegisterModuleList)
            {
                moduleList.Remove(removeModule);
            }

            unRegisterModuleList.Clear();

            foreach (var module in moduleList)
            {
                if (!collideReceiverThisFrame.ContainsKey(module.InstanceId) || collideReceiverThisFrame[module.InstanceId].Count == 0)
                {
                    continue;
                }

                module.OnUpdateModule(deltaTime, collideReceiverThisFrame[module.InstanceId]);
                collideReceiverThisFrame[module.InstanceId].Clear();
            }

            foreach (var registerModule in registerModuleList)
            {
                moduleList.AddLast(registerModule);
            }

            registerModuleList.Clear();
        }

        void RegisterCollisionEffectSenderModule(CollisionEventEffectSenderModule collisionEventEffectSenderModule)
        {
            registerModuleList.AddLast(collisionEventEffectSenderModule);
        }

        void UnRegisterCollisionEffectSenderModule(CollisionEventEffectSenderModule collisionEventEffectSenderModule)
        {
            unRegisterModuleList.AddLast(collisionEventEffectSenderModule);
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
