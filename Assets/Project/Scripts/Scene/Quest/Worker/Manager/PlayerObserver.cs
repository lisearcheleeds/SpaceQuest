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
                // ワープ中は一番近いエリアを常に見る
                MessageBus.Instance.UtilGetNearestAreaData.Broadcast(
                    observePlayerQuestData.MainActorData,
                    nearestAreaData =>
                    {
                        if (currentAreaData == null || currentAreaData.AreaId != nearestAreaData.AreaId)
                        {
                            MessageBus.Instance.ManagerCommandLoadArea.Broadcast(nearestAreaData);
                        }
                    });
            }
        }
        
        void ManagerCommandSetObservePlayer(Guid playerInstanceId)
        {
            observePlayerQuestData = questData.PlayerQuestData.FirstOrDefault(x => x.InstanceId == playerInstanceId);
            
            uiManager.SetObservePlayerQuestData(observePlayerQuestData);
        }

        void ManagerCommandLoadArea(AreaData areaData)
        {
            currentAreaData = areaData;
            
            uiManager.SetObserveAreaData(areaData);
            areaUpdater.SetObserveAreaData(areaData);
        }
    }
}