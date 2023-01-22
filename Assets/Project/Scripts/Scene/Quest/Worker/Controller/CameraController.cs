using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class CameraController : MonoBehaviour
    {
        static readonly float CameraModeSwitchTime = 1.0f;
        
        [SerializeField] Camera cameraAmbient;
        [SerializeField] Camera camera3d;

        [SerializeField] Camera cameraUi;

        QuestData questData;

        CameraMode beforeCameraMode = CameraMode.Default;
        CameraMode currentCameraMode = CameraMode.Default;
        float cameraModeSwitchTime;

        Vector2 targetAngle;
        Quaternion targetQuaternion = Quaternion.identity;
        Quaternion currentQuaternion = Quaternion.identity;

        IPositionData trackingTarget;
        
        public enum CameraMode
        {
            Default,
            Map,
        }
        
        public enum CameraType
        {
            CameraAmbient,
            Camera3d,
        }

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.UserCommandSetCameraMode.AddListener(UserCommandSetCameraMode);

            MessageBus.Instance.UserCommandRotateCamera.AddListener(UserCommandRotateCamera);
            
            MessageBus.Instance.UserCommandSetCameraTrackTarget.AddListener(UserCommandSetCameraTrackTarget);
            MessageBus.Instance.UserCommandGetWorldToCanvasPoint.AddListener(UserCommandGetWorldToCanvasPoint);
        }

        public void Finalize()
        {
            MessageBus.Instance.UserCommandSetCameraMode.RemoveListener(UserCommandSetCameraMode);
            
            MessageBus.Instance.UserCommandRotateCamera.RemoveListener(UserCommandRotateCamera);
            
            MessageBus.Instance.UserCommandSetCameraTrackTarget.RemoveListener(UserCommandSetCameraTrackTarget);
            MessageBus.Instance.UserCommandGetWorldToCanvasPoint.RemoveListener(UserCommandGetWorldToCanvasPoint);
        }

        public void OnLateUpdate()
        {
            var target3dCameraPosition = Vector3.zero;
            var targetAmbientCameraPosition = Vector3.zero;
            if (trackingTarget != null)
            {
                target3dCameraPosition = trackingTarget.Position;
                if (trackingTarget.AreaId.HasValue)
                {
                    MessageBus.Instance.UtilGetAreaData.Broadcast(
                        trackingTarget.AreaId.Value,
                        areaData => targetAmbientCameraPosition = areaData.StarSystemPosition);
                }
                else
                {
                    targetAmbientCameraPosition = trackingTarget.Position;
                }
            }
         
            var cameraModeLerpRatio = Mathf.Clamp01((Time.time - cameraModeSwitchTime) / CameraModeSwitchTime);
            
            currentQuaternion = Quaternion.Lerp(GetCameraAngleQuaternion(beforeCameraMode), GetCameraAngleQuaternion(currentCameraMode), cameraModeLerpRatio);
            cameraAmbient.transform.rotation = currentQuaternion;
            camera3d.transform.rotation = currentQuaternion;
            
            cameraAmbient.transform.position = Vector3.Lerp(GetAmbientCameraPosition(beforeCameraMode), GetAmbientCameraPosition(currentCameraMode), cameraModeLerpRatio);
            camera3d.transform.position = Vector3.Lerp(Get3dCameraPosition(beforeCameraMode), Get3dCameraPosition(currentCameraMode), cameraModeLerpRatio);

            MessageBus.Instance.UserCommandSetCameraAngle.Broadcast(currentQuaternion);
            
            Quaternion GetCameraAngleQuaternion(CameraMode cameraMode)
            {
                return cameraMode switch
                {
                    CameraMode.Default => Quaternion.Lerp(currentQuaternion, targetQuaternion, 0.05f),
                    CameraMode.Map => Quaternion.Lerp(currentQuaternion, Quaternion.AngleAxis(55, Vector3.right), 0.4f),
                };
            }
            
            Vector3 GetAmbientCameraPosition(CameraMode cameraMode)
            {
                return cameraMode switch
                {
                    CameraMode.Default => Vector3.Lerp(cameraAmbient.transform.position, targetAmbientCameraPosition, 0.05f),
                    CameraMode.Map => Vector3.Lerp(cameraAmbient.transform.position, new Vector3(0, 250, -200), 0.05f),
                };
            }
            
            Vector3 Get3dCameraPosition(CameraMode cameraMode)
            {
                return cameraMode switch
                {
                    CameraMode.Default => target3dCameraPosition + currentQuaternion * new Vector3(0, 0, -35f),
                    CameraMode.Map => -targetAmbientCameraPosition * 100.0f,
                };
            }
        }

        void UserCommandSetCameraMode(CameraMode mode)
        {
            beforeCameraMode = currentCameraMode;
            currentCameraMode = mode;

            cameraModeSwitchTime = Time.time;
        }

        void UserCommandRotateCamera(Vector2 delta)
        {
            targetAngle.x = targetAngle.x + delta.x;
            targetAngle.y = Mathf.Clamp(targetAngle.y + delta.y, -90, 90);
            
            targetQuaternion = Quaternion.AngleAxis(targetAngle.x, Vector3.up) * Quaternion.AngleAxis(targetAngle.y, Vector3.right);
        }

        void UserCommandSetCameraTrackTarget(IPositionData cameraTrackTarget)
        {
            trackingTarget = cameraTrackTarget;
        }

        void UserCommandGetWorldToCanvasPoint(CameraType cameraType, Vector3 worldPos, RectTransform rectTransform, Action<Vector3?> callback)
        {
            var camera = cameraType switch
            {
                CameraType.Camera3d => camera3d,
                CameraType.CameraAmbient => cameraAmbient,
                _ => null,
            };

            var screenPoint = camera.WorldToScreenPoint(worldPos);
            if (screenPoint.z < 0)
            {
                callback(null);
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                screenPoint,
                cameraUi,
                out var localPoint);

            callback(localPoint);
        }
    }
}
