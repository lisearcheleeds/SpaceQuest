using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class CollisionEventModuleUpdater
    {
        QuestData questData;

        LinkedList<CollisionEventModule> moduleList = new LinkedList<CollisionEventModule>();

        LinkedList<CollisionEventModule> registerModuleList = new LinkedList<CollisionEventModule>();
        LinkedList<CollisionEventModule> unRegisterModuleList = new LinkedList<CollisionEventModule>();

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

            foreach (var removeModule in unRegisterModuleList)
            {
                moduleList.Remove(removeModule);
            }

            unRegisterModuleList.Clear();

            foreach (var module in moduleList)
            {
                if (!collideCurrentFrame.ContainsKey(module.InstanceId) || collideCurrentFrame[module.InstanceId].Count == 0)
                {
                    continue;
                }

                module.OnUpdateModule(deltaTime, collideCurrentFrame[module.InstanceId]);
                collideCurrentFrame[module.InstanceId].Clear();
            }

            foreach (var registerModule in registerModuleList)
            {
                moduleList.AddLast(registerModule);
            }

            registerModuleList.Clear();
        }

        void RegisterCollisionEventModule(CollisionEventModule collisionEventModule)
        {
            registerModuleList.AddLast(collisionEventModule);
        }

        void UnRegisterCollisionEventModule(CollisionEventModule collisionEventModule)
        {
            unRegisterModuleList.AddLast(collisionEventModule);
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
