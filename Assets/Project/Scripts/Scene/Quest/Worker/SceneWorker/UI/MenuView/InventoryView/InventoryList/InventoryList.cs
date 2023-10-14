using UnityEngine;
using VariableInventorySystem;

namespace AloneSpace
{
    public class InventoryList : MonoBehaviour
    {
        public StandardStashView StandardStashView => stashView;

        [SerializeField] StandardStashView stashView;

        bool isDirty;

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.SetUserControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.ManagerCommandPickItem.AddListener(ManagerCommandPickItem);
            MessageBus.Instance.ManagerCommandTransferItem.AddListener(ManagerCommandTransferItem);
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
    }
}
