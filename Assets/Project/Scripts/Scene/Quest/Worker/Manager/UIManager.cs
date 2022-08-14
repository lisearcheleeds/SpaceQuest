using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RoboQuest.Quest
{
    public class UIManager : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] Button mapButton;
        [SerializeField] Button interactButton;
        [SerializeField] Button tacticsButton;
        [SerializeField] Button inventoryButton;
        
        [Header("Side")]
        [SerializeField] GlobalMapPanel miniMapPanel;
        [SerializeField] ActorDataViewList actorDataViewList;
        
        [Header("Center")]
        [SerializeField] MapPanelView mapPanelView;
        [SerializeField] InteractionItemObjectList interactionItemObjectList;
        [SerializeField] TacticsView tacticsView;
        [SerializeField] ItemDataMenu itemDataMenu;
        [SerializeField] InventoryView inventoryView;

        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            mapPanelView.Initialize(questData);
            miniMapPanel.Initialize(questData);
            actorDataViewList.Initialize(questData);
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
