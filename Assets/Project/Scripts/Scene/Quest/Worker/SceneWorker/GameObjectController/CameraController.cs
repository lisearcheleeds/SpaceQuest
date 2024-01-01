using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AloneSpace
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera ambientCamera;
        [SerializeField] Camera far3DCamera;
        [SerializeField] Camera near3DCamera;
        
        [SerializeField] Camera cameraUIRadarView;

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
            MessageBus.Instance.UserCommandGetCameraRotation.SetListener(GetCameraRotation);
            MessageBus.Instance.UserCommandGetCameraFieldOfView.SetListener(GetCameraFieldOfView);
        }

        public void Finalize()
        {
            MessageBus.Instance.UserCommandSetCameraTrackTarget.RemoveListener(UserCommandSetCameraTrackTarget);
            MessageBus.Instance.UserCommandGetWorldToCanvasPoint.SetListener(null);
            MessageBus.Instance.UserCommandGetCameraRotation.SetListener(null);
            MessageBus.Instance.UserCommandGetCameraFieldOfView.SetListener(null);
        }

        public void OnUpdate()
        {
            var targetPosition = trackingTarget?.Position ?? Vector3.zero;
            var targetAmbientPosition = GetTargetAmbientPosition(trackingTarget);
            var lookAtRotation = GetLookAtRotation(questData.UserData);

            currentRotation = Quaternion.Lerp(currentRotation, lookAtRotation, 0.4f);
            ambientCamera.transform.rotation = currentRotation;
            far3DCamera.transform.rotation = currentRotation;
            near3DCamera.transform.rotation = currentRotation;
            
            cameraUIRadarView.transform.rotation = currentRotation;

            currentAmbientPosition = Vector3.Lerp(currentAmbientPosition, targetAmbientPosition, 0.05f);
            ambientCamera.transform.position = currentAmbientPosition;

            currentPosition = Vector3.Lerp(currentPosition, targetPosition, 0.1f);
            var lookAtDistance = Mathf.Abs(questData.UserData.LookAtDistance);
            var cameraPosition = currentPosition + currentRotation * new Vector3(0, lookAtDistance, lookAtDistance * -4.0f);
            far3DCamera.transform.position = cameraPosition;
            near3DCamera.transform.position = cameraPosition;
            
            cameraUIRadarView.transform.position = currentRotation * new Vector3(0, 0, -4.0f);
        }

        void UserCommandSetCameraTrackTarget(IPositionData cameraTrackTarget)
        {
            trackingTarget = cameraTrackTarget;
        }

        Vector3? UserCommandGetWorldToCanvasPoint(CameraType cameraType, Vector3 worldPos, RectTransform rectTransform)
        {
            var camera = cameraType switch
            {
                CameraType.Near3DCamera => near3DCamera,
                CameraType.Far3DCamera => far3DCamera,
                CameraType.AmbientCamera => ambientCamera,
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
        
        Quaternion GetCameraRotation(CameraType cameraType)
        {
            return cameraType switch
            {
                CameraType.Near3DCamera => near3DCamera.transform.rotation,
                CameraType.Far3DCamera => far3DCamera.transform.rotation,
                CameraType.AmbientCamera => ambientCamera.transform.rotation,
                _ => throw new ArgumentException(),
            };
        }
        
        float GetCameraFieldOfView(CameraType cameraType)
        {
            return cameraType switch
            {
                CameraType.Near3DCamera => near3DCamera.fieldOfView,
                CameraType.Far3DCamera => far3DCamera.fieldOfView,
                CameraType.AmbientCamera => ambientCamera.fieldOfView,
                _ => throw new ArgumentException(),
            };
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

        static Quaternion GetLookAtRotation(UserData userData)
        {
            return userData.LookAtSpace
                   * Quaternion.AngleAxis(userData.LookAtAngle.y, Vector3.up)
                   * Quaternion.AngleAxis(userData.LookAtAngle.x, Vector3.right);
        }
    }
}
