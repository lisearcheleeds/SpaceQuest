using System;
using System.Collections.Generic;
using System.Linq;

namespace AloneSpace
{
    public static class ActorAI
    {
        static readonly Dictionary<ActorAIState, IActorAIState> AIList = new Dictionary<ActorAIState, IActorAIState>()
        {
            { ActorAIState.None, null },
            { ActorAIState.Check, new ActorAICheck() },
            { ActorAIState.Sleep, new ActorAISleep() },
            { ActorAIState.Fight, new ActorAIFight() },
            { ActorAIState.Moving, new ActorAIMoving() },            
        };
        
        public static void Update(QuestData questData, ActorData actorData, float deltaTime)
        {
            var actorAIStateData = actorData.ActorAIStateData;

            if (!CheckStateData(actorAIStateData))
            {
                // skip
                return;
            }

            UpdateInteract(actorData, deltaTime);
            
            actorAIStateData.ActorAIState = AIList[actorAIStateData.ActorAIState]?.Update(questData, actorData, deltaTime) ?? ActorAIState.None;
            
            ClearUsedCache(actorAIStateData);
        }

        static bool CheckStateData(ActorAIStateData actorAIStateData)
        {
            if (actorAIStateData.MainTarget != null && !actorAIStateData.MainTarget.IsAlive)
            {
                actorAIStateData.MainTarget = null;
            }

            return true;
        }

        static void UpdateInteract(ActorData actorData, float deltaTime)
        {
            if (actorData.ActorAIStateData.InteractOrder == null)
            {
                actorData.ActorAIStateData.CurrentInteractingTime = 0;
                return;
            }

            if (actorData.ActorAIStateData.InteractOrder.IsInteractionRange(actorData))
            {
                actorData.ActorAIStateData.CurrentInteractingTime += deltaTime;
            }
            else
            {
                actorData.ActorAIStateData.CurrentInteractingTime = 0;
                return;
            }

            if (actorData.ActorAIStateData.CurrentInteractingTime < actorData.ActorAIStateData.InteractOrder.InteractTime)
            {
                return;
            }
            
            // インタラクト終了
            switch (actorData.ActorAIStateData.InteractOrder)
            {
                case ItemInteractData itemInteractData:
                    var insertableInventory = actorData.InventoryDataList.FirstOrDefault(x => x.VariableInventoryViewData.GetInsertableId(itemInteractData.ItemData).HasValue);
                    MessageBus.Instance.ManagerCommandPickItem.Broadcast(insertableInventory, itemInteractData);
                    break;
                case BrokenActorInteractData brokenActorInteractData:
                    throw new NotImplementedException();
                case InventoryInteractData inventoryInteractData:
                    // ユーザー操作待ち 相手のインベントリをUIでOpenする
                    throw new NotImplementedException();
                case AreaInteractData areaInteractData:
                    MessageBus.Instance.PlayerCommandSetMoveTarget.Broadcast(actorData, areaInteractData);
                    break;
            }

            actorData.ActorAIStateData.InteractOrder = null;
        }

        static void ClearUsedCache(ActorAIStateData actorAIStateData)
        {
            actorAIStateData.ThreatList.Clear();
        }
    }
}
