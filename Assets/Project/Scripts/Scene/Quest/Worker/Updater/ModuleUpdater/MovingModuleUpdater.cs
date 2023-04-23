using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class MovingModuleUpdater
    {
        QuestData questData;

        List<MovingModule> moduleList = new List<MovingModule>();
        
        List<MovingModule> registerModuleList = new List<MovingModule>();
        List<MovingModule> unRegisterModuleList = new List<MovingModule>();
        
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
                moduleList.Add(registerModule);
            }
            
            registerModuleList.Clear();
        }

        void RegisterMovingModule(MovingModule movingModule)
        {
            registerModuleList.Add(movingModule);
        }
        
        void UnRegisterMovingModule(MovingModule movingModule)
        {
            unRegisterModuleList.Add(movingModule);
        }
    }
}