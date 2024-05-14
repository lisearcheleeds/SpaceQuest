using UnityEngine;
using VariableInventorySystem;

namespace AloneSpace.UI
{
    public class InventoryList : MonoBehaviour
    {
        public StandardGridView StandardGridView => standardGridView;
        public DropAreaView DropAreaView => dropAreaView;

        [SerializeField] StandardGridView standardGridView;
        [SerializeField] DropAreaView dropAreaView;

        bool isDirty;

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.User.SetControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.Inventory.PickItem.AddListener(ManagerCommandPickItem);
            MessageBus.Instance.Inventory.TransferItem.AddListener(ManagerCommandTransferItem);

            dropAreaView.Apply(OnDropAreaDrop, GetDropAreaIsInsertableCondition, GetDropAreaIsInnerCell);
        }

        public void Finalize()
        {
            MessageBus.Instance.User.SetControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.Inventory.PickItem.RemoveListener(ManagerCommandPickItem);
            MessageBus.Instance.Inventory.TransferItem.RemoveListener(ManagerCommandTransferItem);
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
            if (questData.UserData.ControlActorData != null)
            {
                standardGridView.Apply(questData.UserData.ControlActorData.InventoryData.Inventory);
            }
        }

        void SetUserControlActor(ActorData userControlActor)
        {
            SetDirty();
        }

        void ManagerCommandPickItem(InventoryData inventoryData, ItemInteractData itemInteractData)
        {
            if (questData.UserData.ControlActorData.InventoryData.InstanceId == inventoryData.InstanceId)
            {
                SetDirty();
            }
        }

        void ManagerCommandTransferItem(InventoryData fromInventoryData, InventoryData toInventoryData, ItemData itemData)
        {
            if (questData.UserData.ControlActorData.InventoryData.InstanceId == fromInventoryData.InstanceId ||
                questData.UserData.ControlActorData.InventoryData.InstanceId == toInventoryData.InstanceId)
            {
                SetDirty();
            }
        }

        bool OnDropAreaDrop(ICellData cellData)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return false;
            }

            var variableInventoryViewData = questData.UserData.ControlActorData.InventoryData.Inventory;
            var insertableId = variableInventoryViewData.GetInsertableId(cellData);
            if (!insertableId.HasValue)
            {
                return false;
            }

            // place
            variableInventoryViewData.InsertInventoryItem(insertableId.Value, cellData);
            SetDirty();

            return true;
        }

        bool GetDropAreaIsInsertableCondition(ICellData cellData)
        {
            return questData.UserData.ControlActorData.InventoryData.Inventory.GetInsertableId(cellData).HasValue;
        }

        bool GetDropAreaIsInnerCell(ICellData cellData)
        {
            return questData.UserData.ControlActorData.InventoryData.Inventory.GetId(cellData).HasValue;
        }
    }
}
