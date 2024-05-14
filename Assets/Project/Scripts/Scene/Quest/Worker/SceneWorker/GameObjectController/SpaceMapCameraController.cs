using System;
using UnityEngine;

namespace AloneSpace
{
    public class SpaceMapCameraController : MonoBehaviour
    {
        [SerializeField] Camera spaceMapCamera;

        IPositionData trackingTarget;
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            // MessageBus.Instance.UserInput.UserCommandSetCameraTrackTarget.AddListener(UserCommandSetCameraTrackTarget);
            MessageBus.Instance.Util.GetWorldToCanvasPoint.SetListener(UserCommandGetWorldToCanvasPoint);
        }

        public void Finalize()
        {
            // MessageBus.Instance.UserInput.UserCommandSetCameraTrackTarget.RemoveListener(UserCommandSetCameraTrackTarget);
            MessageBus.Instance.Util.GetWorldToCanvasPoint.SetListener(null);
        }

        public void OnUpdate()
        {
            var targetPosition = trackingTarget?.Position ?? Vector3.zero;
            var lookAtRotation = GetLookAtRotation(questData.UserData);

            spaceMapCamera.transform.rotation = lookAtRotation;
            spaceMapCamera.transform.position = targetPosition + lookAtRotation * new Vector3(0, 0, -2000.0f - questData.UserData.SpaceMapLookAtDistance * 10.0f);
        }

        void UserCommandSetCameraTrackTarget(IPositionData cameraTrackTarget)
        {
            trackingTarget = cameraTrackTarget;
        }

        Vector3? UserCommandGetWorldToCanvasPoint(CameraType cameraType, Vector3 worldPos, RectTransform rectTransform)
        {
            return null;
            /*
            var screenPoint = spaceMapCamera.WorldToScreenPoint(worldPos);
            if (screenPoint.z < 0)
            {
                return null;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                screenPoint,
                cameraUi,
                out var localPoint);

            return localPoint;
            */
        }

        static Quaternion GetLookAtRotation(UserData userData)
        {
            return Quaternion.identity
                   * Quaternion.AngleAxis(userData.SpaceMapLookAtAngle.y, Vector3.up)
                   * Quaternion.AngleAxis(userData.SpaceMapLookAtAngle.x, Vector3.right);
        }
    }
}
