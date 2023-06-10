using System.Collections.Generic;

namespace AloneSpace
{
    public class OrderModuleUpdater
    {
        QuestData questData;

        List<IOrderModule> moduleList = new List<IOrderModule>();
        
        List<IOrderModule> registerModuleList = new List<IOrderModule>();
        List<IOrderModule> unRegisterModuleList = new List<IOrderModule>();
        
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

        void RegisterOrderModule(IOrderModule orderModule)
        {
            registerModuleList.Add(orderModule);
        }
        
        void UnRegisterOrderModule(IOrderModule orderModule)
        {
            unRegisterModuleList.Add(orderModule);
        }
    }
}