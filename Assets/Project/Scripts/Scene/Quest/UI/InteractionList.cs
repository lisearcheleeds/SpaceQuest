using UnityEngine;
using System.Linq;

namespace AloneSpace
{
    public class InteractionList : MonoBehaviour
    {
        [SerializeField] InteractionListView interactionListView;

        QuestData questData;
     
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
            var cellData = questData.StarSystemData.AreaData
                .Select(areaData => new InteractionListViewCell.CellData(
                    areaData,
                    selectCellData?.AreaData.AreaId == areaData.AreaId,
                    GetDistanceText))
                .ToArray();
            
            interactionListView.Apply(cellData, OnClickSelectCell, OnClickConfirmCell);

            string GetDistanceText(AreaData areaData)
            {
                return $"{(questData.ObserveAreaData.Position - areaData.Position).magnitude * 1000.0f}m";
            }
        }

        void OnClickSelectCell(InteractionListViewCell.CellData cellData)
        {
            selectCellData = cellData;
            Refresh();
        }

        void OnClickConfirmCell(InteractionListViewCell.CellData cellData)
        {
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
