using UnityEngine;

namespace AloneSpace
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] UserUpdater userUpdater;
        [SerializeField] DebugViewer debugViewer;

        UtilMessageSolver utilMessageSolver = new UtilMessageSolver();
        InteractMessageResolver interactMessageResolver = new InteractMessageResolver();
        PlayerMessageResolver playerMessageResolver = new PlayerMessageResolver();
        ActorMessageResolver actorMessageResolver = new ActorMessageResolver();
        WeaponMessageResolver weaponMessageResolver = new WeaponMessageResolver();

        ThinkModuleUpdater thinkModuleUpdater = new ThinkModuleUpdater();
        OrderModuleUpdater orderModuleUpdater = new OrderModuleUpdater();
        MovingModuleUpdater movingModuleUpdater = new MovingModuleUpdater();
        CollisionEffectSenderModuleUpdater collisionEffectSenderModuleUpdater = new CollisionEffectSenderModuleUpdater();
        CollisionEffectReceiverModuleUpdater collisionEffectReceiverModuleUpdater = new CollisionEffectReceiverModuleUpdater();

        CollisionChecker collisionChecker = new CollisionChecker();

        public void Initialize(QuestData questData)
        {
            userUpdater.Initialize(questData);
            debugViewer.Initialize(questData);

            utilMessageSolver.Initialize(questData);
            interactMessageResolver.Initialize(questData);
            playerMessageResolver.Initialize(questData);
            actorMessageResolver.Initialize(questData);
            weaponMessageResolver.Initialize(questData);

            thinkModuleUpdater.Initialize(questData);
            orderModuleUpdater.Initialize(questData);
            movingModuleUpdater.Initialize(questData);
            collisionEffectSenderModuleUpdater.Initialize(questData);
            collisionEffectReceiverModuleUpdater.Initialize(questData);

            collisionChecker.Initialize();
        }

        public void FinishQuest()
        {
            userUpdater.Finalize();
            debugViewer.Finalize();

            utilMessageSolver.Finalize();
            interactMessageResolver.Finalize();
            playerMessageResolver.Finalize();
            actorMessageResolver.Finalize();
            weaponMessageResolver.Finalize();

            thinkModuleUpdater.Finalize();
            orderModuleUpdater.Finalize();
            movingModuleUpdater.Finalize();
            collisionEffectSenderModuleUpdater.Finalize();
            collisionEffectReceiverModuleUpdater.Finalize();

            collisionChecker.Finalize();
        }

        void LateUpdate()
        {
            var deltaTime = Time.deltaTime;

            userUpdater.OnLateUpdate();

            thinkModuleUpdater.UpdateModule(deltaTime);
            orderModuleUpdater.UpdateModule(deltaTime);
            movingModuleUpdater.UpdateModule(deltaTime);
            collisionEffectSenderModuleUpdater.UpdateModule(deltaTime);
            collisionEffectReceiverModuleUpdater.UpdateModule(deltaTime);

            collisionChecker.OnLateUpdate();
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
