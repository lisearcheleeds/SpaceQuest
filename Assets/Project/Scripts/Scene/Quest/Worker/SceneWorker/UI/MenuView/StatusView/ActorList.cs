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

            if (prevAroundTargetsCount != questData.UserData.ControlActorData?.ActorStateData.AroundTargets.Length)
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
                actorListViewCellDataList = questData.UserData.ControlActorData.ActorStateData.AroundTargets
                    .OfType<ActorData>()
                    .Select(actorData => new ActorListViewCell.CellData(
                        actorData,
                        actorData.InstanceId == selectCellData?.ActorData.InstanceId,
                        GetDistanceText)).ToArray();

                prevAroundTargetsCount = questData.UserData.ControlActorData.ActorStateData.AroundTargets.Length;
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
