﻿using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class MovingModuleUpdater
    {
        QuestData questData;

        LinkedList<MovingModule> moduleList = new LinkedList<MovingModule>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.Module.RegisterMovingModule.AddListener(RegisterMovingModule);
            MessageBus.Instance.Module.UnRegisterMovingModule.AddListener(UnRegisterMovingModule);
        }

        public void Finalize()
        {
            MessageBus.Instance.Module.RegisterMovingModule.RemoveListener(RegisterMovingModule);
            MessageBus.Instance.Module.UnRegisterMovingModule.RemoveListener(UnRegisterMovingModule);
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

        void RegisterMovingModule(MovingModule movingModule)
        {
            moduleList.AddLast(movingModule);
        }

        void UnRegisterMovingModule(MovingModule movingModule)
        {
            moduleList.Remove(movingModule);
        }
    }
}
