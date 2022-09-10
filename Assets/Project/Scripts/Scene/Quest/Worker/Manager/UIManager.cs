using System;
using RoboQuest;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace AloneSpace
{
    public class UIManager : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] Button mapButton;
        [SerializeField] Button interactButton;
        [SerializeField] Button tacticsButton;
        [SerializeField] Button inventoryButton;
        
        [Header("Center")]
        [SerializeField] MapPanelView mapPanelView;
        [SerializeField] CameraAngleController cameraAngleController;
        [SerializeField] InteractionItemObjectList interactionItemObjectList;
        [SerializeField] TacticsView tacticsView;
        [SerializeField] ItemDataMenu itemDataMenu;
        [SerializeField] InventoryView inventoryView;
        
        [Header("3D")]
        [SerializeField] MapPanel mapPanel;
        [SerializeField] CameraAngleControllerEffect cameraAngleControllerEffect;

        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            mapPanel.Initialize(questData);
            cameraAngleControllerEffect.Initialize();
            
            mapPanelView.Initialize();
            cameraAngleController.Initialize();
            interactionItemObjectList.Initialize(questData);
            tacticsView.Initialize(OnClickTactics);
            itemDataMenu.Initialize();
            inventoryView.Initialize(questData);
            
            mapButton.onClick.AddListener(OnClickMap);
            interactButton.onClick.AddListener(OnClickInteract);
            tacticsButton.onClick.AddListener(OnClickTactics);
            inventoryButton.onClick.AddListener(OnClickInventory);
            
            MessageBus.Instance.PlayerCommandSetTacticsType.AddListener(PlayerCommandSetTacticsType);
        }

        public void Finalize()
        {
            MessageBus.Instance.PlayerCommandSetTacticsType.RemoveListener(PlayerCommandSetTacticsType);
        }

        public void ResetArea()
        {
        }

        public void OnLoadedArea()
        {
            MessageBus.Instance.UserCommandGlobalMapFocusCell.Broadcast(questData.ObserveActor.CurrentAreaIndex, true);
            interactionItemObjectList.Close();
        }

        void OnClickMap()
        {
            MessageBus.Instance.UserCommandGlobalMapFocusCell.Broadcast(questData.ObserveActor.CurrentAreaIndex, true);
            
            if (!mapPanelView.IsOpen)
            {
                mapPanelView.Open();
            }
            else
            {
                mapPanelView.Close();
            }
        }
        
        void OnClickInteract()
        {
            if (!interactionItemObjectList.IsOpen)
            {
                interactionItemObjectList.Open();
            }
            else
            {
                interactionItemObjectList.Close();
            }
            
            inventoryView.Close();
        }
        
        void OnClickTactics()
        {
            if (!tacticsView.IsOpen)
            {
                tacticsView.Open();
            }
            else
            {
                tacticsView.Close();
            }
        }
        
        void OnClickInventory()
        {
            if (!inventoryView.IsOpen)
            {
                inventoryView.Open();
                inventoryView.ApplyInventoryData(questData.ObserveActor.InventoryDataList, true);
            }
            else
            {
                inventoryView.Close();   
            }
            
            interactionItemObjectList.Close();
        }

        void OnClickTactics(TacticsType tacticsType)
        {
            MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(questData.ObservePlayer.InstanceId, tacticsType);
        }

        void PlayerCommandSetTacticsType(Guid playerInstanceId, TacticsType tacticsType)
        {
            if (questData.ObservePlayer.InstanceId == playerInstanceId)
            {
                tacticsView.ChangeTactics(tacticsType);
            }
        }
    }
}
