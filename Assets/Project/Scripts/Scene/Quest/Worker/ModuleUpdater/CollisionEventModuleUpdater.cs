using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class CollisionEventModuleUpdater
    {
        QuestData questData;

        Dictionary<Guid, CollisionEventModule> moduleList = new Dictionary<Guid, CollisionEventModule>();
        Dictionary<Guid, HashSet<CollisionEventModule>> collideCurrentFrame = new Dictionary<Guid, HashSet<CollisionEventModule>>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.RegisterCollisionEventModule.AddListener(RegisterCollisionEventModule);
            MessageBus.Instance.UnRegisterCollisionEventModule.AddListener(UnRegisterCollisionEventModule);

            MessageBus.Instance.NoticeCollisionEventData.AddListener(NoticeCollisionEventData);
        }

        public void Finalize()
        {
            MessageBus.Instance.RegisterCollisionEventModule.RemoveListener(RegisterCollisionEventModule);
            MessageBus.Instance.UnRegisterCollisionEventModule.RemoveListener(UnRegisterCollisionEventModule);

            MessageBus.Instance.NoticeCollisionEventData.RemoveListener(NoticeCollisionEventData);
        }

        public void UpdateModule(float deltaTime)
        {
            if (questData == null)
            {
                return;
            }

            foreach (var kv in collideCurrentFrame)
            {
                moduleList[kv.Key].OnUpdateModule(deltaTime, kv.Value);
                kv.Value.Clear();
            }

            collideCurrentFrame.Clear();
        }

        void RegisterCollisionEventModule(CollisionEventModule collisionEventModule)
        {
            moduleList.Add(collisionEventModule.InstanceId, collisionEventModule);
        }

        void UnRegisterCollisionEventModule(CollisionEventModule collisionEventModule)
        {
            moduleList.Remove(collisionEventModule.InstanceId);
        }

        void NoticeCollisionEventData(CollisionEventData collisionEventData)
        {
            // それぞれ左右入れ替わって2回登録されるが、Distinctしても同じ計算量だと思うのでそのまま管理する
            if (!collideCurrentFrame.ContainsKey(collisionEventData.CollisionEventModule1.InstanceId))
            {
                collideCurrentFrame[collisionEventData.CollisionEventModule1.InstanceId] = new HashSet<CollisionEventModule>();
            }

            collideCurrentFrame[collisionEventData.CollisionEventModule1.InstanceId].Add(collisionEventData.CollisionEventModule2);
        }
    }
}
