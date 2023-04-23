using System.Collections.Generic;

namespace AloneSpace
{
    public class CollisionEffectSenderModuleUpdater
    {
        QuestData questData;

        List<CollisionEffectSenderModule> moduleList = new List<CollisionEffectSenderModule>();
        
        List<CollisionEffectSenderModule> registerModuleList = new List<CollisionEffectSenderModule>();
        List<CollisionEffectSenderModule> unRegisterModuleList = new List<CollisionEffectSenderModule>();
        
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

        void RegisterCollisionEffectSenderModule(CollisionEffectSenderModule CollisionEffectSenderModule)
        {
            registerModuleList.Add(CollisionEffectSenderModule);
        }
        
        void UnRegisterCollisionEffectSenderModule(CollisionEffectSenderModule CollisionEffectSenderModule)
        {
            unRegisterModuleList.Add(CollisionEffectSenderModule);
        }
    }
}