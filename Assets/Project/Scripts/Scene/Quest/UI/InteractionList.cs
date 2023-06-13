using System;
using UnityEngine;
using System.Linq;

namespace AloneSpace
{
    public class InteractionList : MonoBehaviour
    {
        [SerializeField] InteractionListView interactionListView;

        QuestData questData;
        PlayerData observePlayerData;
        AreaData observeAreaData;
     
        InteractionListViewCell.CellData selectCellData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.UserInputSwitchInteractList.AddListener(UserInputSwitchInteractList);
            MessageBus.Instance.UserInputOpenInteractList.AddListener(UserInputOpenInteractList);
            MessageBus.Instance.UserInputCloseInteractList.AddListener(UserInputCloseInteractList);
            
            MessageBus.Instance.SetUserPlayer.AddListener(SetUserPlayer);
            MessageBus.Instance.SetUserArea.AddListener(SetUserArea);
            
            UserInputCloseInteractList();
        }

        public void Finalize()
        {
            MessageBus.Instance.UserInputSwitchInteractList.RemoveListener(UserInputSwitchInteractList);
            MessageBus.Instance.UserInputOpenInteractList.RemoveListener(UserInputOpenInteractList);
            MessageBus.Instance.UserInputCloseInteractList.RemoveListener(UserInputCloseInteractList);
            
            MessageBus.Instance.SetUserPlayer.RemoveListener(SetUserPlayer);
            MessageBus.Instance.SetUserArea.RemoveListener(SetUserArea);
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
                if (observePlayerData.MainActorData.AreaId == targetData.AreaId)
                {
                    // 同一エリア内
                    return $"{(targetData.Position - observePlayerData.MainActorData.Position).magnitude * 1000.0f}m";
                }

                if (observePlayerData.MainActorData.AreaId.HasValue)
                {
                    // 移動中
                    var targetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(targetData.AreaId.Value);
                    var offsetPosition = targetAreaData.StarSystemPosition - observePlayerData.MainActorData.Position;
                    return $"{offsetPosition.magnitude * 1000.0f}m";
                }

                if (observePlayerData.MainActorData.AreaId != targetData.AreaId)
                {
                    // 違うエリア内
                    var observeActorStarSystemPosition = MessageBus.Instance.UtilGetAreaData.Unicast(observePlayerData.MainActorData.AreaId.Value);
                    var targetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(targetData.AreaId.Value);

                    var offsetPosition = targetAreaData.StarSystemPosition - observeActorStarSystemPosition.StarSystemPosition;
                    return $"{offsetPosition.magnitude * 1000.0f}m";
                }

                throw new ArgumentException();
            }
        }

        void SetUserPlayer(PlayerData playerData)
        {
            this.observePlayerData = playerData;
            Refresh();
        }
        
        void SetUserArea(AreaData observeAreaData)
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
                observePlayerData.MainActorData,
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
