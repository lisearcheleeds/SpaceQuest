using System.Collections.Generic;

namespace AloneSpace
{
    public class CollisionEffectReceiverModuleUpdater
    {
        QuestData questData;

        List<CollisionEffectReceiverModule> moduleList = new List<CollisionEffectReceiverModule>();
        
        List<CollisionEffectReceiverModule> registerModuleList = new List<CollisionEffectReceiverModule>();
        List<CollisionEffectReceiverModule> unRegisterModuleList = new List<CollisionEffectReceiverModule>();
        
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

            foreach (var removeModule in unRegisterModuleList)
            {
                moduleList.Remove(removeModule);
            }
            
            unRegisterModuleList.Clear();

            foreach (var module in moduleList)
            {
                module.OnUpdateModule(deltaTime);
            }

            foreach (var registerModule in registerModuleList)
            {
                moduleList.Add(registerModule);
            }
            
            registerModuleList.Clear();
        }

        void RegisterCollisionEffectReceiverModule(CollisionEffectReceiverModule CollisionEffectReceiverModule)
        {
            registerModuleList.Add(CollisionEffectReceiverModule);
        }
        
        void UnRegisterCollisionEffectReceiverModule(CollisionEffectReceiverModule CollisionEffectReceiverModule)
        {
            unRegisterModuleList.Add(CollisionEffectReceiverModule);
        }
    }
}