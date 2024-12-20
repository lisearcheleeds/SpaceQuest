﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class CollisionEffectReceiverModuleUpdater
    {
        QuestData questData;

        Dictionary<Guid, CollisionEventEffectReceiverModule> moduleList = new Dictionary<Guid, CollisionEventEffectReceiverModule>();
        Dictionary<Guid, HashSet<CollisionEventEffectSenderModule>> collideSenderThisFrame = new Dictionary<Guid, HashSet<CollisionEventEffectSenderModule>>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.Module.RegisterCollisionEffectReceiverModule.AddListener(RegisterCollisionEffectReceiverModule);
            MessageBus.Instance.Module.UnRegisterCollisionEffectReceiverModule.AddListener(UnRegisterCollisionEffectReceiverModule);
            MessageBus.Instance.Temp.NoticeCollisionEventEffectData.AddListener(NoticeCollisionEventEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.Module.RegisterCollisionEffectReceiverModule.RemoveListener(RegisterCollisionEffectReceiverModule);
            MessageBus.Instance.Module.UnRegisterCollisionEffectReceiverModule.RemoveListener(UnRegisterCollisionEffectReceiverModule);
            MessageBus.Instance.Temp.NoticeCollisionEventEffectData.RemoveListener(NoticeCollisionEventEffectData);
        }

        public void UpdateModule(float deltaTime)
        {
            if (questData == null)
            {
                return;
            }

            foreach (var kv in collideSenderThisFrame)
            {
                moduleList[kv.Key].OnUpdateModule(deltaTime, kv.Value);
                kv.Value.Clear();
            }

            collideSenderThisFrame.Clear();
        }

        void RegisterCollisionEffectReceiverModule(CollisionEventEffectReceiverModule collisionEventEffectReceiverModule)
        {
            moduleList.Add(collisionEventEffectReceiverModule.InstanceId, collisionEventEffectReceiverModule);
        }

        void UnRegisterCollisionEffectReceiverModule(CollisionEventEffectReceiverModule collisionEventEffectReceiverModule)
        {
            moduleList.Remove(collisionEventEffectReceiverModule.InstanceId);
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
