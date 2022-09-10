using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace AloneSpace
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] InteractManager interactManager;
        [SerializeField] PlayerManager playerManager;
        [SerializeField] NoticeManager noticeManager;
     
        [SerializeField] UIManager uiManager;
        [SerializeField] AreaController areaController;
        
        [SerializeField] DebugViewer debugViewer;
        
        Action endQuest;

        QuestData questData;
        int? currentAreaIndex;

        public void StartQuest(QuestData questData, Action endQuest)
        {
            this.questData = questData;
            Initialize();
            
            MessageBus.Instance.UserCommandSetObservePlayer.Broadcast(questData.PlayerQuestData.First().InstanceId);
        }

        void Initialize()
        {
            interactManager.Initialize(questData);
            playerManager.Initialize(questData);
            noticeManager.Initialize(questData);
            
            uiManager.Initialize(questData);
            areaController.Initialize(questData);
                
            debugViewer.Initialize(questData);
            
            MessageBus.Instance.UserCommandSetObservePlayer.AddListener(UserCommandSetObservePlayer);
            MessageBus.Instance.UserCommandSetObserveActor.AddListener(UserCommandSetObserveActor);
            MessageBus.Instance.SetObserveArea.AddListener(SetObserveArea);
            
            MessageBus.Instance.ManagerCommandTransitionActor.AddListener(ManagerCommandTransitionActor);
        }

        void FinishQuest()
        {
            interactManager.Finalize();
            playerManager.Finalize();
            noticeManager.Finalize();
            
            uiManager.Finalize();
            areaController.Finalize();
            
            debugViewer.Finalize();
            
            endQuest();
        }

        void UserCommandSetObservePlayer(Guid playerInstanceId)
        {
            questData.UserCommandSetObservePlayer(playerInstanceId);
            MessageBus.Instance.UserCommandSetObserveActor.Broadcast(
                questData.PlayerQuestData.First(playerQuestData => playerQuestData.InstanceId == playerInstanceId).MainActorData.InstanceId);
        }

        void UserCommandSetObserveActor(Guid actorInstanceId)
        {
            questData.UserCommandSetObserveActor(actorInstanceId);
            MessageBus.Instance.SetObserveArea.Broadcast(questData.ObserveActor.CurrentAreaIndex);
        }

        void SetObserveArea(int areaIndex)
        {
            StartCoroutine(LoadArea(areaIndex));
        }

        void ManagerCommandTransitionActor(ActorData actorData, int fromAreaIndex, int toAreaIndex)
        {
            actorData.SetCurrentAreaIndex(toAreaIndex);

            if (questData.ObserveActor.InstanceId == actorData.InstanceId)
            {
                MessageBus.Instance.SetObserveArea.Broadcast(toAreaIndex);
            }
        }

        IEnumerator LoadArea(int areaIndex)
        {
            if (currentAreaIndex.HasValue)
            {
                playerManager.ResetArea();
                uiManager.ResetArea();
            }
            
            questData.SetObserveArea(areaIndex);

            yield return areaController.LoadArea(areaIndex);
            
            playerManager.OnLoadedArea(areaIndex);
            areaController.OnLoadedArea();
            uiManager.OnLoadedArea();

            currentAreaIndex = areaIndex;
        }
    }
}
