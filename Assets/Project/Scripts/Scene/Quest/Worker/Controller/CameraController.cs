using System;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera mainCamera;
        [SerializeField] Transform cameraAnchor;

        CameraMode cameraMode;
        Transform focusObject;
        Actor[] actors;

        QuestData questData;
        Vector2 angle;
        Quaternion rotation = Quaternion.identity;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.UserCommandSetCameraMode.AddListener(UserCommandSetCameraMode);
            MessageBus.Instance.UserCommandSetCameraFocusObject.AddListener(UserCommandSetCameraFocusObject);
            MessageBus.Instance.UserCommandRotateCamera.AddListener(UserCommandRotateCamera);
            
            MessageBus.Instance.UserCommandSetObserveActor.AddListener(UserCommandSetObserveActor);
            MessageBus.Instance.SubscribeUpdateActorList.AddListener(SubscribeUpdateActorList);
        }

        public void Finalize()
        {
            MessageBus.Instance.UserCommandSetCameraMode.RemoveListener(UserCommandSetCameraMode);
            MessageBus.Instance.UserCommandSetCameraFocusObject.RemoveListener(UserCommandSetCameraFocusObject);
            MessageBus.Instance.UserCommandRotateCamera.RemoveListener(UserCommandRotateCamera);
            
            MessageBus.Instance.UserCommandSetObserveActor.RemoveListener(UserCommandSetObserveActor);
            MessageBus.Instance.SubscribeUpdateActorList.RemoveListener(SubscribeUpdateActorList);
        }

        void Update()
        {
            switch (cameraMode)
            {
                case CameraMode.FocusObject:
                    FocusObject();
                    break;
                default:
                    FocusObject();
                    break;
            }
        }

        void UserCommandSetCameraMode(CameraMode cameraMode)
        {
            this.cameraMode = cameraMode;
            focusObject = null;
        }

        void UserCommandSetCameraFocusObject(Transform focusObject)
        {
            this.focusObject = focusObject;
        }

        void UserCommandSetObserveActor(Guid observeActorId)
        {
            var actor = actors?.FirstOrDefault(x => x.InstanceId == observeActorId);
            if (actor != null)
            {
                MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraMode.FocusObject);
                MessageBus.Instance.UserCommandSetCameraFocusObject.Broadcast(actor.transform);
            }
        }
        
        void SubscribeUpdateActorList(Actor[] actors)
        {
            this.actors = actors;
        }

        void FocusObject()
        {
            if (focusObject == null)
            {
                return;
            }

            mainCamera.transform.position += (focusObject.position - mainCamera.transform.position) * 0.1f;
        }
        
        void UserCommandRotateCamera(Vector2 delta)
        {
            angle.x = angle.x + delta.x;
            angle.y = Mathf.Clamp(angle.y + delta.y, -90, 90);
            
            rotation = Quaternion.AngleAxis(angle.x, Vector3.up) * Quaternion.AngleAxis(angle.y, Vector3.right);
            
            cameraAnchor.rotation = rotation;
            MessageBus.Instance.UserCommandSetCameraAngle.Broadcast(rotation);
        }
    }
}
