using UnityEngine;

namespace AloneSpace
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] UserUpdater userUpdater;
        [SerializeField] DebugViewer debugViewer;

        QuestDataMessageResolver questDataMessageResolver = new QuestDataMessageResolver();
        InteractMessageResolver interactMessageResolver = new InteractMessageResolver();
        PlayerMessageResolver playerMessageResolver = new PlayerMessageResolver();
        ActorMessageResolver actorMessageResolver = new ActorMessageResolver();
        WeaponMessageResolver weaponMessageResolver = new WeaponMessageResolver();

        ThinkModuleUpdater thinkModuleUpdater = new ThinkModuleUpdater();
        OrderModuleUpdater orderModuleUpdater = new OrderModuleUpdater();
        MovingModuleUpdater movingModuleUpdater = new MovingModuleUpdater();

        CollisionChecker collisionChecker = new CollisionChecker();

        CollisionEventModuleUpdater collisionEventModuleUpdater = new CollisionEventModuleUpdater();

        CollisionEffectSenderModuleUpdater collisionEffectSenderModuleUpdater = new CollisionEffectSenderModuleUpdater();
        CollisionEffectReceiverModuleUpdater collisionEffectReceiverModuleUpdater = new CollisionEffectReceiverModuleUpdater();


        public void Initialize(QuestData questData)
        {
            userUpdater.Initialize(questData);
            debugViewer.Initialize(questData);

            questDataMessageResolver.Initialize(questData);
            interactMessageResolver.Initialize(questData);
            playerMessageResolver.Initialize(questData);
            actorMessageResolver.Initialize(questData);
            weaponMessageResolver.Initialize(questData);

            thinkModuleUpdater.Initialize(questData);
            orderModuleUpdater.Initialize(questData);
            movingModuleUpdater.Initialize(questData);

            // collisionChecker.Initialize();

            collisionEventModuleUpdater.Initialize(questData);

            collisionEffectSenderModuleUpdater.Initialize(questData);
            collisionEffectReceiverModuleUpdater.Initialize(questData);
        }

        public void FinishQuest()
        {
            userUpdater.Finalize();
            debugViewer.Finalize();

            questDataMessageResolver.Finalize();
            interactMessageResolver.Finalize();
            playerMessageResolver.Finalize();
            actorMessageResolver.Finalize();
            weaponMessageResolver.Finalize();

            thinkModuleUpdater.Finalize();
            orderModuleUpdater.Finalize();
            movingModuleUpdater.Finalize();

            // collisionChecker.Finalize();

            collisionEventModuleUpdater.Finalize();

            collisionEffectSenderModuleUpdater.Finalize();
            collisionEffectReceiverModuleUpdater.Finalize();
        }

        void LateUpdate()
        {
            var deltaTime = Time.deltaTime;

            userUpdater.OnLateUpdate(deltaTime);

            thinkModuleUpdater.UpdateModule(deltaTime);
            orderModuleUpdater.UpdateModule(deltaTime);
            movingModuleUpdater.UpdateModule(deltaTime);

            // collisionChecker.OnLateUpdate();

            collisionEventModuleUpdater.UpdateModule(deltaTime);

            collisionEffectSenderModuleUpdater.UpdateModule(deltaTime);
            collisionEffectReceiverModuleUpdater.UpdateModule(deltaTime);
        }

        /*
        宇宙船と兵装パーツを作る
        リネーム祭り
        当たり判定
        ダメージ処理
        アイテム拾う処理
        換装する処理というかUI
        */
    }
}
