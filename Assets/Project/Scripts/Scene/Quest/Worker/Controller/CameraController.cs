using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera cameraAmbient;
        [SerializeField] Camera camera3d;
        [SerializeField] Transform camera3dAnchor;

        [SerializeField] Camera cameraUi;

        QuestData questData;

        Vector2 angle;
        Quaternion rotation = Quaternion.identity;
        
        public enum CameraType
        {
            CameraAmbient,
            Camera3d,
        }

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.UserCommandRotateCamera.AddListener(UserCommandRotateCamera);
            MessageBus.Instance.UserCommandSetAmbientCameraPosition.AddListener(UserCommandSetAmbientCameraPosition);
            MessageBus.Instance.UserCommandGetWorldToCanvasPoint.AddListener(UserCommandGetWorldToCanvasPoint);
        }

        public void Finalize()
        {
            MessageBus.Instance.UserCommandRotateCamera.RemoveListener(UserCommandRotateCamera);
            MessageBus.Instance.UserCommandSetAmbientCameraPosition.RemoveListener(UserCommandSetAmbientCameraPosition);
            MessageBus.Instance.UserCommandGetWorldToCanvasPoint.RemoveListener(UserCommandGetWorldToCanvasPoint);
        }
        
        void UserCommandRotateCamera(Vector2 delta)
        {
            angle.x = angle.x + delta.x;
            angle.y = Mathf.Clamp(angle.y + delta.y, -90, 90);
            
            rotation = Quaternion.AngleAxis(angle.x, Vector3.up) * Quaternion.AngleAxis(angle.y, Vector3.right);
            
            cameraAmbient.transform.rotation = rotation;
            camera3dAnchor.rotation = rotation;
            MessageBus.Instance.UserCommandSetCameraAngle.Broadcast(rotation);
        }

        void UserCommandSetAmbientCameraPosition(Vector3 position)
        {
            cameraAmbient.transform.position = position;
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
