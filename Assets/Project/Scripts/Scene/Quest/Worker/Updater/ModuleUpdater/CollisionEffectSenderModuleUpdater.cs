using System.Collections.Generic;

namespace AloneSpace
{
    public class CollisionEffectSenderModuleUpdater
    {
        QuestData questData;

        List<CollisionEffectSenderModule> moduleList = new List<CollisionEffectSenderModule>();
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.RegisterCollisionEffectSenderModule.AddListener(RegisterCollisionEffectSenderModule);
            MessageBus.Instance.UnRegisterCollisionEffectSenderModule.AddListener(UnRegisterCollisionEffectSenderModule);
        }

        public void Finalize()
        {
            MessageBus.Instance.RegisterCollisionEffectSenderModule.RemoveListener(RegisterCollisionEffectSenderModule);
            MessageBus.Instance.UnRegisterCollisionEffectSenderModule.RemoveListener(UnRegisterCollisionEffectSenderModule);
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

        void RegisterCollisionEffectSenderModule(CollisionEffectSenderModule CollisionEffectSenderModule)
        {
            moduleList.Add(CollisionEffectSenderModule);
        }
        
        void UnRegisterCollisionEffectSenderModule(CollisionEffectSenderModule CollisionEffectSenderModule)
        {
            moduleList.Remove(CollisionEffectSenderModule);
        }
    }
}