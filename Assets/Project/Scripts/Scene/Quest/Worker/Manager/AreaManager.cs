using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class AreaManager : MonoBehaviour
    {
        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.ManagerCommandTransitionActor.AddListener(ManagerCommandTransitionActor);
        }

        public void Finalize()
        {
            MessageBus.Instance.ManagerCommandTransitionActor.RemoveListener(ManagerCommandTransitionActor);
        }
        
        void ManagerCommandTransitionActor(ActorData actorData, int toAreaId)
        {
            if (questData.ObserveActor.InstanceId == actorData.InstanceId)
            {
                MessageBus.Instance.SetObserveArea.Broadcast(toAreaId);
            }
        }
    }
}