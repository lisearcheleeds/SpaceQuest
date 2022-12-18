using System;
using System.Collections;
using AloneSpace;
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
        [SerializeField] CameraAngleControllerEffect cameraAngleControllerEffect;

        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            cameraAngleControllerEffect.Initialize();
            
            mapPanelView.Initialize(questData);
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

        public void OnLoadedArea()
        {
            MessageBus.Instance.UserCommandGlobalMapFocusCell.Broadcast(questData.ObservePlayerQuestData.MainActorData.AreaId, true);
            interactionItemObjectList.Close();
        }

        void OnClickMap()
        {
            MessageBus.Instance.UserCommandGlobalMapFocusCell.Broadcast(questData.ObservePlayerQuestData.MainActorData.AreaId, true);
            
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
                inventoryView.ApplyInventoryData(questData.ObservePlayerQuestData.MainActorData.InventoryDataList, true);
            }
            else
            {
                inventoryView.Close();   
            }
            
            interactionItemObjectList.Close();
        }

        void OnClickTactics(TacticsType tacticsType)
        {
            MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(questData.ObservePlayerQuestData.MainActorData.PlayerInstanceId, tacticsType);
        }

        void PlayerCommandSetTacticsType(Guid playerInstanceId, TacticsType tacticsType)
        {
            if (questData.ObservePlayerQuestData.MainActorData.PlayerInstanceId == playerInstanceId)
            {
                tacticsView.ChangeTactics(tacticsType);
            }
        }
    }
}
