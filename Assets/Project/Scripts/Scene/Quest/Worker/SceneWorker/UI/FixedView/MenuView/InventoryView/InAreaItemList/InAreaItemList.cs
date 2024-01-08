using System;
using UnityEngine;
using System.Linq;
using VariableInventorySystem;

namespace AloneSpace.UI
{
    public class InAreaItemList : MonoBehaviour
    {
        public DropAreaView DropAreaView => dropAreaView;

        [SerializeField] DropAreaView dropAreaView;
        [SerializeField] InAreaItemListView inAreaItemListView;

        InAreaItemListViewCell.CellData selectCellData;

        bool isDirty;

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.SetUserControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.SetUserObserveArea.AddListener(SetUserObserveArea);
            MessageBus.Instance.ManagerCommandPickedItem.AddListener(ManagerCommandPickedItem);
            MessageBus.Instance.ManagerCommandDroppedItem.AddListener(ManagerCommandDroppedItem);

            dropAreaView.Apply(OnDropAreaDrop, GetDropAreaIsInsertableCondition, GetDropAreaIsInnerCell);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetUserControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.SetUserObserveArea.RemoveListener(SetUserObserveArea);
            MessageBus.Instance.ManagerCommandPickedItem.RemoveListener(ManagerCommandPickedItem);
            MessageBus.Instance.ManagerCommandDroppedItem.RemoveListener(ManagerCommandDroppedItem);
        }

        public void SetDirty()
        {
            isDirty = true;
        }

        public void OnUpdate()
        {
            if (isDirty)
            {
                Refresh();
                isDirty = false;
            }
        }

        void Refresh()
        {
            var cellData = Array.Empty<InAreaItemListViewCell.CellData>();
            if (questData.UserData.ObserveAreaData != null && questData.UserData.ControlActorData != null)
            {
                cellData = questData.InteractData.Values
                    .Where(x => x.AreaId == questData.UserData.ObserveAreaData.AreaId)
                    .Where(interactData => interactData is ItemInteractData)
                    .Select(interactData => new InAreaItemListViewCell.CellData(
                        interactData,
                        interactData.InstanceId == selectCellData?.InteractData.InstanceId,
                        GetState,
                        GetDistanceText))
                    .ToArray();
            }

            inAreaItemListView.Apply(cellData, OnClickSelectCell, OnClickConfirmCell);

            InteractOrderState GetState(IInteractData targetData)
            {
                return questData.UserData.ControlActorData.ActorStateData.InteractOrderStateList
                    .FirstOrDefault(x => x.InteractData.InstanceId == targetData.InstanceId);
            }

            string GetDistanceText(IInteractData targetData)
            {
                if (questData.UserData.ControlActorData.AreaId == targetData.AreaId)
                {
                    // 同一エリア内
                    return $"{(targetData.Position - questData.UserData.ControlActorData.Position).magnitude :F1}m";
                }

                if (questData.UserData.ControlActorData.AreaId.HasValue)
                {
                    // 移動中
                    var targetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(targetData.AreaId.Value);
                    var offsetPosition = targetAreaData.StarSystemPosition - questData.UserData.ControlActorData.Position;
                    return $"{offsetPosition.magnitude :F1}m";
                }

                if (questData.UserData.ControlActorData.AreaId != targetData.AreaId)
                {
                    // 違うエリア内
                    var observeActorStarSystemPosition = MessageBus.Instance.UtilGetAreaData.Unicast(questData.UserData.ControlActorData.AreaId.Value);
                    var targetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(targetData.AreaId.Value);

                    var offsetPosition = targetAreaData.StarSystemPosition - observeActorStarSystemPosition.StarSystemPosition;
                    return $"{offsetPosition.magnitude :F1}m";
                }

                throw new ArgumentException();
            }
        }

        void SetUserControlActor(ActorData actorData)
        {
            SetDirty();
        }

        void SetUserObserveArea(AreaData areaData)
        {
            SetDirty();
        }

        void ManagerCommandPickedItem(InventoryData inventoryData, ItemData pickedItem)
        {
            SetDirty();
        }
        
        void ManagerCommandDroppedItem(InventoryData inventoryData, ItemData droppedItem)
        {
            SetDirty();
        }

        void OnClickSelectCell(InAreaItemListViewCell.CellData cellData)
        {
            selectCellData = cellData;
            SetDirty();
        }

        void OnClickConfirmCell(InAreaItemListViewCell.CellData cellData)
        {
            var isContains = questData.UserData.ControlActorData.ActorStateData.InteractOrderStateList
                .Any(x => x.InteractData.InstanceId == cellData.InteractData.InstanceId);
            if (isContains)
            {
                // キャンセル
                MessageBus.Instance.PlayerCommandRemoveInteractOrder.Broadcast(questData.UserData.ControlActorData.InstanceId, cellData.InteractData);
            }
            else
            {
                // 登録
                MessageBus.Instance.PlayerCommandAddInteractOrder.Broadcast(questData.UserData.ControlActorData.InstanceId, cellData.InteractData);
            }
        }

        bool OnDropAreaDrop(ICellData cellData)
        {
            if (!questData.UserData.ControlActorData.AreaId.HasValue)
            {
                return false;
            }

            MessageBus.Instance.ManagerCommandDropItem.Broadcast(questData.UserData.ControlActorData.InventoryData, cellData as ItemData);
            
            SetDirty();

            return true;
        }

        bool GetDropAreaIsInsertableCondition(ICellData cellData)
        {
            // 捨てるだけなので常にtrue
            return true;
        }

        bool GetDropAreaIsInnerCell(ICellData cellData)
        {
            // TODOリストにある場合はtrue
            return false;
        }
    }
}
