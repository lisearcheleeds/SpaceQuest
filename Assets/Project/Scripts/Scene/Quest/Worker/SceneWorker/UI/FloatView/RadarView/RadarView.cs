﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace.UI
{
    public class RadarView : MonoBehaviour
    {
        [SerializeField] RadarPointMarker radarPointMarkerPrefab;
        [SerializeField] RectTransform markerParent;
        [SerializeField] float distanceScale;
        
        [SerializeField] Transform center3DObject;

        List<RadarPointMarker> radarPointMarkerList = new();
        bool isDirty;
        
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.User.SetControlActor.AddListener(SetUserControlActor);
            
            MessageBus.Instance.Data.OnCreateActorData.AddListener(OnCreateActorData);
            MessageBus.Instance.Data.OnReleaseActorData.AddListener(OnReleaseActorData);
            MessageBus.Instance.Data.OnCreateInteractData.AddListener(OnCreateInteractData);
            MessageBus.Instance.Data.OnReleaseInteractData.AddListener(OnReleaseInteractData);    
        }

        public void Finalize()
        {
            MessageBus.Instance.User.SetControlActor.RemoveListener(SetUserControlActor);
            
            MessageBus.Instance.Data.OnCreateActorData.RemoveListener(OnCreateActorData);
            MessageBus.Instance.Data.OnReleaseActorData.RemoveListener(OnReleaseActorData);
            MessageBus.Instance.Data.OnCreateInteractData.RemoveListener(OnCreateInteractData);
            MessageBus.Instance.Data.OnReleaseInteractData.RemoveListener(OnReleaseInteractData);
        }

        public void OnUpdate()
        {
            // Actorの表示
            if (questData.UserData.ControlActorData == null)
            {
                return;
            }

            UpdateObjectRotation();
            
            if (isDirty)
            {
                UpdateMarkerObject();
                isDirty = false;
            }

            UpdateMarkerDirection();
        }

        void UpdateObjectRotation()
        {
            center3DObject.transform.rotation = questData.UserData.ControlActorData.Rotation;
        }

        void UpdateMarkerObject()
        {
            foreach (var radarPointMarker in radarPointMarkerList)
            {
                radarPointMarker.gameObject.SetActive(false);
            }

            var currentAreaId = questData.UserData.ControlActorData.AreaId;
            var currentAreaActors = questData.ActorData.Values.Where(actorData => 
                actorData.InstanceId != questData.UserData.ControlActorData.InstanceId && actorData.AreaId == currentAreaId).ToArray();

            for (var i = 0; i < currentAreaActors.Length; i++)
            {
                if (radarPointMarkerList.Count <= i)
                {
                    radarPointMarkerList.Add(Instantiate(radarPointMarkerPrefab, markerParent, false));                        
                }
                
                radarPointMarkerList[i].SetMarkerTarget(
                    questData.UserData.PlayerData.InstanceId == currentAreaActors[i].PlayerInstanceId
                        ? RadarPointMarker.MarkerType.FriendActor
                        : RadarPointMarker.MarkerType.EnemyActor,
                    currentAreaActors[i]);
                radarPointMarkerList[i].gameObject.SetActive(true);
            }

            // Interactの表示
            var currentAreaInteracts = questData.InteractData.Values.Where(interactData => interactData.AreaId == currentAreaId).ToArray();
            for (var i = 0; i < currentAreaInteracts.Length; i++)
            {
                var offsetIndex = i + currentAreaActors.Length;
                
                if (radarPointMarkerList.Count <= offsetIndex)
                {
                    radarPointMarkerList.Add(Instantiate(radarPointMarkerPrefab, markerParent, false));                        
                }
                
                radarPointMarkerList[offsetIndex].SetMarkerTarget(
                    RadarPointMarker.MarkerType.InteractItem,
                    currentAreaInteracts[i]);
                radarPointMarkerList[i].gameObject.SetActive(true);
            }
        }

        void UpdateMarkerDirection()
        {
            var cameraRotation = Quaternion.Inverse(GetCameraRotation(questData.UserData));
            foreach (var radarPointMarker in radarPointMarkerList)
            {
                var direction = (radarPointMarker.MarkerTarget.Position - questData.UserData.ControlActorData.Position).normalized;
                radarPointMarker.SetDirection(cameraRotation * direction, distanceScale);
            }
        }

        Quaternion GetCameraRotation(UserData userData)
        {
            return userData.LookAtSpace
                   * Quaternion.AngleAxis(userData.LookAtAngle.y, Vector3.up)
                   * Quaternion.AngleAxis(userData.LookAtAngle.x, Vector3.right);
        }
        
        void SetUserControlActor(ActorData userControlActor)
        {
            isDirty = true;
        }
        
        void OnCreateActorData(ActorData actorData)
        {
            isDirty = true;
        }
        
        void OnReleaseActorData(ActorData actorData)
        {
            isDirty = true;
        }
        
        void OnCreateInteractData(IInteractData interactData)
        {
            isDirty = true;
        }
        
        void OnReleaseInteractData(IInteractData interactData)
        {
            isDirty = true;
        }
    }
}
