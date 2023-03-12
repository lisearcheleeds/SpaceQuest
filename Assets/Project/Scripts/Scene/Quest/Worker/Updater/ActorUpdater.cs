using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class ActorUpdater : IUpdater
    {
        // 1秒間に更新を行うレート
        static readonly float TickRate = 0.0f;//0.1f / 1.0f;
        
        QuestData questData;

        Dictionary<Guid, float> updateTimeStamps = new Dictionary<Guid, float>();
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.NoticeHitCollision.AddListener(NoticeHitCollision);
            MessageBus.Instance.NoticeDamage.AddListener(NoticeDamage);
            MessageBus.Instance.NoticeBroken.AddListener(NoticeBroken);
            
            MessageBus.Instance.PlayerCommandSetInteractOrder.AddListener(PlayerCommandSetInteractOrder);
            MessageBus.Instance.PlayerCommandSetAreaId.AddListener(PlayerCommandSetAreaId);
            MessageBus.Instance.PlayerCommandSetMoveTarget.AddListener(PlayerCommandSetMoveTarget);
            MessageBus.Instance.NoticeHitThreat.AddListener(NoticeHitThreat);
            
            MessageBus.Instance.ActorCommandForwardBoosterPowerRatio.AddListener(ActorCommandForwardBoosterPowerRatio);
            MessageBus.Instance.ActorCommandBackBoosterPowerRatio.AddListener(ActorCommandBackBoosterPowerRatio);
            MessageBus.Instance.ActorCommandRightBoosterPowerRatio.AddListener(ActorCommandRightBoosterPowerRatio);
            MessageBus.Instance.ActorCommandLeftBoosterPowerRatio.AddListener(ActorCommandLeftBoosterPowerRatio);
            MessageBus.Instance.ActorCommandTopBoosterPowerRatio.AddListener(ActorCommandTopBoosterPowerRatio);
            MessageBus.Instance.ActorCommandBottomBoosterPowerRatio.AddListener(ActorCommandBottomBoosterPowerRatio);
            MessageBus.Instance.ActorCommandPitchBoosterPowerRatio.AddListener(ActorCommandPitchBoosterPowerRatio);
            MessageBus.Instance.ActorCommandRollBoosterPowerRatio.AddListener(ActorCommandRollBoosterPowerRatio);
            MessageBus.Instance.ActorCommandYawBoosterPowerRatio.AddListener(ActorCommandYawBoosterPowerRatio);
            
            MessageBus.Instance.ActorCommandLookAt.AddListener(ActorCommandLookAt);
            MessageBus.Instance.ActorCommandRotateToLookAt.AddListener(ActorCommandRotateToLookAt);
            
            MessageBus.Instance.ActorCommandSetActorMode.AddListener(ActorCommandSetActorMode);
            MessageBus.Instance.ActorCommandSetActorCombatMode.AddListener(ActorCommandSetActorCombatMode);
        }

        public void Finalize()
        {
            MessageBus.Instance.NoticeHitCollision.RemoveListener(NoticeHitCollision);
            MessageBus.Instance.NoticeDamage.RemoveListener(NoticeDamage);
            MessageBus.Instance.NoticeBroken.RemoveListener(NoticeBroken);
            
            MessageBus.Instance.PlayerCommandSetInteractOrder.RemoveListener(PlayerCommandSetInteractOrder);
            MessageBus.Instance.PlayerCommandSetAreaId.RemoveListener(PlayerCommandSetAreaId);
            MessageBus.Instance.PlayerCommandSetMoveTarget.RemoveListener(PlayerCommandSetMoveTarget);
            MessageBus.Instance.NoticeHitThreat.RemoveListener(NoticeHitThreat);
            
            MessageBus.Instance.ActorCommandForwardBoosterPowerRatio.RemoveListener(ActorCommandForwardBoosterPowerRatio);
            MessageBus.Instance.ActorCommandBackBoosterPowerRatio.RemoveListener(ActorCommandBackBoosterPowerRatio);
            MessageBus.Instance.ActorCommandRightBoosterPowerRatio.RemoveListener(ActorCommandRightBoosterPowerRatio);
            MessageBus.Instance.ActorCommandLeftBoosterPowerRatio.RemoveListener(ActorCommandLeftBoosterPowerRatio);
            MessageBus.Instance.ActorCommandTopBoosterPowerRatio.RemoveListener(ActorCommandTopBoosterPowerRatio);
            MessageBus.Instance.ActorCommandBottomBoosterPowerRatio.RemoveListener(ActorCommandBottomBoosterPowerRatio);
            MessageBus.Instance.ActorCommandPitchBoosterPowerRatio.RemoveListener(ActorCommandPitchBoosterPowerRatio);
            MessageBus.Instance.ActorCommandRollBoosterPowerRatio.RemoveListener(ActorCommandRollBoosterPowerRatio);
            MessageBus.Instance.ActorCommandYawBoosterPowerRatio.RemoveListener(ActorCommandYawBoosterPowerRatio);
            
            MessageBus.Instance.ActorCommandLookAt.RemoveListener(ActorCommandLookAt);
            MessageBus.Instance.ActorCommandRotateToLookAt.RemoveListener(ActorCommandRotateToLookAt);
            
            MessageBus.Instance.ActorCommandSetActorMode.RemoveListener(ActorCommandSetActorMode);
            MessageBus.Instance.ActorCommandSetActorCombatMode.RemoveListener(ActorCommandSetActorCombatMode);
        }
        
        public void OnLateUpdate()
        {
            if (questData == null)
            {
                return;
            }

            foreach (var actorData in questData.ActorData)
            {
                if (!updateTimeStamps.ContainsKey(actorData.InstanceId))
                {
                    updateTimeStamps[actorData.InstanceId] = Time.time - TickRate - 1.0f;
                }

                if (updateTimeStamps[actorData.InstanceId] < Time.time - TickRate)
                {
                    var deltaTime = Time.time - updateTimeStamps[actorData.InstanceId];
                    updateTimeStamps[actorData.InstanceId] += deltaTime;
                    
                    actorData.Update(deltaTime);
                    ActorAI.Update(questData, actorData, deltaTime);
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
            areaData.AddInteractData(new InventoryInteractData(inventoryData, actorData.AreaId.Value, actorData.Position, actorData.Rotation));
            
            // 更新
            MessageBus.Instance.SetDirtyActorObjectList.Broadcast();
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
        
        void ActorCommandForwardBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData.First(x => x.InstanceId == actorId).SetForwardBoosterPowerRatio(power);
        }
        
        void ActorCommandBackBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData.First(x => x.InstanceId == actorId).SetBackBoosterPowerRatio(power);
        }
        
        void ActorCommandRightBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData.First(x => x.InstanceId == actorId).SetRightBoosterPowerRatio(power);
        }
        
        void ActorCommandLeftBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData.First(x => x.InstanceId == actorId).SetLeftBoosterPowerRatio(power);
        }
        
        void ActorCommandTopBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData.First(x => x.InstanceId == actorId).SetTopBoosterPowerRatio(power);
        }
        
        void ActorCommandBottomBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData.First(x => x.InstanceId == actorId).SetBottomBoosterPowerRatio(power);
        }
        
        void ActorCommandPitchBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData.First(x => x.InstanceId == actorId).SetPitchBoosterPowerRatio(power);
        }
        
        void ActorCommandRollBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData.First(x => x.InstanceId == actorId).SetRollBoosterPowerRatio(power);
        }
        
        void ActorCommandYawBoosterPowerRatio(Guid actorId, float power)
        {
            questData.ActorData.First(x => x.InstanceId == actorId).SetYawBoosterPowerRatio(power);
        }

        void ActorCommandLookAt(Guid actorId, Vector3 lookAt)
        {
            questData.ActorData.First(x => x.InstanceId == actorId).SetLookAt(lookAt);
        }
        
        void ActorCommandRotateToLookAt(Guid actorId, bool isRotateToLookAt)
        {
            questData.ActorData.First(x => x.InstanceId == actorId).SetRotateToLookAt(isRotateToLookAt);
        }
        
        void ActorCommandSetActorMode(Guid actorId, ActorMode actorMode)
        {
            questData.ActorData.First(x => x.InstanceId == actorId).SetActorMode(actorMode);
        }

        void ActorCommandSetActorCombatMode(Guid actorId, ActorCombatMode actorCombatMode)
        {
            questData.ActorData.First(x => x.InstanceId == actorId).SetActorCombatMode(actorCombatMode);
        }
    }
}