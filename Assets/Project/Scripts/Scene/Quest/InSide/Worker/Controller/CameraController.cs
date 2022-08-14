using System;
using System.Linq;
using UnityEngine;

namespace RoboQuest.Quest.InSide
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera mainCamera;
        [SerializeField] Vector3 offset;
        [SerializeField] float distanceScale;

        CameraMode cameraMode;
        Transform focusObject;
        Actor[] actors;

        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.UserCommandSetCameraMode.AddListener(UserCommandSetCameraMode);
            MessageBus.Instance.UserCommandSetCameraFocusObject.AddListener(UserCommandSetCameraFocusObject);
            
            MessageBus.Instance.UserCommandSetObserveActor.AddListener(UserCommandSetObserveActor);
            MessageBus.Instance.SubscribeUpdateActorList.AddListener(SubscribeUpdateActorList);
        }

        public void Finalize()
        {
            MessageBus.Instance.UserCommandSetCameraMode.RemoveListener(UserCommandSetCameraMode);
            MessageBus.Instance.UserCommandSetCameraFocusObject.RemoveListener(UserCommandSetCameraFocusObject);
            
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
            var actor = actors.FirstOrDefault(x => x.InstanceId == observeActorId);
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

            var targetPosition = focusObject.position + offset + new Vector3(0, 1, -1) * distanceScale;
            mainCamera.transform.position += (targetPosition - mainCamera.transform.position) * 0.1f;
        }
    }
}
