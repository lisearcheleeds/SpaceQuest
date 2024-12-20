﻿using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace AloneSpace
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera ambientCamera;
        
        [SerializeField] Camera farCamera;
        [SerializeField] Camera nearCamera;
        
        [SerializeField] Camera spaceMapCamera;
        
        [SerializeField] Camera radarCamera;

        [SerializeField] Camera uiCamera;

        Vector3 currentAmbientPosition = Vector3.zero;
        Vector3 currentTargetPosition = Vector3.zero;
        Quaternion currentTargetRotation = Quaternion.identity;
        Vector3 currentCameraPosition = Vector3.zero;
        float currentFoV = 60.0f;

        IPositionData trackingTarget;
        
        QuestData questData;

        CameraGroupType cameraGroupType = CameraGroupType.Space;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.UserInput.UserCommandSetCameraTrackTarget.AddListener(UserCommandSetCameraTrackTarget);
            MessageBus.Instance.UserInput.UserCommandSetCameraGroupType.AddListener(UserCommandSetCameraGroupType);
            
            MessageBus.Instance.Util.GetWorldToCanvasPoint.SetListener(UserCommandGetWorldToCanvasPoint);
            MessageBus.Instance.Util.GetCameraRotation.SetListener(GetCameraRotation);
            MessageBus.Instance.Util.GetCameraFieldOfView.SetListener(GetCameraFieldOfView);
        }

        public void Finalize()
        {
            MessageBus.Instance.UserInput.UserCommandSetCameraTrackTarget.RemoveListener(UserCommandSetCameraTrackTarget);
            MessageBus.Instance.UserInput.UserCommandSetCameraGroupType.RemoveListener(UserCommandSetCameraGroupType);
            
            MessageBus.Instance.Util.GetWorldToCanvasPoint.SetListener(null);
            MessageBus.Instance.Util.GetCameraRotation.SetListener(null);
            MessageBus.Instance.Util.GetCameraFieldOfView.SetListener(null);
        }

        public void OnUpdate()
        {
            switch (cameraGroupType)
            {
                case CameraGroupType.Space:
                    UpdateSpace();
                    break;
                case CameraGroupType.SpaceMap:
                    UpdateSpaceMap();
                    break;
            }
        }

        void UpdateSpace()
        {
            var targetPosition = trackingTarget?.Position ?? Vector3.zero;
            var targetAmbientPosition = GetTargetAmbientPosition(trackingTarget);
            var lookAtRotation = GetLookAtRotation(questData.UserData.LookAtSpace, questData.UserData.LookAtAngle);

            currentTargetRotation = Quaternion.Lerp(currentTargetRotation, lookAtRotation, 0.4f);
            ambientCamera.transform.rotation = currentTargetRotation;
            farCamera.transform.rotation = currentTargetRotation;
            nearCamera.transform.rotation = currentTargetRotation;
            
            radarCamera.transform.rotation = currentTargetRotation;

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
            
            radarCamera.transform.position = currentTargetRotation * new Vector3(0, 0, -4.0f);
        }

        void UpdateSpaceMap()
        {
            var targetAmbientPosition = GetTargetAmbientPosition(trackingTarget);
            var spaceMapLookAtRotation = GetLookAtRotation(Quaternion.identity, questData.UserData.SpaceMapLookAtAngle);

            ambientCamera.transform.rotation = spaceMapLookAtRotation;
            ambientCamera.transform.position = targetAmbientPosition;
                
            spaceMapCamera.transform.rotation = spaceMapLookAtRotation;
            spaceMapCamera.transform.position = Vector3.zero + spaceMapLookAtRotation * new Vector3(0, 0, -1000.0f - questData.UserData.SpaceMapLookAtDistance * 10.0f);
        }

        void UserCommandSetCameraTrackTarget(IPositionData cameraTrackTarget)
        {
            trackingTarget = cameraTrackTarget;
        }
        
        void UserCommandSetCameraGroupType(CameraGroupType cameraGroupType)
        {
            if (this.cameraGroupType == cameraGroupType)
            {
                return;
            }

            this.cameraGroupType = cameraGroupType;
            
            var afterCameras = GetCameras(cameraGroupType);
            var cameraData = ambientCamera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Clear();
            cameraData.cameraStack.AddRange(afterCameras);

            Camera[] GetCameras(CameraGroupType group)
            {
                return group switch
                {
                    CameraGroupType.Space => new[] { farCamera, nearCamera, uiCamera },
                    CameraGroupType.SpaceMap => new[] { spaceMapCamera, uiCamera },
                    _ => throw new ArgumentException(),
                };
            }
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
                uiCamera,
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
        
        static Quaternion GetLookAtRotation(Quaternion lookAtSpace, Vector3 lookAtAngle)
        {
            return lookAtSpace
                   * Quaternion.AngleAxis(lookAtAngle.y, Vector3.up)
                   * Quaternion.AngleAxis(lookAtAngle.x, Vector3.right);
        }

        static float GetFieldOfView(float currentFoV, ActorOperationMode actorOperationMode)
        {
            var targetFoV = actorOperationMode switch
            {
                ActorOperationMode.ObserverMode => 65.0f,
                ActorOperationMode.LockOnMode => 65.0f,
                _ => 60.0f,
            };

            return Mathf.Lerp(currentFoV, targetFoV, 0.2f);
        }

        static Vector3 GetLerpedCurrentPosition(Vector3 currentPosition, Vector3 targetPosition, ActorOperationMode actorOperationMode)
        {
            var lerpRatio = actorOperationMode switch
            {
                ActorOperationMode.ObserverMode => 1.0f,
                _ => 0.1f,
            };
            
            return Vector3.Lerp(currentPosition, targetPosition, lerpRatio);
        }

        static Vector3 GetCameraOffsetPosition(Vector3 currentTargetPosition, Quaternion currentTargetRotation, Vector3 currentCameraPosition, UserData userData)
        {
            var lookAtDistance = Mathf.Abs(userData.LookAtDistance);
            var offset = userData.ActorOperationMode switch
            {
                ActorOperationMode.ObserverMode => new Vector3(0, 0, -lookAtDistance + lookAtDistance * -4.0f),
                _ => new Vector3(0, lookAtDistance, lookAtDistance * -4.0f),
            };
            
            var target = currentTargetPosition + currentTargetRotation * offset;

            var lerpRatio = userData.ActorOperationMode switch
            {
                ActorOperationMode.ObserverMode => 0.3f,
                _ => 1.0f,
            };
            return Vector3.Lerp(currentCameraPosition, target, lerpRatio);
        }
    }
}
