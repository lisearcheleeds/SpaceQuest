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

            string GetDistanceText(IInteractData targetData)
            {
                if (observePlayerQuestData.MainActorData.AreaId == targetData.AreaId)
                {
                    // 同一エリア内
                    return $"{(targetData.Position - observePlayerQuestData.MainActorData.Position).magnitude * 1000.0f}m";
                }

                if (observePlayerQuestData.MainActorData.AreaId.HasValue)
                {
                    // 移動中
                    string res = "";
                    MessageBus.Instance.UtilGetAreaData.Broadcast(
                        targetData.AreaId.Value,
                        targetAreaData =>
                        {
                            var offsetPosition = targetAreaData.StarSystemPosition - observePlayerQuestData.MainActorData.Position;
                            res = $"{offsetPosition.magnitude * 1000.0f}m";                                                        
                        });
                    return res;
                }

                if (observePlayerQuestData.MainActorData.AreaId != targetData.AreaId)
                {
                    // 違うエリア内
                    string res = "";
                    MessageBus.Instance.UtilGetAreaData.Broadcast(
                        observePlayerQuestData.MainActorData.AreaId.Value,
                        observeActorStarSystemPosition =>
                        {
                            MessageBus.Instance.UtilGetAreaData.Broadcast(
                                targetData.AreaId.Value,
                                targetAreaData =>
                                {
                                    var offsetPosition = targetAreaData.StarSystemPosition - observeActorStarSystemPosition.StarSystemPosition;
                                    res = $"{offsetPosition.magnitude * 1000.0f}m";
                                });
                        });
                    
                    return res;
                }

                throw new ArgumentException();
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
