using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace AloneSpace
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera ambientCamera;
        [SerializeField] Camera farCamera;
        [SerializeField] Camera nearCamera;
        
        [SerializeField] Camera cameraUIRadarView;

        [SerializeField] Camera cameraUi;

        Vector3 currentAmbientPosition = Vector3.zero;
        Vector3 currentTargetPosition = Vector3.zero;
        Quaternion currentTargetRotation = Quaternion.identity;
        Vector3 currentCameraPosition = Vector3.zero;
        float currentFoV = 60.0f;

        IPositionData trackingTarget;
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.UserInput.UserCommandSetCameraTrackTarget.AddListener(UserCommandSetCameraTrackTarget);
            MessageBus.Instance.Util.GetWorldToCanvasPoint.SetListener(UserCommandGetWorldToCanvasPoint);
            MessageBus.Instance.Util.GetCameraRotation.SetListener(GetCameraRotation);
            MessageBus.Instance.Util.GetCameraFieldOfView.SetListener(GetCameraFieldOfView);
        }

        public void Finalize()
        {
            MessageBus.Instance.UserInput.UserCommandSetCameraTrackTarget.RemoveListener(UserCommandSetCameraTrackTarget);
            MessageBus.Instance.Util.GetWorldToCanvasPoint.SetListener(null);
            MessageBus.Instance.Util.GetCameraRotation.SetListener(null);
            MessageBus.Instance.Util.GetCameraFieldOfView.SetListener(null);
        }

        public void OnUpdate()
        {
            var targetPosition = trackingTarget?.Position ?? Vector3.zero;
            var targetAmbientPosition = GetTargetAmbientPosition(trackingTarget);
            var lookAtRotation = GetLookAtRotation(questData.UserData);

            currentTargetRotation = Quaternion.Lerp(currentTargetRotation, lookAtRotation, 0.4f);
            ambientCamera.transform.rotation = currentTargetRotation;
            farCamera.transform.rotation = currentTargetRotation;
            nearCamera.transform.rotation = currentTargetRotation;
            
            cameraUIRadarView.transform.rotation = currentTargetRotation;

            currentAmbientPosition = Vector3.Lerp(currentAmbientPosition, targetAmbientPosition, 0.05f);
            ambientCamera.transform.position = currentAmbientPosition;

            currentFoV = GetFieldOfView(currentFoV, questData.UserData.ActorOperationMode);
            ambientCamera.fieldOfView = currentFoV;
            farCamera.fieldOfView = currentFoV;
            nearCamera.fieldOfView = currentFoV;
            
            currentTargetPosition = GetLerpedCurrentPosition(currentTargetPosition, targetPosition, questData.UserData.ActorOperationMode);
            
            currentCameraPosition = GetCameraOffsetPosition(currentTargetPosition, currentTargetRotation, currentCameraPosition, questData.UserData);
            farCamera.transform.position = currentCameraPosition;
            nearCamera.transform.position = currentCameraPosition;
            
            cameraUIRadarView.transform.position = currentTargetRotation * new Vector3(0, 0, -4.0f);
        }

        void UserCommandSetCameraTrackTarget(IPositionData cameraTrackTarget)
        {
            trackingTarget = cameraTrackTarget;
        }

        Vector3? UserCommandGetWorldToCanvasPoint(CameraType cameraType, Vector3 worldPos, RectTransform rectTransform)
        {
            var camera = cameraType switch
            {
                CameraType.NearCamera => nearCamera,
                CameraType.FarCamera => farCamera,
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
                CameraType.NearCamera => nearCamera.transform.rotation,
                CameraType.FarCamera => farCamera.transform.rotation,
                CameraType.AmbientCamera => ambientCamera.transform.rotation,
                _ => throw new ArgumentException(),
            };
        }
        
        float GetCameraFieldOfView(CameraType cameraType)
        {
            return cameraType switch
            {
                CameraType.NearCamera => nearCamera.fieldOfView,
                CameraType.FarCamera => farCamera.fieldOfView,
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
                return MessageBus.Instance.Util.GetAreaData.Unicast(trackingTarget.AreaId.Value).StarSystemPosition;
            }

            return trackingTarget.Position;
        }

        static Quaternion GetLookAtRotation(UserData userData)
        {
            return userData.LookAtSpace
                   * Quaternion.AngleAxis(userData.LookAtAngle.y, Vector3.up)
                   * Quaternion.AngleAxis(userData.LookAtAngle.x, Vector3.right);
        }

        static float GetFieldOfView(float currentFoV, ActorOperationMode actorOperationMode)
        {
            var targetFoV = actorOperationMode switch
            {
                ActorOperationMode.Observe => 65.0f,
                ActorOperationMode.ObserveFreeCamera => 65.0f,
                ActorOperationMode.Cockpit => 60.0f,
                ActorOperationMode.CockpitFreeCamera => 65.0f,
                ActorOperationMode.Spotter => 60.0f,
                ActorOperationMode.SpotterFreeCamera => 65.0f,
                _ => 60.0f,
            };

            return Mathf.Lerp(currentFoV, targetFoV, 0.2f);
        }

        static Vector3 GetLerpedCurrentPosition(Vector3 currentPosition, Vector3 targetPosition, ActorOperationMode actorOperationMode)
        {
            var lerpRatio = actorOperationMode switch
            {
                ActorOperationMode.Observe => 1.0f,
                ActorOperationMode.ObserveFreeCamera => 1.0f,
                _ => 0.1f,
            };
            
            return Vector3.Lerp(currentPosition, targetPosition, lerpRatio);
        }

        static Vector3 GetCameraOffsetPosition(Vector3 currentTargetPosition, Quaternion currentTargetRotation, Vector3 currentCameraPosition, UserData userData)
        {
            var lookAtDistance = Mathf.Abs(userData.LookAtDistance);
            var offset = userData.ActorOperationMode switch
            {
                ActorOperationMode.Observe => new Vector3(0, 0, -lookAtDistance + lookAtDistance * -4.0f),
                ActorOperationMode.ObserveFreeCamera => new Vector3(0, 0, -lookAtDistance + lookAtDistance * -4.0f),
                _ => new Vector3(0, lookAtDistance, lookAtDistance * -4.0f),
            };
            
            var target = currentTargetPosition + currentTargetRotation * offset;

            var lerpRatio = userData.ActorOperationMode switch
            {
                ActorOperationMode.Observe => 0.3f,
                ActorOperationMode.ObserveFreeCamera => 1.0f,
                _ => 1.0f,
            };
            return Vector3.Lerp(currentCameraPosition, target, lerpRatio);
        }
    }
}
