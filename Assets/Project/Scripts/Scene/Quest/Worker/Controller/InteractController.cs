using System;
using System.Collections;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class InteractController
    {
        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.ManagerCommandPickItem.AddListener(PickItem);
            MessageBus.Instance.ManagerCommandTransferItem.AddListener(TransferItem);
        }

        public void Finalize()
        {
            MessageBus.Instance.ManagerCommandPickItem.RemoveListener(PickItem);
            MessageBus.Instance.ManagerCommandTransferItem.RemoveListener(TransferItem);
        }

        void PickItem(InventoryData toInventory, ItemInteractData pickItem)
        {
            var insertableId = toInventory.VariableInventoryViewData.GetInsertableId(pickItem.ItemData);
            if (insertableId.HasValue)
            {
                // アイテムを格納
                toInventory.VariableInventoryViewData.InsertInventoryItem(insertableId.Value, pickItem.ItemData);
                MessageBus.Instance.UserCommandUpdateInventory.Broadcast(new[] { toInventory.InstanceId });

                // エリアデータからアイテムを削除
                var areaData = questData.StarSystemData.AreaData.First(x => x.AreaId == pickItem.AreaId);
                areaData.RemoveInteractData(pickItem);
            }
            else
            {
                // InteractItem.InteractItemを確認
                throw new ObjectDisposedException("ObjectDisposedException");
            }
        }
        
        void TransferItem(InventoryData fromInventory, InventoryData toInventory, ItemData itemData)
        {
            var removableId = fromInventory.VariableInventoryViewData.GetId(itemData);
            var insertableId = toInventory.VariableInventoryViewData.GetInsertableId(itemData);
            if (removableId.HasValue && insertableId.HasValue)
            {
                // アイテムを格納
                toInventory.VariableInventoryViewData.InsertInventoryItem(insertableId.Value, itemData);
                fromInventory.VariableInventoryViewData.RemoveInventoryItem(removableId.Value);
                
                MessageBus.Instance.UserCommandUpdateInventory.Broadcast(new[] { toInventory.InstanceId, fromInventory.InstanceId });
            }
            else
            {
                // InteractItem.InteractItemを確認
                throw new ObjectDisposedException("ObjectDisposedException");
            }
        }
    }
}