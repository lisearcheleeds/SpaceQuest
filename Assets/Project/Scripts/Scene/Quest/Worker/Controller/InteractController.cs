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
            
            MessageBus.Instance.ManagerCommandStoreItem.AddListener(StoreItem);
            MessageBus.Instance.ManagerCommandTransferItem.AddListener(TransferItem);
        }

        public void Finalize()
        {
            MessageBus.Instance.ManagerCommandStoreItem.RemoveListener(StoreItem);
            MessageBus.Instance.ManagerCommandTransferItem.RemoveListener(TransferItem);
        }

        void StoreItem(int areaId, InventoryData toInventory, ItemData itemData)
        {
            var insertableId = toInventory.VariableInventoryViewData.GetInsertableId(itemData);
            if (insertableId.HasValue)
            {
                // アイテムを格納
                toInventory.VariableInventoryViewData.InsertInventoryItem(insertableId.Value, itemData);
                MessageBus.Instance.UserCommandUpdateInventory.Broadcast(new[] { toInventory.InstanceId });

                // エリアデータからアイテムを削除
                var areaData = questData.StarSystemData.AreaData.First(areaData => areaData.AreaId == areaId);
                foreach (var interactData in areaData.InteractData.ToArray())
                {
                    if (!(interactData is ItemInteractData interactItemData))
                    {
                        continue;
                    }

                    if (interactItemData.ItemData == itemData)
                    {
                        areaData.RemoveInteractData(interactItemData);
                        return;
                    }
                }
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