using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class CollisionEffectReceiverModuleUpdater
    {
        QuestData questData;

        LinkedList<CollisionEventEffectReceiverModule> moduleList = new LinkedList<CollisionEventEffectReceiverModule>();

        LinkedList<CollisionEventEffectReceiverModule> registerModuleList = new LinkedList<CollisionEventEffectReceiverModule>();
        LinkedList<CollisionEventEffectReceiverModule> unRegisterModuleList = new LinkedList<CollisionEventEffectReceiverModule>();

        Dictionary<Guid, HashSet<CollisionEventEffectSenderModule>> collideSenderThisFrame = new Dictionary<Guid, HashSet<CollisionEventEffectSenderModule>>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.RegisterCollisionEffectReceiverModule.AddListener(RegisterCollisionEffectReceiverModule);
            MessageBus.Instance.UnRegisterCollisionEffectReceiverModule.AddListener(UnRegisterCollisionEffectReceiverModule);
            MessageBus.Instance.NoticeCollisionEventEffectData.AddListener(NoticeCollisionEventEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.RegisterCollisionEffectReceiverModule.RemoveListener(RegisterCollisionEffectReceiverModule);
            MessageBus.Instance.UnRegisterCollisionEffectReceiverModule.RemoveListener(UnRegisterCollisionEffectReceiverModule);
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
                if (!collideSenderThisFrame.ContainsKey(module.InstanceId) || collideSenderThisFrame[module.InstanceId].Count == 0)
                {
                    continue;
                }

                module.OnUpdateModule(deltaTime, collideSenderThisFrame[module.InstanceId]);
                collideSenderThisFrame[module.InstanceId].Clear();
            }

            foreach (var registerModule in registerModuleList)
            {
                moduleList.AddLast(registerModule);
            }

            registerModuleList.Clear();
        }

        void RegisterCollisionEffectReceiverModule(CollisionEventEffectReceiverModule collisionEventEffectReceiverModule)
        {
            registerModuleList.AddLast(collisionEventEffectReceiverModule);
        }

        void UnRegisterCollisionEffectReceiverModule(CollisionEventEffectReceiverModule collisionEventEffectReceiverModule)
        {
            unRegisterModuleList.AddLast(collisionEventEffectReceiverModule);
        }

        void NoticeCollisionEventEffectData(CollisionEventEffectData effectData)
        {
            // Receiverごとに管理する
            if (!collideSenderThisFrame.ContainsKey(effectData.ReceiverModule.InstanceId))
            {
                collideSenderThisFrame[effectData.ReceiverModule.InstanceId] = new HashSet<CollisionEventEffectSenderModule>();
            }

            collideSenderThisFrame[effectData.ReceiverModule.InstanceId].Add(effectData.SenderModule);
        }
    }
}
