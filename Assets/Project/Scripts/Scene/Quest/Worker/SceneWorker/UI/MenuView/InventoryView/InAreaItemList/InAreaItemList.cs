using System;
using UnityEngine;
using System.Linq;

namespace AloneSpace
{
    public class InAreaItemList : MonoBehaviour
    {
        [SerializeField] InAreaItemListView inAreaItemListView;

        InAreaItemListViewCell.CellData selectCellData;

        bool isDirty;

        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.SetUserControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.SetUserObserveArea.AddListener(SetUserObserveArea);
            MessageBus.Instance.ManagerCommandPickItem.AddListener(ManagerCommandPickItem);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetUserControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.SetUserObserveArea.RemoveListener(SetUserObserveArea);
            MessageBus.Instance.ManagerCommandPickItem.RemoveListener(ManagerCommandPickItem);
        }

        public void SetDirty()
        {
            isDirty = true;
        }

        public void OnUpdate()
        {
            if (isDirty)
            {
                Refresh();
                isDirty = false;
            }
        }

        void Refresh()
        {
            var cellData = Array.Empty<InAreaItemListViewCell.CellData>();
            if (questData.UserData.ObserveAreaData != null && questData.UserData.ControlActorData != null)
            {
                cellData = questData.InteractData.Values
                    .Where(x => x.AreaId == questData.UserData.ObserveAreaData.AreaId)
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
                if (questData.UserData.ControlActorData.ActorStateData.InteractOrderDic.ContainsKey(targetData))
                {
                    return questData.UserData.ControlActorData.ActorStateData.InteractOrderDic[targetData];
                }

                return null;
            }

            string GetDistanceText(IInteractData targetData)
            {
                if (questData.UserData.ControlActorData.AreaId == targetData.AreaId)
                {
                    // 同一エリア内
                    return $"{(targetData.Position - questData.UserData.ControlActorData.Position).magnitude :F1}m";
                }

                if (questData.UserData.ControlActorData.AreaId.HasValue)
                {
                    // 移動中
                    var targetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(targetData.AreaId.Value);
                    var offsetPosition = targetAreaData.StarSystemPosition - questData.UserData.ControlActorData.Position;
                    return $"{offsetPosition.magnitude :F1}m";
                }

                if (questData.UserData.ControlActorData.AreaId != targetData.AreaId)
                {
                    // 違うエリア内
                    var observeActorStarSystemPosition = MessageBus.Instance.UtilGetAreaData.Unicast(questData.UserData.ControlActorData.AreaId.Value);
                    var targetAreaData = MessageBus.Instance.UtilGetAreaData.Unicast(targetData.AreaId.Value);

                    var offsetPosition = targetAreaData.StarSystemPosition - observeActorStarSystemPosition.StarSystemPosition;
                    return $"{offsetPosition.magnitude :F1}m";
                }

                throw new ArgumentException();
            }
        }

        void SetUserControlActor(ActorData actorData)
        {
            SetDirty();
        }

        void SetUserObserveArea(AreaData areaData)
        {
            SetDirty();
        }

        void ManagerCommandPickItem(InventoryData inventoryData, ItemInteractData itemInteractData)
        {
            if (questData.UserData.ControlActorData.AreaId == itemInteractData.AreaId)
            {
                SetDirty();
            }
        }

        void OnClickSelectCell(InAreaItemListViewCell.CellData cellData)
        {
            selectCellData = cellData;
            SetDirty();
        }

        void OnClickConfirmCell(InAreaItemListViewCell.CellData cellData)
        {
            if (questData.UserData.ControlActorData.ActorStateData.InteractOrderDic.ContainsKey(cellData.InteractData))
            {
                // キャンセル
                MessageBus.Instance.PlayerCommandRemoveInteractOrder.Broadcast(questData.UserData.ControlActorData.InstanceId, cellData.InteractData);
            }
            else
            {
                // 登録
                MessageBus.Instance.PlayerCommandAddInteractOrder.Broadcast(questData.UserData.ControlActorData.InstanceId, cellData.InteractData);
            }
        }
    }
}
