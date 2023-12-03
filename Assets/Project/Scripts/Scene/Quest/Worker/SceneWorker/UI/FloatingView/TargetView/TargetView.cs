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
        }

        public void Finalize()
        {
            MessageBus.Instance.SetUserControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.ActorCommandSetMainTarget.RemoveListener(ActorCommandSetMainTarget);
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

        void RefreshWeaponDataView()
        {
            if (userControlActor?.AreaId == null)
            {
                return;
            }

            var aroundTargets = MessageBus.Instance.GetFrameCacheActorRelationData.Unicast(userControlActor.InstanceId);
            var loopMax = Mathf.Max(targetMarkerList.Count, aroundTargets.Count);
            for (var i = 0; i < loopMax; i++)
            {
                if (targetMarkerList.Count <= i)
                {
                    targetMarkerList.Add(Instantiate(targetMarkerPrefab, actorMarkerParent));
                    targetMarkerList[i].Initialize(GetScreenPositionFromWorldPosition);
                }

                if (i < aroundTargets.Count && aroundTargets[i].OtherActorData.InstanceId != userControlActor.InstanceId)
                {
                    targetMarkerList[i].SetTargetData(userControlActor, aroundTargets[i].OtherActorData);
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
