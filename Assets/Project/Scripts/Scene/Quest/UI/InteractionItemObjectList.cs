using UnityEngine;
using System.Linq;
using AloneSpace;

namespace AloneSpace
{
    public class InteractionItemObjectList : MonoBehaviour
    {
        public bool IsOpen => gameObject.activeSelf;

        [SerializeField] InteractionItemObjectListView interactionItemObjectListView;

        QuestData questData;
        
        // エリア内のドロップアイテムオブジェクト
        ItemObject[] interactionItems;
        
        // 選択中のドロップアイテムオブジェクト
        ItemObject focusInteractionItem;
        
        // 選択中のドロップアイテムデータ
        ItemData focusItemData;
        
        // 選択中のセルデータ
        InteractionItemObjectListViewCell.CellData selectCellData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            Close();
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            MessageBus.Instance.UserCommandCloseItemDataMenu.Broadcast();
            gameObject.SetActive(false);
        }

        void Refresh()
        {
            return;
            if (!interactionItems.Contains(focusInteractionItem))
            {
                Back();
            }
            
            // 表示するリスト階層を再取得
            var cellData = interactionItems
                .Select(interactItem => new InteractionItemObjectListViewCell.CellData(interactItem.ItemInteractData.ItemData))
                .ToArray();

            var takeOrderItems = questData.GetObservePlayerActors()
                .Select(x => x.ActorAICache.InteractOrder)
                .ToArray();
            
            // 表示するリスト階層を実際に表示
            // interactionItemObjectListView.Apply(cellData, takeOrderItems, selectCellData, OnClickCell);
        }

        void OnClickCell(InteractionItemObjectListViewCell.CellData cellData)
        {
            selectCellData = cellData;
            focusInteractionItem = interactionItems.FirstOrDefault(interactItem => interactItem.ItemInteractData.ItemData == cellData.ItemData);

            focusItemData = cellData.ItemData;
            
            Refresh();

            UpdateItemDataMenu();
            MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraMode.FocusObject);
            MessageBus.Instance.UserCommandSetCameraFocusObject.Broadcast(focusInteractionItem.transform);
        }
        
        void Back()
        {
            MessageBus.Instance.UserCommandCloseItemDataMenu.Broadcast();

            selectCellData = null;
            focusInteractionItem = null;
        }
        
        void OnClickInteractItemButton()
        {
            /*
            var isTakeOrderItem = questData.ObservePlayerActors.Any(x => x.InteractOrder.OfType<ItemInteractData>().Any(y => y.ItemData == focusItemData));

            if (isTakeOrderItem)
            {
                MessageBus.Instance.PlayerCommandRemoveInteractItemOrder.Broadcast(questData.ObserveActor, focusInteractionItem);
            }
            else
            {
                MessageBus.Instance.PlayerCommandAddInteractItemOrder.Broadcast(questData.ObserveActor, focusInteractionItem);
            }
            */

            UpdateItemDataMenu();
            Refresh();
        }

        void UpdateItemDataMenu()
        {
            /*
            var isTakeOrderItem = questData.ObservePlayerActors.Any(x => x.InteractOrder.OfType<ItemInteractData>().Any(y => y.ItemData == focusItemData));
            
            MessageBus.Instance.UserCommandOpenItemDataMenu.Broadcast(
                focusItemData, 
                OnClickInteractItemButton,
                isTakeOrderItem ? "Cancel" : "Take",
                $"{(focusInteractionItem.transform.position - questData.ObserveActor.Position).magnitude}m");
            */
        }
    }
}
