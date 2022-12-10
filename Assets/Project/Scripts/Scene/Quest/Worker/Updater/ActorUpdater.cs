using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class ActorUpdater : IUpdater
    {
        static readonly float UpdateInterval = 3.0f;
        
        QuestData questData;

        Dictionary<Guid, float> updateTimeStamps = new Dictionary<Guid, float>();
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.NoticeHitCollision.AddListener(NoticeHitCollision);
            MessageBus.Instance.NoticeDamage.AddListener(NoticeDamage);
            MessageBus.Instance.NoticeBroken.AddListener(NoticeBroken);
            
            MessageBus.Instance.AddActorData.AddListener(AddActorData);
            MessageBus.Instance.RemoveActorData.AddListener(RemoveActorData);
        }

        public void Finalize()
        {
            MessageBus.Instance.NoticeHitCollision.RemoveListener(NoticeHitCollision);
            MessageBus.Instance.NoticeDamage.RemoveListener(NoticeDamage);
            MessageBus.Instance.NoticeBroken.RemoveListener(NoticeBroken);
            
            MessageBus.Instance.AddActorData.RemoveListener(AddActorData);
            MessageBus.Instance.RemoveActorData.RemoveListener(RemoveActorData);
        }
        
        public void OnLateUpdate()
        {
            if (questData == null)
            {
                return;
            }

            var deltaTime = Time.deltaTime;

            // modifiedになる可能性があるのでコピー ほんとはやめる
            foreach (var actorData in questData.ActorData.ToArray())
            {
                if (!updateTimeStamps.ContainsKey(actorData.InstanceId))
                {
                    updateTimeStamps[actorData.InstanceId] = Time.time - UpdateInterval - 1.0f;
                }

                if (updateTimeStamps[actorData.InstanceId] < Time.time - UpdateInterval)
                {
                    actorData.Update(deltaTime);
                    ActorAI.Update(questData, actorData, deltaTime);

                    updateTimeStamps[actorData.InstanceId] = Time.time;
                }
            }
        }

        void NoticeHitCollision(ICollisionData collision1, ICollisionData collision2)
        {
            (collision1 as ActorData)?.OnCollision(collision2);
            (collision2 as ActorData)?.OnCollision(collision1);
        }

        void NoticeDamage(ICauseDamageData causeDamageData, IDamageableData damageableData)
        {
            var prevIsBroken = damageableData.IsBroken;
            
            damageableData.OnDamage(new DamageData(causeDamageData, damageableData));

            if (!prevIsBroken && damageableData.IsBroken)
            {
                MessageBus.Instance.NoticeBroken.Broadcast(damageableData);
            }
        }
        
        void NoticeBroken(IDamageableData damageableData)
        {
            if (!(damageableData is ActorData actorData))
            {
                return;
            }

            var areaIndex = actorData.AreaId;
            
            // 一覧から削除
            questData.ActorData.Remove(actorData);
            
            // 残骸を設置
            var interactBrokenActorData = new BrokenActorInteractData(actorData);
            questData.StarSystemData.AreaData[areaIndex].AddInteractData(interactBrokenActorData);
            
            // 適当なアイテムを設置
            var inventoryData = ItemDataVOHelper.GetActorDropInventoryData(actorData);
            questData.StarSystemData.AreaData[areaIndex].AddInteractData(new InventoryInteractData(inventoryData, actorData));
        }
        
        void AddActorData(ActorData actorData)
        {
            MessageBus.Instance.SendCollision.Broadcast(actorData, true);
        }

        void RemoveActorData(ActorData actorData)
        {
            MessageBus.Instance.SendCollision.Broadcast(actorData, false);
        }
    }
}