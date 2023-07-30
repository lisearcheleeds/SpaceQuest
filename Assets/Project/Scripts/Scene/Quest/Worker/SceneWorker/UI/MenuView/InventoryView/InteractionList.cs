using System;
using UnityEngine;
using System.Linq;

namespace AloneSpace
{
    public class InteractionList : MonoBehaviour
    {
        [SerializeField] InteractionListView interactionListView;

        ActorData userControlActor;
        AreaData observeArea;

        InteractionListViewCell.CellData selectCellData;

        public void Initialize()
        {
            MessageBus.Instance.SetUserControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.SetUserObserveArea.AddListener(SetUserObserveArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetUserControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.SetUserObserveArea.RemoveListener(SetUserObserveArea);
        }

        void Refresh()
        {
            var cellData = Array.Empty<InteractionListViewCell.CellData>();
            if (observeArea != null && userControlActor != null)
            {
                cellData = observeArea.InteractData
                    .Select(interactData => new InteractionListViewCell.CellData(
                        interactData,
                        interactData.InstanceId == selectCellData?.InteractData.InstanceId,
                        GetDistanceText))
                    .ToArray();
            }

            interactionListView.Apply(cellData, OnClickSelectCell, OnClickConfirmCell);

            string GetDistanceText(IInteractData targetData)
            {
                if (userControlActor.AreaId == targetData.AreaId)
                {
                    // 同一エリア内
                    return $"{(targetData.Position - userControlActor.Position).magnitude * 1000.0f}m";
                }

                if (userControlActor.AreaId.HasValue)
                {
                    // 移動中
                    var targetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(targetData.AreaId.Value);
                    var offsetPosition = targetAreaData.StarSystemPosition - userControlActor.Position;
                    return $"{offsetPosition.magnitude * 1000.0f}m";
                }

                if (userControlActor.AreaId != targetData.AreaId)
                {
                    // 違うエリア内
                    var observeActorStarSystemPosition = MessageBus.Instance.UtilGetAreaData.Unicast(userControlActor.AreaId.Value);
                    var targetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(targetData.AreaId.Value);

                    var offsetPosition = targetAreaData.StarSystemPosition - observeActorStarSystemPosition.StarSystemPosition;
                    return $"{offsetPosition.magnitude * 1000.0f}m";
                }

                throw new ArgumentException();
            }
        }

        void SetUserControlActor(ActorData actorData)
        {
            this.userControlActor = actorData;
            Refresh();
        }

        void SetUserObserveArea(AreaData areaData)
        {
            this.observeArea = areaData;
            Refresh();
        }

        void OnClickSelectCell(InteractionListViewCell.CellData cellData)
        {
            selectCellData = cellData;
            Refresh();
        }

        void OnClickConfirmCell(InteractionListViewCell.CellData cellData)
        {
            MessageBus.Instance.PlayerCommandSetInteractOrder.Broadcast(userControlActor.InstanceId, cellData.InteractData);
        }
    }
}
