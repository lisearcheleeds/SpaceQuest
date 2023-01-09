using System;
using UnityEngine;
using System.Linq;

namespace AloneSpace
{
    public class InteractionList : MonoBehaviour
    {
        [SerializeField] InteractionListView interactionListView;

        QuestData questData;
        PlayerQuestData observePlayerQuestData;
        AreaData observeAreaData;
     
        InteractionListViewCell.CellData selectCellData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.UserInputSwitchInteractList.AddListener(UserInputSwitchInteractList);
            MessageBus.Instance.UserInputOpenInteractList.AddListener(UserInputOpenInteractList);
            MessageBus.Instance.UserInputCloseInteractList.AddListener(UserInputCloseInteractList);
            
            UserInputCloseInteractList();
        }

        void Refresh()
        {
            var cellData = Array.Empty<InteractionListViewCell.CellData>();
            if (observeAreaData != null)
            {
                cellData = observeAreaData.InteractData
                    .Select(interactData => new InteractionListViewCell.CellData(
                        interactData,
                        interactData.InstanceId == selectCellData?.InteractData.InstanceId,
                        GetDistanceText))
                    .ToArray();
            }
            
            interactionListView.Apply(cellData, OnClickSelectCell, OnClickConfirmCell);

            string GetDistanceText(IInteractData positionData)
            {
                var res = "";
                MessageBus.Instance.UtilGetOffsetStarSystemPosition.Broadcast(
                    observePlayerQuestData.MainActorData,
                    positionData,
                    offsetStarSystemPosition => res = $"{offsetStarSystemPosition.magnitude * 1000.0f}m");
                return res;
            }
        }

        public void SetObservePlayerQuestData(PlayerQuestData playerQuestData)
        {
            this.observePlayerQuestData = playerQuestData;
            Refresh();
        }
        
        public void SetObserveAreaData(AreaData observeAreaData)
        {
            this.observeAreaData = observeAreaData;
            Refresh();
        }

        void OnClickSelectCell(InteractionListViewCell.CellData cellData)
        {
            selectCellData = cellData;
            Refresh();
        }

        void OnClickConfirmCell(InteractionListViewCell.CellData cellData)
        {
            MessageBus.Instance.PlayerCommandSetInteractOrder.Broadcast(
                observePlayerQuestData.MainActorData,
                cellData.InteractData);
        }
        
        void UserInputSwitchInteractList()
        {
            if (gameObject.activeSelf)
            {
                MessageBus.Instance.UserInputCloseInteractList.Broadcast();
            }
            else
            {
                MessageBus.Instance.UserInputOpenInteractList.Broadcast();
            }
        }
        
        void UserInputOpenInteractList()
        {
            gameObject.SetActive(true);

            Refresh();
        }
        
        void UserInputCloseInteractList()
        {
            gameObject.SetActive(false);
        }
    }
}
