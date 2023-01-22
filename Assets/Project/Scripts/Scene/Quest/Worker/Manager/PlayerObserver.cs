using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class PlayerObserver : MonoBehaviour
    {
        [SerializeField] UIManager uiManager;
        [SerializeField] AreaUpdater areaUpdater;

        PlayerQuestData observePlayerQuestData;
        AreaData currentAreaData;
        
        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            uiManager.Initialize(questData);
            areaUpdater.Initialize(questData);
            
            MessageBus.Instance.ManagerCommandSetObservePlayer.AddListener(ManagerCommandSetObservePlayer);
            MessageBus.Instance.ManagerCommandLoadArea.AddListener(ManagerCommandLoadArea);
        }

        public void Finalize()
        {
            uiManager.Finalize();
            areaUpdater.Finalize();
            
            MessageBus.Instance.ManagerCommandSetObservePlayer.RemoveListener(ManagerCommandSetObservePlayer);
            MessageBus.Instance.ManagerCommandLoadArea.RemoveListener(ManagerCommandLoadArea);
        }
        
        public void OnLateUpdate()
        {
            if (observePlayerQuestData == null)
            {
                return;
            }

            uiManager.OnLateUpdate();
            areaUpdater.OnLateUpdate();
            
            if (observePlayerQuestData.MainActorData.ActorMode == ActorMode.Warp)
            {
                if (observePlayerQuestData.MainActorData.AreaId != currentAreaData?.AreaId)
                {
                    MessageBus.Instance.ManagerCommandLoadArea.Broadcast(observePlayerQuestData.MainActorData.AreaId);
                }
            }
        }
        
        void ManagerCommandSetObservePlayer(Guid playerInstanceId)
        {
            observePlayerQuestData = questData.PlayerQuestData.First(x => x.InstanceId == playerInstanceId);
            
            uiManager.SetObservePlayerQuestData(observePlayerQuestData);
            areaUpdater.SetObservePlayerQuestData(observePlayerQuestData);
            
            MessageBus.Instance.ManagerCommandLoadArea.Broadcast(observePlayerQuestData.MainActorData.AreaId);
            MessageBus.Instance.UserCommandSetCameraTrackTarget.Broadcast(observePlayerQuestData.MainActorData);
        }

        void ManagerCommandLoadArea(int? areaId)
        {
            currentAreaData = questData.StarSystemData.AreaData.FirstOrDefault(x => x.AreaId == areaId);
            
            uiManager.SetObserveAreaData(currentAreaData);
            areaUpdater.SetObserveAreaData(currentAreaData);
        }
    }
}