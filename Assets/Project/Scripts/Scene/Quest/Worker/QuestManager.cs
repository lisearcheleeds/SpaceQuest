using UnityEngine;

namespace AloneSpace
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] SceneUpdater sceneUpdater;

        QuestUpdater questUpdater = new QuestUpdater();

        UtilMessageResolver utilMessageResolver = new UtilMessageResolver();
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

            sceneUpdater.Initialize(questData);
            questUpdater.Initialize(questData);

            utilMessageResolver.Initialize(questData);
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

            sceneUpdater.Finalize();
            questUpdater.Finalize();

            utilMessageResolver.Finalize();
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

            sceneUpdater.OnLateUpdate(deltaTime);
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
    }
}
