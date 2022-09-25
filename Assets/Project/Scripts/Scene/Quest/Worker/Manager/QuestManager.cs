using System;
using System.Collections;
using System.Linq;
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
        
        Action endQuest;

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
            weaponEffectUpdater.Initialize(questData);
                
            debugViewer.Initialize(questData);
            
            MessageBus.Instance.AddPlayerQuestData.AddListener(AddPlayerQuestData);
            MessageBus.Instance.AddActorData.AddListener(AddActorData);
            MessageBus.Instance.RemoveActorData.AddListener(RemoveActorData);
            MessageBus.Instance.AddWeaponEffectData.AddListener(AddWeaponEffectData);
            MessageBus.Instance.RemoveWeaponEffectData.AddListener(RemoveWeaponEffectData);
            
            MessageBus.Instance.UserCommandSetObservePlayer.AddListener(UserCommandSetObservePlayer);
            MessageBus.Instance.UserCommandSetObserveActor.AddListener(UserCommandSetObserveActor);
            MessageBus.Instance.SetObserveArea.AddListener(SetObserveArea);
            
            MessageBus.Instance.ManagerCommandTransitionActor.AddListener(ManagerCommandTransitionActor);
        }

        public void StartQuest()
        {
            QuestManagerUtil.InitializePlayer(questData);
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
            MessageBus.Instance.AddWeaponEffectData.RemoveListener(AddWeaponEffectData);
            MessageBus.Instance.RemoveWeaponEffectData.RemoveListener(RemoveWeaponEffectData);
            
            MessageBus.Instance.UserCommandSetObservePlayer.RemoveListener(UserCommandSetObservePlayer);
            MessageBus.Instance.UserCommandSetObserveActor.RemoveListener(UserCommandSetObserveActor);
            MessageBus.Instance.SetObserveArea.RemoveListener(SetObserveArea);
            
            MessageBus.Instance.ManagerCommandTransitionActor.RemoveListener(ManagerCommandTransitionActor);
            
            endQuest();
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

        void AddWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            questData.AddWeaponEffectData(weaponEffectData);
        }
        
        void RemoveWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            questData.RemoveWeaponEffectData(weaponEffectData);
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
            MessageBus.Instance.SetObserveArea.Broadcast(questData.ObserveActor.AreaIndex);
        }

        void SetObserveArea(int areaIndex)
        {
            StartCoroutine(LoadArea(areaIndex));
        }

        void ManagerCommandTransitionActor(ActorData actorData, int fromAreaIndex, int toAreaIndex)
        {
            actorData.SetAreaIndex(toAreaIndex);

            if (questData.ObserveActor.InstanceId == actorData.InstanceId)
            {
                MessageBus.Instance.SetObserveArea.Broadcast(toAreaIndex);
            }
        }

        IEnumerator LoadArea(int areaIndex)
        {
            questData.SetObserveArea(areaIndex);

            yield return areaUpdater.LoadArea(areaIndex);
            
            areaUpdater.OnLoadedArea();
            uiManager.OnLoadedArea();
        }
    }
}
