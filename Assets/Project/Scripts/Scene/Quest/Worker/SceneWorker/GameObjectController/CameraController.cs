using System;
using UnityEngine;

namespace AloneSpace
{
    public class CameraController : MonoBehaviour
    {
        static readonly float CameraModeSwitchTime = 1.0f;

        [SerializeField] Camera cameraAmbient;
        [SerializeField] Camera cameraArea;
        [SerializeField] Camera camera3d;

        [SerializeField] Camera cameraUi;

        Vector3 currentAmbientPosition = Vector3.zero;
        Vector3 currentPosition = Vector3.zero;
        Quaternion currentRotation = Quaternion.identity;

        IPositionData trackingTarget;
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.UserCommandSetCameraTrackTarget.AddListener(UserCommandSetCameraTrackTarget);
            MessageBus.Instance.UserCommandGetWorldToCanvasPoint.SetListener(UserCommandGetWorldToCanvasPoint);
        }

        public void Finalize()
        {
            MessageBus.Instance.UserCommandSetCameraTrackTarget.RemoveListener(UserCommandSetCameraTrackTarget);
            MessageBus.Instance.UserCommandGetWorldToCanvasPoint.SetListener(null);
        }

        public void OnUpdate()
        {
            var targetPosition = trackingTarget?.Position ?? Vector3.zero;
            var targetAmbientPosition = GetTargetAmbientPosition(trackingTarget);
            var targetRotation = GetTargetRotation(questData.UserData);

            currentRotation = Quaternion.Lerp(currentRotation, targetRotation, 0.4f);
            cameraAmbient.transform.rotation = currentRotation;
            cameraArea.transform.rotation = currentRotation;
            camera3d.transform.rotation = currentRotation;

            currentAmbientPosition = Vector3.Lerp(currentAmbientPosition, targetAmbientPosition, 0.05f);
            cameraAmbient.transform.position = currentAmbientPosition;

            currentPosition = Vector3.Lerp(currentPosition, targetPosition, 0.05f);
            var lookAtDistance = Mathf.Abs(questData.UserData.LookAtDistance);
            var cameraOffsetPosition = currentPosition + currentRotation * new Vector3(0, lookAtDistance, lookAtDistance * -4.0f);
            cameraArea.transform.position = cameraOffsetPosition;
            camera3d.transform.position = cameraOffsetPosition;
        }

        void UserCommandSetCameraTrackTarget(IPositionData cameraTrackTarget)
        {
            trackingTarget = cameraTrackTarget;
        }

        Vector3? UserCommandGetWorldToCanvasPoint(CameraType cameraType, Vector3 worldPos, RectTransform rectTransform)
        {
            var camera = cameraType switch
            {
                CameraType.Camera3d => camera3d,
                CameraType.CameraAmbient => cameraAmbient,
                _ => throw new ArgumentException(),
            };

            var screenPoint = camera.WorldToScreenPoint(worldPos);
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
        }

        static Vector3 GetTargetAmbientPosition(IPositionData trackingTarget)
        {
            if (trackingTarget == null)
            {
                return Vector3.zero;
            }

            if (trackingTarget.AreaId.HasValue)
            {
                return MessageBus.Instance.UtilGetAreaData.Unicast(trackingTarget.AreaId.Value).StarSystemPosition;
            }

            return trackingTarget.Position;
        }

        static Quaternion GetTargetRotation(UserData userData)
        {
            return userData.LookAtSpace
                   * Quaternion.AngleAxis(userData.LookAtAngle.y, Vector3.up)
                   * Quaternion.AngleAxis(userData.LookAtAngle.x, Vector3.right);
        }
    }
}
