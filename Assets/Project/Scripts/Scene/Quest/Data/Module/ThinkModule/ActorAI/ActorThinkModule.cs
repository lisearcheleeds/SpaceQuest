using System;
using System.Collections.Generic;
using System.Linq;

namespace AloneSpace
{
    public class ActorThinkModule : IThinkModule
    {
        static readonly Dictionary<ActorAIState, IActorAIState> AIList = new Dictionary<ActorAIState, IActorAIState>()
        {
            { ActorAIState.None, null },
            { ActorAIState.Check, new ActorAICheck() },
            { ActorAIState.Sleep, new ActorAISleep() },
            { ActorAIState.Fight, new ActorAIFight() },
            { ActorAIState.Moving, new ActorAIMoving() },            
        };

        ActorData actorData;
        
        public Guid InstanceId => actorData.InstanceId;
        
        public ActorThinkModule(ActorData actorData)
        {
            this.actorData = actorData;
        }

        public void ActivateModule()
        {
            MessageBus.Instance.RegisterThinkModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.UnRegisterThinkModule.Broadcast(this);
        }

        public void OnUpdateModule(float deltaTime)
        {
            var actorAIStateData = actorData.ActorStateData;

            if (!CheckStateData(actorAIStateData))
            {
                // skip
                return;
            }

            UpdateInteract(actorData, deltaTime);
            
            actorAIStateData.ActorAIState = AIList[actorAIStateData.ActorAIState]?.Update(actorData, deltaTime) ?? ActorAIState.None;
            
            ClearUsedCache(actorAIStateData);
        }
        
        static bool CheckStateData(ActorStateData actorStateData)
        {
            if (actorStateData.MainTarget != null)
            {
                actorStateData.MainTarget = null;
            }

            return true;
        }

        static void UpdateInteract(ActorData actorData, float deltaTime)
        {
            if (actorData.ActorStateData.InteractOrder == null)
            {
                actorData.ActorStateData.CurrentInteractingTime = 0;
                return;
            }

            if (actorData.ActorStateData.InteractOrder.IsInteractionRange(actorData))
            {
                actorData.ActorStateData.CurrentInteractingTime += deltaTime;
            }
            else
            {
                actorData.ActorStateData.CurrentInteractingTime = 0;
                return;
            }

            if (actorData.ActorStateData.CurrentInteractingTime < actorData.ActorStateData.InteractOrder.InteractTime)
            {
                return;
            }
            
            // インタラクト終了
            switch (actorData.ActorStateData.InteractOrder)
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

            actorData.ActorStateData.InteractOrder = null;
        }

        static void ClearUsedCache(ActorStateData actorStateData)
        {
        }
    }
}
