using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace.UI
{
    public class ActorList : MonoBehaviour
    {
        [SerializeField] ActorListView actorListView;

        ActorListViewCell.CellData selectCellData;
        QuestData questData;

        bool isDirty;

        List<ActorListViewCell.CellData> actorListViewCellDataList = new List<ActorListViewCell.CellData>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.User.SetControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.Data.OnCreateActorData.AddListener(OnCreateActorData);
            MessageBus.Instance.Data.OnReleaseActorData.AddListener(OnReleaseActorData);
        }

        public void Finalize()
        {
            MessageBus.Instance.User.SetControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.Data.OnCreateActorData.RemoveListener(OnCreateActorData);
            MessageBus.Instance.Data.OnReleaseActorData.RemoveListener(OnReleaseActorData);
        }

        public void SetDirty()
        {
            isDirty = true;
        }

        public void OnUpdate()
        {
            if (!isDirty || !enabled)
            {
                return;
            }

            isDirty = false;
            Refresh();
        }

        void Refresh()
        {
            actorListViewCellDataList.Clear();

            if (questData.UserData.ControlActorData != null)
            {
                // 自機
                var controlActorDataCellData = new ActorListViewCell.CellData(
                    questData.UserData.ControlActorData,
                    selectCellData == null || questData.UserData.ControlActorData.InstanceId == selectCellData.ActorData.InstanceId,
                    GetDistanceText);

                if (selectCellData == null)
                {
                    // TODO: 割りと使い捨て気味なので気が向いたら考える
                    OnClickSelectCell(controlActorDataCellData);
                    return;
                }

                actorListViewCellDataList.Add(controlActorDataCellData);

                // 他
                var aroundTargets = MessageBus.Instance.FrameCache.GetActorRelationData.Unicast(questData.UserData.ControlActorData.InstanceId);
                actorListViewCellDataList.AddRange(
                    aroundTargets
                        .Select(actorRelationData => new ActorListViewCell.CellData(
                            actorRelationData.OtherActorData,
                            actorRelationData.OtherActorData.InstanceId == selectCellData.ActorData.InstanceId,
                            GetDistanceText)));
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

        void OnCreateActorData(ActorData actorData)
        {
            isDirty = true;
        }

        void OnReleaseActorData(ActorData actorData)
        {
            isDirty = true;
        }

        void OnClickSelectCell(ActorListViewCell.CellData cellData)
        {
            selectCellData = cellData;
            SetDirty();

            MessageBus.Instance.UserInput.UIMenuStatusViewSelectActorData.Broadcast(cellData?.ActorData);
        }
    }
}
