using System.Collections.Generic;

namespace AloneSpace
{
    public class CollisionEffectReceiverModuleUpdater
    {
        QuestData questData;

        List<CollisionEffectReceiverModule> moduleList = new List<CollisionEffectReceiverModule>();
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.RegisterCollisionEffectReceiverModule.AddListener(RegisterCollisionEffectReceiverModule);
            MessageBus.Instance.UnRegisterCollisionEffectReceiverModule.AddListener(UnRegisterCollisionEffectReceiverModule);
        }

        public void Finalize()
        {
            MessageBus.Instance.RegisterCollisionEffectReceiverModule.RemoveListener(RegisterCollisionEffectReceiverModule);
            MessageBus.Instance.UnRegisterCollisionEffectReceiverModule.RemoveListener(UnRegisterCollisionEffectReceiverModule);
        }
        
        public void UpdateModule(float deltaTime)
        {
            if (questData == null)
            {
                return;
            }

            foreach (var module in moduleList)
            {
                module.OnUpdateModule(deltaTime);
            }
        }

        void RegisterCollisionEffectReceiverModule(CollisionEffectReceiverModule CollisionEffectReceiverModule)
        {
            moduleList.Add(CollisionEffectReceiverModule);
        }
        
        void UnRegisterCollisionEffectReceiverModule(CollisionEffectReceiverModule CollisionEffectReceiverModule)
        {
            moduleList.Remove(CollisionEffectReceiverModule);
        }
    }
}