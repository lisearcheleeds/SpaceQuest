using System.Collections.Generic;

namespace AloneSpace
{
    public class OrderModuleUpdater
    {
        QuestData questData;

        List<IOrderModule> moduleList = new List<IOrderModule>();
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.RegisterOrderModule.AddListener(RegisterOrderModule);
            MessageBus.Instance.UnRegisterOrderModule.AddListener(UnRegisterOrderModule);
        }

        public void Finalize()
        {
            MessageBus.Instance.RegisterOrderModule.RemoveListener(RegisterOrderModule);
            MessageBus.Instance.UnRegisterOrderModule.RemoveListener(UnRegisterOrderModule);
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

        void RegisterOrderModule(IOrderModule orderModule)
        {
            moduleList.Add(orderModule);
        }
        
        void UnRegisterOrderModule(IOrderModule orderModule)
        {
            moduleList.Remove(orderModule);
        }
    }
}