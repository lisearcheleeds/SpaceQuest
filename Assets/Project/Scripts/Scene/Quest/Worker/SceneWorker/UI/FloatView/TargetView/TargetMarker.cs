﻿using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace.UI
{
    public class TargetMarker : MonoBehaviour
    {
        ActorData fromActorData;
        IPositionData targetData;

        [SerializeField] GameObject targetMark;
        [SerializeField] GameObject objectMark;

        Func<Vector3, Vector3?> getScreenPositionFromWorldPosition;

        public void Initialize(Func<Vector3, Vector3?> getScreenPositionFromWorldPosition)
        {
            this.getScreenPositionFromWorldPosition = getScreenPositionFromWorldPosition;
        }

        public void Finalize()
        {
        }

        public void OnLateUpdate()
        {
            if (targetData == null)
            {
                return;
            }

            var screenPosition = getScreenPositionFromWorldPosition(targetData.Position);
            if (gameObject.activeSelf != screenPosition.HasValue)
            {
                gameObject.SetActive(screenPosition.HasValue);
            }

            if (!screenPosition.HasValue)
            {
                return;
            }

            transform.localPosition = screenPosition.Value;
        }

        public void SetTargetData(ActorData fromActorData, IPositionData targetData)
        {
            this.fromActorData = fromActorData;
            this.targetData = targetData;
            gameObject.SetActive(targetData != null);

            UpdateView();
        }

        void UpdateView()
        {
            targetMark.SetActive(targetData != null && MessageBus.Instance.FrameCache.GetActorRelationData.Unicast(fromActorData.InstanceId).Any(x => x.OtherActorData.InstanceId == targetData.InstanceId));
            objectMark.SetActive(false);
        }
    }
}
