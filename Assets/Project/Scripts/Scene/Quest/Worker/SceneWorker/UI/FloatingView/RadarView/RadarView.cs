using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class RadarView : MonoBehaviour
    {
        [SerializeField] RadarPointMarker radarPointMarkerPrefab;
        [SerializeField] RectTransform markerParent;
        [SerializeField] float distanceScale;

        List<RadarPointMarker> radarPointMarkerList = new List<RadarPointMarker>();
        bool isDirty;
        
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.SetUserControlActor.AddListener(SetUserControlActor);
            
            MessageBus.Instance.CreatedActorData.AddListener(CreatedActorData);
            MessageBus.Instance.ReleasedActorData.AddListener(ReleasedActorData);
            MessageBus.Instance.CreatedInteractData.AddListener(CreatedInteractData);
            MessageBus.Instance.ReleasedInteractData.AddListener(ReleasedInteractData);
                    
        }

        public void Finalize()
        {
            MessageBus.Instance.SetUserControlActor.RemoveListener(SetUserControlActor);
            
            MessageBus.Instance.CreatedActorData.RemoveListener(CreatedActorData);
            MessageBus.Instance.ReleasedActorData.RemoveListener(ReleasedActorData);
            MessageBus.Instance.CreatedInteractData.RemoveListener(CreatedInteractData);
            MessageBus.Instance.ReleasedInteractData.RemoveListener(ReleasedInteractData);
        }

        public void OnUpdate()
        {
            if (isDirty)
            {
                isDirty = false;

                foreach (var radarPointMarker in radarPointMarkerList)
                {
                    radarPointMarker.gameObject.SetActive(false);
                }
                
                var controlActorData = questData.UserData.ControlActorData;
                var currentAreaId = controlActorData.AreaId;
                var currentAreaActors = questData.ActorData.Values.Where(actorData => 
                    actorData.InstanceId != controlActorData.InstanceId && actorData.AreaId == currentAreaId).ToArray();

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

            var cameraRotation = Quaternion.Inverse(GetTargetRotation(questData.UserData));
            foreach (var radarPointMarker in radarPointMarkerList)
            {
                var direction = (radarPointMarker.MarkerTarget.Position - questData.UserData.ControlActorData.Position).normalized;
                radarPointMarker.SetDirection(cameraRotation * direction, distanceScale);
            }
        }

        Quaternion GetTargetRotation(UserData userData)
        {
            return userData.LookAtSpace
                   * Quaternion.AngleAxis(userData.LookAtAngle.y, Vector3.up)
                   * Quaternion.AngleAxis(userData.LookAtAngle.x, Vector3.right);
        }
        
        void SetUserControlActor(ActorData userControlActor)
        {
            isDirty = true;
        }
        
        void CreatedActorData(ActorData actorData)
        {
            isDirty = true;
        }
        
        void ReleasedActorData(ActorData actorData)
        {
            isDirty = true;
        }
        
        void CreatedInteractData(IInteractData interactData)
        {
            isDirty = true;
        }
        
        void ReleasedInteractData(IInteractData interactData)
        {
            isDirty = true;
        }
    }
}
