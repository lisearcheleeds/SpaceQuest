using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class ThinkModuleUpdater
    {
        // 1秒間に更新を行うレート
        static readonly float TickRate = 1.0f / 1.0f;

        QuestData questData;

        Dictionary<Guid, float> updateTimeStamps = new Dictionary<Guid, float>();

        LinkedList<IThinkModule> moduleList = new LinkedList<IThinkModule>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.RegisterThinkModule.AddListener(RegisterThinkModule);
            MessageBus.Instance.UnRegisterThinkModule.AddListener(UnRegisterThinkModule);
        }

        public void Finalize()
        {
            MessageBus.Instance.RegisterThinkModule.RemoveListener(RegisterThinkModule);
            MessageBus.Instance.UnRegisterThinkModule.RemoveListener(UnRegisterThinkModule);
        }

        public void UpdateModule(float deltaTime)
        {
            if (questData == null)
            {
                return;
            }

            foreach (var module in moduleList)
            {
                if (updateTimeStamps[module.InstanceId] + TickRate < Time.time)
                {
                    module.OnUpdateModule(Time.time - updateTimeStamps[module.InstanceId]);
                    updateTimeStamps[module.InstanceId] += TickRate;
                }
            }
        }

        void RegisterThinkModule(IThinkModule thinkModule)
        {
            updateTimeStamps[thinkModule.InstanceId] = Time.time - TickRate - 1.0f;
            moduleList.AddLast(thinkModule);
        }

        void UnRegisterThinkModule(IThinkModule thinkModule)
        {
            moduleList.Remove(thinkModule);
        }
    }
}
