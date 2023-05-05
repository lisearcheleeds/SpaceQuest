using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class TargetView : MonoBehaviour
    {
        [SerializeField] ActorMarker actorMarkerPrefab;
        [SerializeField] RectTransform actorMarkerParent;
        
        List<ActorMarker> actorMarkerList = new List<ActorMarker>();
        bool isDirty;
        ActorData actorData;

        public void Initialize()
        {
            MessageBus.Instance.SetUserPlayer.AddListener(SetUserPlayer);
        }
        
        public void Finalize()
        {
            MessageBus.Instance.SetUserPlayer.RemoveListener(SetUserPlayer);
        }
        
        public void OnLateUpdate()
        {
            if (isDirty)
            {
                isDirty = false;
                RefreshWeaponDataView();
            }

            foreach (var actorMarker in actorMarkerList)
            {
                actorMarker.OnLateUpdate();
            }
        }

        void SetUserPlayer(PlayerQuestData playerQuestData)
        {
            actorData = playerQuestData.MainActorData;
            isDirty = true;
        }

        void RefreshWeaponDataView()
        {
            if (actorData?.AreaId == null)
            {
                return;
            }

            var actorDataList = MessageBus.Instance.UtilGetAreaActorData.Unicast(actorData.AreaId.Value);
            var loopMax = Mathf.Max(actorMarkerList.Count, actorDataList.Length);
            for (var i = 0; i < loopMax; i++)
            {
                if (actorMarkerList.Count <= i)
                {
                    actorMarkerList.Add(Instantiate(actorMarkerPrefab, actorMarkerParent));
                    actorMarkerList[i].Initialize(GetScreenPositionFromWorldPosition);
                }

                if (i < actorDataList.Length && actorDataList[i].InstanceId != actorData.InstanceId)
                {
                    actorMarkerList[i].SetActorData(actorDataList[i]);
                }
                else
                {
                    actorMarkerList[i].SetActorData(null);
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