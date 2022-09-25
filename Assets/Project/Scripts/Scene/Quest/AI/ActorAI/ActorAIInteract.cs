using System;
using System.Linq;
using AloneSpace;

namespace AloneSpace
{
    public class ActorAIInteract : IActorAIState
    {
        public ActorAIState ActorAIState => ActorAIState.Interact;
        
        public ActorAIState Update(QuestData questData, ActorData actorData, float deltaTime)
        {
            if (actorData.ActorAICache.InteractOrder == null)
            {
                return ActorAIState.Check;
            }
            
            if (!actorData.ActorAICache.AroundInteractDataList.Contains(actorData.ActorAICache.InteractOrder))
            {
                actorData.ActorAICache.InteractOrder = null;
                return ActorAIState.Check;
            }
            
            if (!actorData.ActorAICache.InteractOrder.IsInteractionRange(actorData))
            {
                return ActorAIState.Check;
            }

            actorData.ActorAICache.InteractingTime += deltaTime;
            if (actorData.ActorAICache.InteractOrder.InteractTime < actorData.ActorAICache.InteractingTime)
            {
                // インタラクト終了
                switch (actorData.ActorAICache.InteractOrder)
                {
                    case ItemInteractData itemInteractData:
                        actorData.ActorAICache.InteractOrder = null;
                        var insertableInventory = actorData.InventoryDataList.FirstOrDefault(x => x.VariableInventoryViewData.GetInsertableId(itemInteractData.ItemData).HasValue);
                        MessageBus.Instance.ManagerCommandStoreItem.Broadcast(itemInteractData.AreaIndex, insertableInventory, itemInteractData.ItemData);
                        break;
                    case BrokenActorInteractData brokenActorInteractData:
                        throw new NotImplementedException();
                        break;
                    case InventoryInteractData inventoryInteractData:
                        // ユーザー操作待ち
                        // TODO: 相手のインベントリをUIでOpenする
                        break;
                }
            }
            
            return ActorAIState.Interact;
        }
    }
}