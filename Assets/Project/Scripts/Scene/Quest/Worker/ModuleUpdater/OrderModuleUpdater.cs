using System.Collections.Generic;

namespace AloneSpace
{
    public class OrderModuleUpdater
    {
        QuestData questData;

        LinkedList<IOrderModule> moduleList = new LinkedList<IOrderModule>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.Module.RegisterOrderModule.AddListener(RegisterOrderModule);
            MessageBus.Instance.Module.UnRegisterOrderModule.AddListener(UnRegisterOrderModule);
        }

        public void Finalize()
        {
            MessageBus.Instance.Module.RegisterOrderModule.RemoveListener(RegisterOrderModule);
            MessageBus.Instance.Module.UnRegisterOrderModule.RemoveListener(UnRegisterOrderModule);
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
            moduleList.AddLast(orderModule);
        }

        void UnRegisterOrderModule(IOrderModule orderModule)
        {
            moduleList.Remove(orderModule);
        }
    }
}
