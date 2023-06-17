using UnityEngine;

namespace AloneSpace
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] UserUpdater userUpdater;

        QuestUpdater questUpdater = new QuestUpdater();

        QuestDataMessageResolver questDataMessageResolver = new QuestDataMessageResolver();
        InteractMessageResolver interactMessageResolver = new InteractMessageResolver();
        PlayerMessageResolver playerMessageResolver = new PlayerMessageResolver();
        ActorMessageResolver actorMessageResolver = new ActorMessageResolver();
        UserMessageResolver userMessageResolver = new UserMessageResolver();

        ThinkModuleUpdater thinkModuleUpdater = new ThinkModuleUpdater();
        OrderModuleUpdater orderModuleUpdater = new OrderModuleUpdater();
        MovingModuleUpdater movingModuleUpdater = new MovingModuleUpdater();
        CollisionEventModuleUpdater collisionEventModuleUpdater = new CollisionEventModuleUpdater();
        CollisionEffectSenderModuleUpdater collisionEffectSenderModuleUpdater = new CollisionEffectSenderModuleUpdater();
        CollisionEffectReceiverModuleUpdater collisionEffectReceiverModuleUpdater = new CollisionEffectReceiverModuleUpdater();

        CollisionChecker collisionChecker = new CollisionChecker();

        CreateDataController createDataController = new CreateDataController();
        ReleaseDataController releaseDataController = new ReleaseDataController();

        public void Initialize(QuestData questData)
        {
            createDataController.Initialize(questData);

            userUpdater.Initialize(questData);

            questUpdater.Initialize(questData);

            questDataMessageResolver.Initialize(questData);
            interactMessageResolver.Initialize(questData);
            playerMessageResolver.Initialize(questData);
            actorMessageResolver.Initialize(questData);
            userMessageResolver.Initialize(questData);

            thinkModuleUpdater.Initialize(questData);
            orderModuleUpdater.Initialize(questData);
            movingModuleUpdater.Initialize(questData);

            // collisionChecker.Initialize();

            collisionEventModuleUpdater.Initialize(questData);

            collisionEffectSenderModuleUpdater.Initialize(questData);
            collisionEffectReceiverModuleUpdater.Initialize(questData);

            releaseDataController.Initialize(questData);
        }

        public void OnStart()
        {
            questUpdater.OnStart();
        }

        public void Finalize()
        {
            createDataController.Finalize();

            userUpdater.Finalize();

            questUpdater.Finalize();

            questDataMessageResolver.Finalize();
            interactMessageResolver.Finalize();
            playerMessageResolver.Finalize();
            actorMessageResolver.Finalize();
            userMessageResolver.Finalize();

            thinkModuleUpdater.Finalize();
            orderModuleUpdater.Finalize();
            movingModuleUpdater.Finalize();

            // collisionChecker.Finalize();

            collisionEventModuleUpdater.Finalize();

            collisionEffectSenderModuleUpdater.Finalize();
            collisionEffectReceiverModuleUpdater.Finalize();

            releaseDataController.Finalize();
        }

        public void OnLateUpdate()
        {
            var deltaTime = Time.deltaTime;

            createDataController.OnLateUpdate(deltaTime);

            userUpdater.OnLateUpdate(deltaTime);

            questUpdater.OnLateUpdate(deltaTime);

            thinkModuleUpdater.UpdateModule(deltaTime);
            orderModuleUpdater.UpdateModule(deltaTime);
            movingModuleUpdater.UpdateModule(deltaTime);

            // collisionChecker.OnLateUpdate();

            collisionEventModuleUpdater.UpdateModule(deltaTime);

            collisionEffectSenderModuleUpdater.UpdateModule(deltaTime);
            collisionEffectReceiverModuleUpdater.UpdateModule(deltaTime);

            releaseDataController.OnLateUpdate(deltaTime);
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
