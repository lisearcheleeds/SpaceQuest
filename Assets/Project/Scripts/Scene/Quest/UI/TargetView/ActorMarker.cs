using System;
using UnityEngine;

namespace AloneSpace
{
    public class ActorMarker : MonoBehaviour
    {
        ActorData actorData;

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
            if (actorData == null)
            {
                return;
            }

            var screenPosition = getScreenPositionFromWorldPosition(actorData.Position);
            gameObject.SetActive(screenPosition.HasValue);
            if (!screenPosition.HasValue)
            {
                return;
            }

            transform.localPosition = screenPosition.Value;
        }

        public void SetActorData(ActorData actorData)
        {
            gameObject.SetActive(actorData != null);
            this.actorData = actorData;
        }
    }
}