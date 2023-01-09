using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class UIManager : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] Button mapButton;
        [SerializeField] Button interactButton;
        [SerializeField] Button inventoryButton;
        
        [Header("Center")]
        [SerializeField] MapPanelView mapPanelView;
        [SerializeField] CameraAngleController cameraAngleController;
        [SerializeField] InteractionList interactionList;
        [SerializeField] ItemDataMenu itemDataMenu;
        [SerializeField] InventoryView inventoryView;
        
        [Header("3D")]
        [SerializeField] CameraAngleControllerEffect cameraAngleControllerEffect;

        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            cameraAngleControllerEffect.Initialize();
            
            mapPanelView.Initialize(questData);
            cameraAngleController.Initialize();
            interactionList.Initialize(questData);
            itemDataMenu.Initialize();
            inventoryView.Initialize(questData);
            
            mapButton.onClick.AddListener(OnClickMap);
            interactButton.onClick.AddListener(OnClickInteract);
            inventoryButton.onClick.AddListener(OnClickInventory);
        }

        public void Finalize()
        {
        }

        public void OnLateUpdate()
        {
        }

        public void SetObservePlayerQuestData(PlayerQuestData playerQuestData)
        {
            interactionList.SetObservePlayerQuestData(playerQuestData);
            inventoryView.SetObservePlayerQuestData(playerQuestData);
        }
        
        public void SetObserveAreaData(AreaData areaData)
        {
            mapPanelView.SetObserveAreaData(areaData);
            interactionList.SetObserveAreaData(areaData);
        }

        void OnClickMap()
        {
            MessageBus.Instance.UserInputSwitchMap.Broadcast();
        }
        
        void OnClickInteract()
        {
            MessageBus.Instance.UserInputSwitchInteractList.Broadcast();
        }
        
        void OnClickInventory()
        {
            MessageBus.Instance.UserInputSwitchInventory.Broadcast();
        }
    }
}
