using System;
using System.Collections.Generic;
using System.Linq;
using AloneSpace.Common;
using UnityEngine;

namespace AloneSpace.UI
{
    public class ReticleView : MonoBehaviour
    {
        [SerializeField] GameObject flightInstruments;
        [SerializeField] RectTransform artificialHorizon;
        
        [SerializeField] GameObject weaponInstruments;
        [SerializeField] RectTransform weaponBaseReticle;
        
        [Header("Bullet")]
        [SerializeField] RectTransform bulletBase;
        [SerializeField] Circle2D bulletAngle;
        [SerializeField] RectTransform bulletReticle;
        [SerializeField] Circle2D bulletResourceGauge;
        
        [Header("Rocket")]
        [SerializeField] RectTransform rocketBase;
        [SerializeField] RectTransform rocketReticle;
        [SerializeField] Circle2D rocketResourceGauge;
        
        [Header("Missile")]
        [SerializeField] RectTransform missileBase;
        [SerializeField] RectTransform missileReticle;
        [SerializeField] Circle2D missileAngle;
        [SerializeField] Circle2D missileResourceGauge;
        
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
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }
            
            switch (questData.UserData.ActorOperationMode)
            {
                case ActorOperationMode.Observe:
                    break;
                case ActorOperationMode.ObserveFreeCamera:
                    break;
                case ActorOperationMode.Cockpit:
                    WeaponBaseReticleUpdate(true);
                    BulletReticleUpdate();
                    RocketReticleUpdate();
                    MissileReticleUpdate();
                    break;
                case ActorOperationMode.CockpitFreeCamera:
                    WeaponBaseReticleUpdate(false);
                    break;
                case ActorOperationMode.Spotter:
                    WeaponBaseReticleUpdate(false);
                    BulletReticleUpdate();
                    RocketReticleUpdate();
                    MissileReticleUpdate();
                    break;
                case ActorOperationMode.SpotterFreeCamera:
                    WeaponBaseReticleUpdate(false);
                    break;
            }
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

        void WeaponBaseReticleUpdate(bool withArtificialHorizon)
        {
            var fov = MessageBus.Instance.Util.GetCameraFieldOfView.Unicast(CameraType.NearCamera);
            if (fov == 0)
            {
                return;
            }

            if (withArtificialHorizon)
            {
                var cameraRotation = MessageBus.Instance.Util.GetCameraRotation.Unicast(CameraType.NearCamera);
                var controlActorData = questData.UserData.ControlActorData;
                var controlActorRotation = controlActorData.Rotation;
                var diffRotation = Quaternion.Inverse(cameraRotation) * controlActorRotation;
                diffRotation.ToAngleAxis(out var angle, out var axis);

                var screenScale = (360 / fov) * 2;
            
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
            }
        }

