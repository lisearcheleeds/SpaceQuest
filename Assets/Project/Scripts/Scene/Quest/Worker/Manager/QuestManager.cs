using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace AloneSpace
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] InteractManager interactManager;
        [SerializeField] AreaManager areaManager;
        [SerializeField] PlayerManager playerManager;
        [SerializeField] NoticeManager noticeManager;
     
        [SerializeField] UIManager uiManager;
        [SerializeField] AreaController areaController;
        
        [SerializeField] DebugViewer debugViewer;
        
        Action endQuest;
        int? currentAreaIndex;

        public void StartQuest(QuestData questData, Action endQuest)
        {
            interactManager.Initialize(questData);
            areaManager.Initialize(questData);
            playerManager.Initialize(questData);
            noticeManager.Initialize(questData);
            
            uiManager.Initialize(questData);
            areaController.Initialize(questData);
                
            debugViewer.Initialize(questData);
            
            MessageBus.Instance.UserCommandSetObservePlayer.AddListener(questData.UserCommandSetObservePlayer);
            MessageBus.Instance.UserCommandSetObserveActor.AddListener(questData.UserCommandSetObserveActor);
            MessageBus.Instance.SetObserveArea.AddListener(areaIndex => StartCoroutine(LoadArea(areaIndex)));

            foreach (var playerQuestData in questData.PlayerQuestData)
            {
                MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, playerQuestData.TacticsType);
            }
            
            foreach (var actorData in questData.ActorData)
            {
                // Deploy
                MessageBus.Instance.ManagerCommandActorAreaTransition.Broadcast(actorData, actorData.CurrentAreaIndex);
            }
        }

        void FinishQuest()
        {
            interactManager.Finalize();
            areaManager.Finalize();
            playerManager.Finalize();
            noticeManager.Finalize();
            
            uiManager.Finalize();
            areaController.Finalize();
            
            debugViewer.Finalize();
            
            endQuest();
        }

        IEnumerator LoadArea(int nextAreaIndex)
        {
            if (currentAreaIndex.HasValue)
            {
                playerManager.ResetArea();
                areaController.ResetArea();
                uiManager.ResetArea();
            }
            
            yield return areaController.LoadArea(nextAreaIndex);
            
            playerManager.OnLoadedArea(nextAreaIndex);
            areaController.OnLoadedArea();
            uiManager.OnLoadedArea();

            currentAreaIndex = nextAreaIndex;
        }
    }
}
