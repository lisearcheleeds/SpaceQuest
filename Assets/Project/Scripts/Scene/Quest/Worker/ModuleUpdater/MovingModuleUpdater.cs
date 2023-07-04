using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class MovingModuleUpdater
    {
        QuestData questData;

        LinkedList<MovingModule> moduleList = new LinkedList<MovingModule>();

        LinkedList<MovingModule> registerModuleList = new LinkedList<MovingModule>();
        LinkedList<MovingModule> unRegisterModuleList = new LinkedList<MovingModule>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.RegisterMovingModule.AddListener(RegisterMovingModule);
            MessageBus.Instance.UnRegisterMovingModule.AddListener(UnRegisterMovingModule);
        }

        public void Finalize()
        {
            MessageBus.Instance.RegisterMovingModule.RemoveListener(RegisterMovingModule);
            MessageBus.Instance.UnRegisterMovingModule.RemoveListener(UnRegisterMovingModule);
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
                moduleList.AddLast(registerModule);
            }

            registerModuleList.Clear();
        }

        void RegisterMovingModule(MovingModule movingModule)
        {
            registerModuleList.AddLast(movingModule);
        }

        void UnRegisterMovingModule(MovingModule movingModule)
        {
            unRegisterModuleList.AddLast(movingModule);
        }
    }
}
