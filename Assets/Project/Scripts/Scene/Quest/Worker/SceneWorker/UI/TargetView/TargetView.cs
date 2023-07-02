using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class TargetView : MonoBehaviour
    {
        [SerializeField] TargetMarker targetMarkerPrefab;
        [SerializeField] RectTransform actorMarkerParent;

        List<TargetMarker> targetMarkerList = new List<TargetMarker>();
        bool isDirty;
        ActorData userControlActor;

        IPositionData[] prevAroundTargets;

        public void Initialize()
        {
            MessageBus.Instance.SetUserControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.ActorCommandSetMainTarget.AddListener(ActorCommandSetMainTarget);
            MessageBus.Instance.ActorCommandSetAroundTargets.AddListener(ActorCommandSetAroundTargets);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetUserControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.ActorCommandSetMainTarget.RemoveListener(ActorCommandSetMainTarget);
            MessageBus.Instance.ActorCommandSetAroundTargets.RemoveListener(ActorCommandSetAroundTargets);
        }

        public void OnUpdate()
        {
            if (isDirty)
            {
                isDirty = false;
                RefreshWeaponDataView();
            }

            foreach (var actorMarker in targetMarkerList)
            {
                actorMarker.OnLateUpdate();
            }
        }

        void SetUserControlActor(ActorData userControlActor)
        {
            this.userControlActor = userControlActor;
            isDirty = true;
        }

        void ActorCommandSetMainTarget(Guid instanceId, IPositionData target)
        {
            if (userControlActor == null || userControlActor?.InstanceId == instanceId)
            {
                isDirty = true;
            }
        }

        void ActorCommandSetAroundTargets(Guid instanceId, IPositionData[] targets)
        {
            if (userControlActor == null || userControlActor?.InstanceId == instanceId)
            {
                isDirty = true;
            }
        }

        void RefreshWeaponDataView()
        {
            if (userControlActor?.AreaId == null)
            {
                return;
            }

            var aroundTargets = userControlActor.ActorStateData.AroundTargets;
            var loopMax = Mathf.Max(targetMarkerList.Count, aroundTargets.Length);
            for (var i = 0; i < loopMax; i++)
            {
                if (targetMarkerList.Count <= i)
                {
                    targetMarkerList.Add(Instantiate(targetMarkerPrefab, actorMarkerParent));
                    targetMarkerList[i].Initialize(GetScreenPositionFromWorldPosition);
                }

                if (i < aroundTargets.Length && aroundTargets[i].InstanceId != userControlActor.InstanceId)
                {
                    targetMarkerList[i].SetTargetData(userControlActor, aroundTargets[i]);
                }
                else
                {
                    targetMarkerList[i].SetTargetData(userControlActor, null);
                }
            }
        }

        Vector3? GetScreenPositionFromWorldPosition(Vector3 worldPosition)
        {
            return MessageBus.Instance.UserCommandGetWorldToCanvasPoint.Unicast(CameraType.Camera3d,
                worldPosition, actorMarkerParent);
        }
    }
}
