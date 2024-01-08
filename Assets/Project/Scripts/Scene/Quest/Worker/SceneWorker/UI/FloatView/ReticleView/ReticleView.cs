using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace.UI
{
    public class ReticleView : MonoBehaviour
    {
        [SerializeField] GameObject flightInstruments;
        [SerializeField] RectTransform artificialHorizon;
        
        [SerializeField] GameObject weaponInstruments;
        [SerializeField] RectTransform weaponBaseReticle;
        [SerializeField] RectTransform bulletReticle;
        [SerializeField] RectTransform rocketReticle;
        [SerializeField] RectTransform missileReticle;
        
        [SerializeField] GameObject commonInstruments;
        [SerializeField] RectTransform dotReticle;
        
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.AddListener(UserCommandSetActorOperationMode);
        }

        public void Finalize()
        {
            MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.RemoveListener(UserCommandSetActorOperationMode);
        }

        public void OnUpdate()
        {
            switch (questData.UserData.ActorOperationMode)
            {
                case ActorOperationMode.Observe:
                    ObserveUpdate();
                    break;
                case ActorOperationMode.ObserveFreeCamera:
                    ObserveFreeCamera();
                    break;
                case ActorOperationMode.Cockpit:
                    CockpitUpdate();
                    break;
                case ActorOperationMode.CockpitFreeCamera:
                    CockpitFreeCameraUpdate();
                    break;
                case ActorOperationMode.Spotter:
                    SpotterUpdate();
                    break;
                case ActorOperationMode.SpotterFreeCamera:
                    SpotterFreeCameraUpdate();
                    break;
            }
        }

        void ObserveUpdate()
        {
        }

        void ObserveFreeCamera()
        {
        }

        void CockpitUpdate()
        {
            var cameraRotation = MessageBus.Instance.UserInput.UserCommandGetCameraRotation.Unicast(CameraType.Near3DCamera);
            var controlActorRotation = questData.UserData.ControlActorData.Rotation;
            var diffRotation = Quaternion.Inverse(cameraRotation) * controlActorRotation;
            diffRotation.ToAngleAxis(out var angle, out var axis);

            var fov = MessageBus.Instance.UserInput.UserCommandGetCameraFieldOfView.Unicast(CameraType.Near3DCamera);
            var screenScale = (360 / (fov != 0 ? fov : 60)) * 2;
            
            if (angle > 180)
            {
                // angleは360を超えないので-180~180の範囲になる
                angle = 360 - angle;
                axis *= -1.0f;
            }

            var screenPosition = new Vector3(axis.y * angle * screenScale, axis.x * angle * -1 * screenScale, 0);
            var screenAngle = Quaternion.AngleAxis(axis.z * angle, Vector3.forward);
            artificialHorizon.localPosition = screenPosition;
            artificialHorizon.localRotation = screenAngle;
            
            weaponBaseReticle.localPosition = screenPosition;
            weaponBaseReticle.localScale = Vector3.one * (60.0f / fov);
        }

        void CockpitFreeCameraUpdate()
        {
            var fov = MessageBus.Instance.UserInput.UserCommandGetCameraFieldOfView.Unicast(CameraType.Near3DCamera);
            
            weaponBaseReticle.localScale = Vector3.one * (60.0f / fov);
        }

        void SpotterUpdate()
        {
            var fov = MessageBus.Instance.UserInput.UserCommandGetCameraFieldOfView.Unicast(CameraType.Near3DCamera);
            
            weaponBaseReticle.localScale = Vector3.one * (60.0f / fov);
        }

        void SpotterFreeCameraUpdate()
        {
            var fov = MessageBus.Instance.UserInput.UserCommandGetCameraFieldOfView.Unicast(CameraType.Near3DCamera);
            
            weaponBaseReticle.localScale = Vector3.one * (60.0f / fov);
        }

        void UserCommandSetActorOperationMode(ActorOperationMode actorOperationMode)
        {
            flightInstruments.gameObject.SetActive(actorOperationMode == ActorOperationMode.Cockpit);
            weaponInstruments.gameObject.SetActive(actorOperationMode != ActorOperationMode.Observe && actorOperationMode != ActorOperationMode.ObserveFreeCamera);
            commonInstruments.gameObject.SetActive(actorOperationMode == ActorOperationMode.Observe || actorOperationMode == ActorOperationMode.ObserveFreeCamera);
            ResetPositions();
        }
        
        void ResetPositions()
        {
            artificialHorizon.localPosition = Vector3.zero;
            artificialHorizon.localRotation = Quaternion.identity;
            
            weaponBaseReticle.localPosition = Vector3.zero;
            bulletReticle.localPosition = Vector3.zero;
            rocketReticle.localPosition = Vector3.zero;
            missileReticle.localPosition = Vector3.zero;
            
            dotReticle.localPosition = Vector3.zero;
        }
    }
}
