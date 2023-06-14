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
        CreateMessageResolver createMessageResolver = new CreateMessageResolver();

        ThinkModuleUpdater thinkModuleUpdater = new ThinkModuleUpdater();
        OrderModuleUpdater orderModuleUpdater = new OrderModuleUpdater();
        MovingModuleUpdater movingModuleUpdater = new MovingModuleUpdater();

        CollisionChecker collisionChecker = new CollisionChecker();

        CollisionEventModuleUpdater collisionEventModuleUpdater = new CollisionEventModuleUpdater();

        CollisionEffectSenderModuleUpdater collisionEffectSenderModuleUpdater = new CollisionEffectSenderModuleUpdater();
        CollisionEffectReceiverModuleUpdater collisionEffectReceiverModuleUpdater = new CollisionEffectReceiverModuleUpdater();

        CreatedDataUpdater createdDataUpdater = new CreatedDataUpdater();
        ReleasedDataUpdater releasedDataUpdater = new ReleasedDataUpdater();

        public void Initialize(QuestData questData)
        {
            createdDataUpdater.Initialize(questData);

            userUpdater.Initialize(questData);

            questUpdater.Initialize(questData);

            questDataMessageResolver.Initialize(questData);
            interactMessageResolver.Initialize(questData);
            playerMessageResolver.Initialize(questData);
            actorMessageResolver.Initialize(questData);
            createMessageResolver.Initialize(questData);

            thinkModuleUpdater.Initialize(questData);
            orderModuleUpdater.Initialize(questData);
            movingModuleUpdater.Initialize(questData);

            // collisionChecker.Initialize();

            collisionEventModuleUpdater.Initialize(questData);

            collisionEffectSenderModuleUpdater.Initialize(questData);
            collisionEffectReceiverModuleUpdater.Initialize(questData);

            releasedDataUpdater.Initialize(questData);
        }

        public void OnStart()
        {
            questUpdater.OnStart();

            // TODO SetDirtyを軒並み呼ぶ
            MessageBus.Instance.SetDirtyActorObjectList.Broadcast();
        }

        public void Finalize()
        {
            createdDataUpdater.Finalize();

            userUpdater.Finalize();

            questUpdater.Finalize();

            questDataMessageResolver.Finalize();
            interactMessageResolver.Finalize();
            playerMessageResolver.Finalize();
            actorMessageResolver.Finalize();
            createMessageResolver.Finalize();

            thinkModuleUpdater.Finalize();
            orderModuleUpdater.Finalize();
            movingModuleUpdater.Finalize();

            // collisionChecker.Finalize();

            collisionEventModuleUpdater.Finalize();

            collisionEffectSenderModuleUpdater.Finalize();
            collisionEffectReceiverModuleUpdater.Finalize();

            releasedDataUpdater.Finalize();
        }

        public void OnLateUpdate()
        {
            var deltaTime = Time.deltaTime;

            createdDataUpdater.OnLateUpdate(deltaTime);

            userUpdater.OnLateUpdate(deltaTime);

            questUpdater.OnLateUpdate(deltaTime);

            thinkModuleUpdater.UpdateModule(deltaTime);
            orderModuleUpdater.UpdateModule(deltaTime);
            movingModuleUpdater.UpdateModule(deltaTime);

            // collisionChecker.OnLateUpdate();

            collisionEventModuleUpdater.UpdateModule(deltaTime);

            collisionEffectSenderModuleUpdater.UpdateModule(deltaTime);
            collisionEffectReceiverModuleUpdater.UpdateModule(deltaTime);

            releasedDataUpdater.OnLateUpdate(deltaTime);
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
