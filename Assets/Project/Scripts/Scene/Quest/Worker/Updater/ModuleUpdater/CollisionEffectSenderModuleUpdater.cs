using System;
using System.Collections.Generic;

namespace AloneSpace
{
    public class CollisionEffectSenderModuleUpdater
    {
        QuestData questData;

        List<CollisionEventEffectSenderModule> moduleList = new List<CollisionEventEffectSenderModule>();

        List<CollisionEventEffectSenderModule> registerModuleList = new List<CollisionEventEffectSenderModule>();
        List<CollisionEventEffectSenderModule> unRegisterModuleList = new List<CollisionEventEffectSenderModule>();

        Dictionary<Guid, List<CollisionEventEffectReceiverModule>> collideReceiverThisFrame = new Dictionary<Guid, List<CollisionEventEffectReceiverModule>>();

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
                if (!collideReceiverThisFrame.ContainsKey(module.InstanceId))
                {
                    collideReceiverThisFrame[module.InstanceId] = new List<CollisionEventEffectReceiverModule>();
                }

                module.OnUpdateModule(deltaTime, collideReceiverThisFrame[module.InstanceId]);
                collideReceiverThisFrame[module.InstanceId].Clear();
            }

            foreach (var registerModule in registerModuleList)
            {
                moduleList.Add(registerModule);
            }

            registerModuleList.Clear();
        }

        void RegisterCollisionEffectSenderModule(CollisionEventEffectSenderModule collisionEventEffectSenderModule)
        {
            registerModuleList.Add(collisionEventEffectSenderModule);
        }

        void UnRegisterCollisionEffectSenderModule(CollisionEventEffectSenderModule collisionEventEffectSenderModule)
        {
            unRegisterModuleList.Add(collisionEventEffectSenderModule);
        }

        void NoticeCollisionEventEffectData(CollisionEventEffectData effectData)
        {
            // Senderごとに管理する
            if (!collideReceiverThisFrame.ContainsKey(effectData.SenderModule.InstanceId))
            {
                collideReceiverThisFrame[effectData.SenderModule.InstanceId] = new List<CollisionEventEffectReceiverModule>();
            }

            collideReceiverThisFrame[effectData.SenderModule.InstanceId].Add(effectData.ReceiverModule);
        }
    }
}