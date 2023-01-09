using System;
using System.Collections.Generic;
using System.Linq;
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
            
            MessageBus.Instance.PlayerCommandSetInteractOrder.AddListener(PlayerCommandSetInteractOrder);
            MessageBus.Instance.PlayerCommandSetAreaId.AddListener(PlayerCommandSetAreaId);
            MessageBus.Instance.PlayerCommandSetMoveTarget.AddListener(PlayerCommandSetMoveTarget);
            MessageBus.Instance.NoticeHitThreat.AddListener(NoticeHitThreat);
        }

        public void Finalize()
        {
            MessageBus.Instance.NoticeHitCollision.RemoveListener(NoticeHitCollision);
            MessageBus.Instance.NoticeDamage.RemoveListener(NoticeDamage);
            MessageBus.Instance.NoticeBroken.RemoveListener(NoticeBroken);
            
            MessageBus.Instance.AddActorData.RemoveListener(AddActorData);
            MessageBus.Instance.RemoveActorData.RemoveListener(RemoveActorData);
            
            MessageBus.Instance.PlayerCommandSetInteractOrder.RemoveListener(PlayerCommandSetInteractOrder);
            MessageBus.Instance.PlayerCommandSetAreaId.RemoveListener(PlayerCommandSetAreaId);
            MessageBus.Instance.PlayerCommandSetMoveTarget.RemoveListener(PlayerCommandSetMoveTarget);
            MessageBus.Instance.NoticeHitThreat.RemoveListener(NoticeHitThreat);
        }
        
        public void OnLateUpdate()
        {
            if (questData == null)
            {
                return;
            }

            var deltaTime = Time.deltaTime;

            foreach (var actorData in questData.ActorData)
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

            // ダメージチェック
            foreach (var actorData in questData.ActorData)
            {
                if (actorData.IsBroken)
                {
                    MessageBus.Instance.NoticeBroken.Broadcast(actorData);
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
            damageableData.OnDamage(new DamageData(causeDamageData, damageableData));
        }
        
        void NoticeBroken(IDamageableData damageableData)
        {
            if (!(damageableData is ActorData actorData))
            {
                return;
            }

            var areaData = questData.StarSystemData.AreaData.First(areaData => areaData.AreaId == actorData.AreaId);
            
            // 一覧から削除
            questData.ActorData.Remove(actorData);
            
            // 残骸を設置
            var interactBrokenActorData = new BrokenActorInteractData(actorData);
            areaData.AddInteractData(interactBrokenActorData);
            
            // 適当なアイテムを設置
            var inventoryData = ItemDataVOHelper.GetActorDropInventoryData(actorData);
            areaData.AddInteractData(new InventoryInteractData(inventoryData, actorData.AreaId, actorData.Position));
        }
        
        void AddActorData(ActorData actorData)
        {
            // 中継するだけ
            MessageBus.Instance.SendCollision.Broadcast(actorData, true);
        }

        void RemoveActorData(ActorData actorData)
        {
            // 中継するだけ
            MessageBus.Instance.SendCollision.Broadcast(actorData, false);
        }
        
        void PlayerCommandSetInteractOrder(ActorData orderActor, IInteractData interactData)
        {
            orderActor.SetInteractOrder(interactData);
        }

        void PlayerCommandSetAreaId(ActorData orderActor, int? areaId)
        {
            orderActor.SetAreaId(areaId);
            MessageBus.Instance.SetDirtyActorObjectList.Broadcast();
        }

        void PlayerCommandSetMoveTarget(ActorData orderActor, IPositionData moveTarget)
        {
            orderActor.SetMoveTarget(moveTarget);
        }

        void NoticeHitThreat(IThreatData threatData, ICollisionData collisionData)
        {
            (collisionData as ActorData)?.AddThreat(threatData);
        }
    }
}