        void BulletReticleUpdate()
        {
            var controlActorData = questData.UserData.ControlActorData;
            var currentWeaponDataGroup = controlActorData.WeaponDataGroup[controlActorData.ActorStateData.CurrentWeaponGroupIndex];
            
            var currentBulletMaker = currentWeaponDataGroup
                .Select(x => controlActorData.WeaponData[x])
                .OfType<BulletMakerWeaponData>()
                .FirstOrDefault();
            if (currentBulletMaker != null)
            {
                bulletBase.gameObject.SetActive(true);
                
                if (currentBulletMaker.BulletMakerWeaponStateData.TargetData != null && currentBulletMaker.BulletMakerWeaponStateData.IsTargetInAngle)
                {
                    var canvasPoint = MessageBus.Instance.Util.GetWorldToCanvasPoint.Unicast(
                        CameraType.NearCamera,
                        currentBulletMaker.BulletMakerWeaponStateData.TargetData.Position,
                        (RectTransform)bulletReticle.parent);

                    if (canvasPoint.HasValue)
                    {
                        bulletReticle.localPosition = (bulletReticle.localPosition + canvasPoint.Value) * 0.5f;
                    }
                    else
                    {
                        bulletReticle.localPosition = (bulletReticle.localPosition + Vector3.zero) * 0.5f;
                    }
                }
                else
                {
                    bulletReticle.localPosition = (bulletReticle.localPosition + Vector3.zero) * 0.5f;
                }

                var fov = MessageBus.Instance.Util.GetCameraFieldOfView.Unicast(CameraType.NearCamera);
                if (fov != 0)
                {
                    var reticleSize = Screen.height * currentBulletMaker.VO.AngleOfFire / fov;
                    bulletAngle.Apply(
                        reticleSize, 
                        bulletAngle.Division, 
                        bulletAngle.FillOrigin, 
                        bulletAngle.FillAmount, 
                        bulletAngle.IsPierced, 
                        reticleSize - 4.0f);
                }

                return;
            }
            
            var currentParticleBulletMaker = currentWeaponDataGroup
                .Select(x => controlActorData.WeaponData[x])
                .OfType<ParticleBulletMakerWeaponData>()
                .FirstOrDefault();
            if (currentParticleBulletMaker != null)
            {
                bulletBase.gameObject.SetActive(true);
                
                if (currentParticleBulletMaker.ParticleBulletMakerWeaponStateData.TargetData != null && currentParticleBulletMaker.ParticleBulletMakerWeaponStateData.IsTargetInAngle)
                {
                    var canvasPoint = MessageBus.Instance.Util.GetWorldToCanvasPoint.Unicast(
                        CameraType.NearCamera,
                        currentParticleBulletMaker.ParticleBulletMakerWeaponStateData.TargetData.Position,
                        (RectTransform)bulletReticle.parent);

                    if (canvasPoint.HasValue)
                    {
                        bulletReticle.localPosition = (bulletReticle.localPosition + canvasPoint.Value) * 0.5f;
                    }
                    else
                    {
                        bulletReticle.localPosition = (bulletReticle.localPosition + Vector3.zero) * 0.5f;
                    }
                }
                else
                {
                    bulletReticle.localPosition = (bulletReticle.localPosition + Vector3.zero) * 0.5f;
                }

                var fov = MessageBus.Instance.Util.GetCameraFieldOfView.Unicast(CameraType.NearCamera);
                if (fov != 0)
                {
                    var reticleSize = Screen.height * (currentParticleBulletMaker.VO.AngleOfFire / fov);
                    bulletAngle.Apply(
                        reticleSize, 
                        bulletAngle.Division, 
                        bulletAngle.FillOrigin, 
                        bulletAngle.FillAmount, 
                        bulletAngle.IsPierced, 
                        reticleSize - 4.0f);
                }

                return;
            }
            
            bulletBase.gameObject.SetActive(false);
        }

        void RocketReticleUpdate()
        {
            rocketBase.gameObject.SetActive(false);
        }

        void MissileReticleUpdate()
        {
            var controlActorData = questData.UserData.ControlActorData;
            var currentWeaponDataGroup = controlActorData.WeaponDataGroup[controlActorData.ActorStateData.CurrentWeaponGroupIndex];
            
            var currentMissileMaker = currentWeaponDataGroup
                .Select(x => controlActorData.WeaponData[x])
                .OfType<MissileMakerWeaponData>()
                .FirstOrDefault();
            if (currentMissileMaker != null)
            {
                missileBase.gameObject.SetActive(true);
                
                if (currentMissileMaker.MissileMakerWeaponStateData.TargetData != null && currentMissileMaker.MissileMakerWeaponStateData.IsTargetInAngle)
                {
                    var canvasPoint = MessageBus.Instance.Util.GetWorldToCanvasPoint.Unicast(
                        CameraType.NearCamera,
                        currentMissileMaker.MissileMakerWeaponStateData.TargetData.Position,
                        (RectTransform)missileReticle.parent);

                    if (canvasPoint.HasValue)
                    {
                        missileReticle.localPosition = (missileReticle.localPosition + canvasPoint.Value) * 0.5f;
                    }
                    else
                    {
                        missileReticle.localPosition = (missileReticle.localPosition + Vector3.zero) * 0.5f;
                    }
                }
                else
                {
                    missileReticle.localPosition = (missileReticle.localPosition + Vector3.zero) * 0.5f;
                }
                
                var fov = MessageBus.Instance.Util.GetCameraFieldOfView.Unicast(CameraType.NearCamera);
                if (fov != 0)
                {
                    var reticleSize = Screen.height * (currentMissileMaker.VO.LockOnAngle / fov);
                    missileAngle.Apply(
                        reticleSize, 
                        missileAngle.Division, 
                        missileAngle.FillOrigin, 
                        missileAngle.FillAmount, 
                        missileAngle.IsPierced, 
                        reticleSize - 4.0f);
                    
                    missileResourceGauge.Apply(
                        reticleSize - 8.0f, 
                        missileResourceGauge.Division, 
                        missileResourceGauge.FillOrigin, 
                        (currentMissileMaker.VO.MagazineSize - (float)currentMissileMaker.MissileMakerWeaponStateData.ResourceIndex) / currentMissileMaker.VO.MagazineSize, 
                        missileResourceGauge.IsPierced, 
                        reticleSize - 28.0f);
                }
            }
            else
            {
                missileBase.gameObject.SetActive(false);
            }
        }
    }
}
