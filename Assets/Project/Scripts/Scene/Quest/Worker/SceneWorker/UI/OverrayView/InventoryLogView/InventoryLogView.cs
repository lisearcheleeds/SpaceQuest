using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace.UI
{
    public class InventoryLogView : MonoBehaviour
    {
        [SerializeField] RectTransform logParent;
        [SerializeField] InventoryLogViewCell inventoryLogViewCellPrefab;

        List<InventoryLogViewCell> inventoryLogViewCellList = new List<InventoryLogViewCell>();
        List<InventoryLogViewCell.LogData> dirtyItemDataList = new List<InventoryLogViewCell.LogData>();
        
        Guid userControlActorInstanceId;

        public void Initialize()
        {
            MessageBus.Instance.User.SetControlActor.AddListener(SetUserControlActor);   
            MessageBus.Instance.Inventory.OnPickItem.AddListener(ManagerCommandPickedItem);
            MessageBus.Instance.Inventory.OnDropItem.AddListener(ManagerCommandDroppedItem);
        }

        public void Finalize()
        {
            MessageBus.Instance.User.SetControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.Inventory.OnPickItem.RemoveListener(ManagerCommandPickedItem);
            MessageBus.Instance.Inventory.OnDropItem.RemoveListener(ManagerCommandDroppedItem);
        }

        public void OnUpdate()
        {
            if (userControlActorInstanceId == default)
            {
                return;
            }

            UpdateView();
        }

        void UpdateView()
        {
            foreach (var dirtyItemData in dirtyItemDataList)
            {
                // 同一のアイテムが表示中なら相乗りする
                var prevCell = inventoryLogViewCellList.FirstOrDefault(prevData => 
                        prevData.IsUsing &&
                        prevData.CurrentLogData.LogType == dirtyItemData.LogType && 
                        prevData.CurrentLogData.ItemVO.Id == dirtyItemData.ItemVO.Id);

                if (prevCell != null)
                {
                    prevCell.Apply(new InventoryLogViewCell.LogData(prevCell.CurrentLogData, dirtyItemData.Amount));
                }
                else
                {
                    var cell = inventoryLogViewCellList.FirstOrDefault(cell => !cell.IsUsing);
                    if (cell == null)
                    {
                        cell = Instantiate(inventoryLogViewCellPrefab, logParent);
                        inventoryLogViewCellList.Add(cell);
                    }
                    cell.Apply(dirtyItemData);
                }
            }
            
            dirtyItemDataList.Clear();
        }

        void SetUserControlActor(ActorData userControlActor)
        {
            userControlActorInstanceId = userControlActor?.InstanceId ?? default;
        }
        
        void ManagerCommandPickedItem(InventoryData toInventory, ItemData pickedItem)
        {
            var index = dirtyItemDataList.FirstIndex(prevData => 
                prevData.LogType == InventoryLogViewCell.LogType.Add && 
                prevData.ItemVO.Id == pickedItem.ItemVO.Id);
            
            if (index == -1)
            {
                dirtyItemDataList.Add(new InventoryLogViewCell.LogData(
                    InventoryLogViewCell.LogType.Add,
                    pickedItem.ItemVO,
                    pickedItem.Amount ?? 1));
            }
            else
            {
                dirtyItemDataList[index] = new InventoryLogViewCell.LogData(
                    InventoryLogViewCell.LogType.Add,
                    pickedItem.ItemVO,
                    dirtyItemDataList[index].Amount + pickedItem.Amount ?? 1);
            }
        }
        
        void ManagerCommandDroppedItem(InventoryData fromInventory, ItemData droppedItem)
        {
            var index = dirtyItemDataList.FirstIndex(prevData => 
                prevData.LogType == InventoryLogViewCell.LogType.Remove && 
                prevData.ItemVO.Id == droppedItem.ItemVO.Id);
            
            if (index == -1)
            {
                dirtyItemDataList.Add(new InventoryLogViewCell.LogData(
                    InventoryLogViewCell.LogType.Remove,
                    droppedItem.ItemVO,
                    droppedItem.Amount ?? 1));
            }
            else
            {
                dirtyItemDataList[index] = new InventoryLogViewCell.LogData(
                    InventoryLogViewCell.LogType.Remove,
                    droppedItem.ItemVO,
                    dirtyItemDataList[index].Amount + droppedItem.Amount ?? 1);
            }
        }
    }
}
