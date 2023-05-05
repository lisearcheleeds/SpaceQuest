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
        ActorData actorData;

        IPositionData[] prevAroundTargets;

        public void Initialize()
        {
            MessageBus.Instance.SetUserPlayer.AddListener(SetUserPlayer);
            MessageBus.Instance.ActorCommandSetMainTarget.AddListener(ActorCommandSetMainTarget);
            MessageBus.Instance.ActorCommandSetAroundTargets.AddListener(ActorCommandSetAroundTargets);
        }
        
        public void Finalize()
        {
            MessageBus.Instance.SetUserPlayer.RemoveListener(SetUserPlayer);
            MessageBus.Instance.ActorCommandSetMainTarget.RemoveListener(ActorCommandSetMainTarget);
            MessageBus.Instance.ActorCommandSetAroundTargets.RemoveListener(ActorCommandSetAroundTargets);
        }
        
        public void OnLateUpdate()
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

        void SetUserPlayer(PlayerQuestData playerQuestData)
        {
            actorData = playerQuestData.MainActorData;
            isDirty = true;
        }

        void ActorCommandSetMainTarget(Guid instanceId, IPositionData target)
        {
            if (actorData.InstanceId == instanceId)
            {
                isDirty = true;
            }
        }

        void ActorCommandSetAroundTargets(Guid instanceId, IPositionData[] targets)
        {
            if (actorData.InstanceId == instanceId)
            {
                isDirty = true;
            }
        }

        void RefreshWeaponDataView()
        {
            if (actorData?.AreaId == null)
            {
                return;
            }

            var aroundTargets = actorData.ActorStateData.AroundTargets;
            var loopMax = Mathf.Max(targetMarkerList.Count, aroundTargets.Length);
            for (var i = 0; i < loopMax; i++)
            {
                if (targetMarkerList.Count <= i)
                {
                    targetMarkerList.Add(Instantiate(targetMarkerPrefab, actorMarkerParent));
                    targetMarkerList[i].Initialize(GetScreenPositionFromWorldPosition);
                }

                if (i < aroundTargets.Length && aroundTargets[i].InstanceId != actorData.InstanceId)
                {
                    targetMarkerList[i].SetTargetData(actorData, aroundTargets[i]);
                }
                else
                {
                    targetMarkerList[i].SetTargetData(actorData, null);
                }
            }
        }

        Vector3? GetScreenPositionFromWorldPosition(Vector3 worldPosition)
        {
            return MessageBus.Instance.UserCommandGetWorldToCanvasPoint.Unicast(CameraController.CameraType.Camera3d,
                worldPosition, actorMarkerParent);
        }
    }
}