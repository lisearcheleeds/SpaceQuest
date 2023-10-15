using UnityEngine;
using VariableInventorySystem;

namespace AloneSpace
{
    public class InventoryList : MonoBehaviour
    {
        public StandardStashView StandardStashView => stashView;
        public DropAreaView DropAreaView => dropAreaView;

        [SerializeField] StandardStashView stashView;
        [SerializeField] DropAreaView dropAreaView;

        bool isDirty;

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.SetUserControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.ManagerCommandPickItem.AddListener(ManagerCommandPickItem);
            MessageBus.Instance.ManagerCommandTransferItem.AddListener(ManagerCommandTransferItem);

            dropAreaView.Apply(OnDropAreaDrop, GetDropAreaIsInsertableCondition, GetDropAreaIsInnerCell);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetUserControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.ManagerCommandPickItem.RemoveListener(ManagerCommandPickItem);
            MessageBus.Instance.ManagerCommandTransferItem.RemoveListener(ManagerCommandTransferItem);
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
                stashView.Apply(questData.UserData.ControlActorData.InventoryData.VariableInventoryViewData);
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

        bool OnDropAreaDrop(IVariableInventoryCellData cellData)
        {
            if (questData.UserData.ControlActorData == null)
            {
                return false;
            }

            var variableInventoryViewData = questData.UserData.ControlActorData.InventoryData.VariableInventoryViewData;
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

        bool GetDropAreaIsInsertableCondition(IVariableInventoryCellData cellData)
        {
            return questData.UserData.ControlActorData.InventoryData.VariableInventoryViewData.GetInsertableId(cellData).HasValue;
        }

        bool GetDropAreaIsInnerCell(IVariableInventoryCellData cellData)
        {
            return questData.UserData.ControlActorData.InventoryData.VariableInventoryViewData.GetId(cellData).HasValue;
        }
    }
}
