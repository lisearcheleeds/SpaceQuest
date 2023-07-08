using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class ActorList : MonoBehaviour
    {
        [SerializeField] ActorListView actorListView;

        ActorListViewCell.CellData selectCellData;
        QuestData questData;

        int prevAroundTargetsCount;
        bool isDirty;

        ActorListViewCell.CellData[] actorListViewCellDataList;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.SetUserControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.CreatedActorData.AddListener(CreatedActorData);
            MessageBus.Instance.ReleaseActorData.AddListener(ReleaseActorData);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetUserControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.CreatedActorData.RemoveListener(CreatedActorData);
            MessageBus.Instance.ReleaseActorData.RemoveListener(ReleaseActorData);
        }

        public void OnUpdate()
        {
            if (selectCellData == null)
            {
                if (actorListViewCellDataList?.FirstOrDefault() != null)
                {
                    OnClickSelectCell(actorListViewCellDataList?.FirstOrDefault());
                }
            }

            var currentAroundTargetsCount = 0;
            if (questData.UserData.ControlActorData != null)
            {
                currentAroundTargetsCount = MessageBus.Instance.GetFrameCacheActorRelationData.Unicast(questData.UserData.ControlActorData.InstanceId).Count;
            }

            if (prevAroundTargetsCount != currentAroundTargetsCount)
            {
                isDirty = true;
            }

            if (!isDirty || !enabled)
            {
                return;
            }

            isDirty = false;
            Refresh();
        }

        void Refresh()
        {
            if (questData.UserData.ControlActorData != null)
            {
                var aroundTargets = MessageBus.Instance.GetFrameCacheActorRelationData.Unicast(questData.UserData.ControlActorData.InstanceId);
                actorListViewCellDataList = aroundTargets
                    .Select(actorRelationData => new ActorListViewCell.CellData(
                        actorRelationData.OtherActorData,
                        actorRelationData.OtherActorData.InstanceId == selectCellData?.ActorData.InstanceId,
                        GetDistanceText)).ToArray();

                prevAroundTargetsCount = aroundTargets.Count;
            }
            else
            {
                actorListViewCellDataList = Array.Empty<ActorListViewCell.CellData>();
                prevAroundTargetsCount = 0;
            }

            actorListView.Apply(actorListViewCellDataList.ToArray(), OnClickSelectCell);

            string GetDistanceText(IPositionData targetData)
            {
                if (questData.UserData.ControlActorData == null)
                {
                    return "";
                }

                return $"{(targetData.Position - questData.UserData.ControlActorData.Position).magnitude}";
            }
        }

        void SetUserControlActor(ActorData actorData)
        {
            isDirty = true;
        }

        void CreatedActorData(ActorData actorData)
        {
            isDirty = true;
        }

        void ReleaseActorData(ActorData actorData)
        {
            isDirty = true;
        }

        void OnClickSelectCell(ActorListViewCell.CellData cellData)
        {
            selectCellData = cellData;
            isDirty = true;

            MessageBus.Instance.UIMenuStatusViewSelectActorData.Broadcast(cellData?.ActorData);
        }
    }
}
