using UnityEngine;

namespace AloneSpace
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] UserUpdater userUpdater;
        [SerializeField] DebugViewer debugViewer;

        MessageController messageController = new MessageController();
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
            
            userUpdater.Initialize(questData);
            
            messageController.Initialize(questData);
            interactController.Initialize(questData);
            
            playerUpdater.Initialize(questData);
            actorUpdater.Initialize(questData);
            collisionUpdater.Initialize();
            threatUpdater.Initialize();
            weaponUpdater.Initialize(questData);
            weaponEffectUpdater.Initialize();
                
            debugViewer.Initialize(questData);
        }

        public void FinishQuest()
        {
            userUpdater.Finalize();
            
            messageController.Finalize();
            interactController.Finalize();
            
            playerUpdater.Finalize();
            actorUpdater.Finalize();
            collisionUpdater.Finalize();
            threatUpdater.Finalize();
            weaponUpdater.Finalize();
            weaponEffectUpdater.Finalize();
            
            debugViewer.Finalize();
        }
        
        void FixedUpdate()
        {
            var fixedDeltaTime = Time.fixedDeltaTime;

            // 定期処理
            playerUpdater.OnThinkUpdate();
            actorUpdater.OnThinkUpdate();
        }

        void LateUpdate()
        {
            var deltaTime = Time.deltaTime;
            
            // 毎フレーム処理
            userUpdater.OnLateUpdate();
            
            actorUpdater.OnLateUpdate(deltaTime);
            
            collisionUpdater.OnLateUpdate();
            threatUpdater.OnLateUpdate();
            weaponUpdater.OnLateUpdate(deltaTime);
            weaponEffectUpdater.OnLateUpdate(deltaTime);
        }
    }
}
