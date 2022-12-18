using System;
using System.Collections;
using UnityEngine;

namespace AloneSpace
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] UIManager uiManager;
        [SerializeField] AreaUpdater areaUpdater;
        
        [SerializeField] DebugViewer debugViewer;
        
        InteractController interactController = new InteractController();
        
        PlayerUpdater playerUpdater = new PlayerUpdater();
        ActorUpdater actorUpdater = new ActorUpdater();
        CollisionUpdater collisionUpdater = new CollisionUpdater();
        ThreatUpdater threatUpdater = new ThreatUpdater();
        WeaponUpdater weaponUpdater = new WeaponUpdater();
        WeaponEffectUpdater weaponEffectUpdater = new WeaponEffectUpdater();
        
        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            uiManager.Initialize(questData);
            areaUpdater.Initialize(questData);
            
            interactController.Initialize(questData);
            
            playerUpdater.Initialize(questData);
            actorUpdater.Initialize(questData);
            collisionUpdater.Initialize();
            threatUpdater.Initialize();
            weaponUpdater.Initialize(questData);
            weaponEffectUpdater.Initialize();
                
            debugViewer.Initialize(questData);
            
            MessageBus.Instance.AddPlayerQuestData.AddListener(AddPlayerQuestData);
            MessageBus.Instance.AddActorData.AddListener(AddActorData);
            MessageBus.Instance.RemoveActorData.AddListener(RemoveActorData);
            
            MessageBus.Instance.SetObserveArea.AddListener(SetObserveArea);
            
            MessageBus.Instance.ManagerCommandTransitionActor.AddListener(ManagerCommandTransitionActor);
        }

        public IEnumerator StartQuest()
        {
            return LoadArea();
        }

        public void FinishQuest()
        {
            uiManager.Finalize();
            areaUpdater.Finalize();
            
            interactController.Finalize();
            
            playerUpdater.Finalize();
            actorUpdater.Finalize();
            collisionUpdater.Finalize();
            threatUpdater.Finalize();
            weaponUpdater.Finalize();
            weaponEffectUpdater.Finalize();
            
            debugViewer.Finalize();
            
            MessageBus.Instance.AddPlayerQuestData.RemoveListener(AddPlayerQuestData);
            MessageBus.Instance.AddActorData.RemoveListener(AddActorData);
            MessageBus.Instance.RemoveActorData.RemoveListener(RemoveActorData);
            
            MessageBus.Instance.SetObserveArea.RemoveListener(SetObserveArea);
            
            MessageBus.Instance.ManagerCommandTransitionActor.RemoveListener(ManagerCommandTransitionActor);
        }

        void LateUpdate()
        {
            areaUpdater.OnLateUpdate();
            
            playerUpdater.OnLateUpdate();
            actorUpdater.OnLateUpdate();
            collisionUpdater.OnLateUpdate();
            threatUpdater.OnLateUpdate();
            weaponUpdater.OnLateUpdate();
            weaponEffectUpdater.OnLateUpdate();
        }
        
        void AddPlayerQuestData(PlayerQuestData playerQuestData)
        {
            questData.AddPlayerQuestData(playerQuestData);
        }
        
        void AddActorData(ActorData actorData)
        {
            questData.AddActorData(actorData);
        }

        void RemoveActorData(ActorData actorData)
        {
            questData.RemoveActorData(actorData);
        }
        
        void SetObserveArea(int areaIndex)
        {
            questData.SetObserveArea(areaIndex);
            
            StartCoroutine(LoadArea());
        }

        void ManagerCommandTransitionActor(ActorData actorData, int fromAreaIndex, int toAreaIndex)
        {
            actorData.SetAreaIndex(toAreaIndex);

            if (questData.ObservePlayerQuestData.MainActorData.InstanceId == actorData.InstanceId)
            {
                MessageBus.Instance.SetObserveArea.Broadcast(toAreaIndex);
            }
        }

        IEnumerator LoadArea()
        {
            yield return areaUpdater.LoadArea();
            uiManager.OnLoadedArea();
        }
    }
}
