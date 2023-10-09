using System;
using UnityEngine;
using System.Linq;

namespace AloneSpace
{
    public class InAreaItemList : MonoBehaviour
    {
        [SerializeField] InAreaItemListView inAreaItemListView;

        ActorData userControlActor;
        AreaData observeArea;

        InAreaItemListViewCell.CellData selectCellData;

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

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
            var cellData = Array.Empty<InAreaItemListViewCell.CellData>();
            if (observeArea != null && userControlActor != null)
            {
                cellData = observeArea.InteractData
                    .Where(interactData => interactData is ItemInteractData)
                    .Select(interactData => new InAreaItemListViewCell.CellData(
                        interactData,
                        interactData.InstanceId == selectCellData?.InteractData.InstanceId,
                        GetState,
                        GetDistanceText))
                    .ToArray();
            }

            inAreaItemListView.Apply(cellData, OnClickSelectCell, OnClickConfirmCell);

            ActorStateData.InteractOrderState GetState(IInteractData targetData)
            {
                if (userControlActor.ActorStateData.InteractOrderDic.ContainsKey(targetData))
                {
                    return userControlActor.ActorStateData.InteractOrderDic[targetData];
                }

                return null;
            }

            string GetDistanceText(IInteractData targetData)
            {
                if (userControlActor.AreaId == targetData.AreaId)
                {
                    // 同一エリア内
                    return $"{(targetData.Position - userControlActor.Position).magnitude * 10.0f}m";
                }

                if (userControlActor.AreaId.HasValue)
                {
                    // 移動中
                    var targetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(targetData.AreaId.Value);
                    var offsetPosition = targetAreaData.StarSystemPosition - userControlActor.Position;
                    return $"{offsetPosition.magnitude * 10.0f}m";
                }

                if (userControlActor.AreaId != targetData.AreaId)
                {
                    // 違うエリア内
                    var observeActorStarSystemPosition = MessageBus.Instance.UtilGetAreaData.Unicast(userControlActor.AreaId.Value);
                    var targetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(targetData.AreaId.Value);

                    var offsetPosition = targetAreaData.StarSystemPosition - observeActorStarSystemPosition.StarSystemPosition;
                    return $"{offsetPosition.magnitude * 10.0f}m";
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

        void OnClickSelectCell(InAreaItemListViewCell.CellData cellData)
        {
            selectCellData = cellData;
            Refresh();
        }

        void OnClickConfirmCell(InAreaItemListViewCell.CellData cellData)
        {
            if (userControlActor.ActorStateData.InteractOrderDic.ContainsKey(cellData.InteractData))
            {
                // キャンセル
                MessageBus.Instance.PlayerCommandRemoveInteractOrder.Broadcast(userControlActor.InstanceId, cellData.InteractData);
            }
            else
            {
                // 登録
                MessageBus.Instance.PlayerCommandAddInteractOrder.Broadcast(userControlActor.InstanceId, cellData.InteractData);
            }
        }
    }
}
