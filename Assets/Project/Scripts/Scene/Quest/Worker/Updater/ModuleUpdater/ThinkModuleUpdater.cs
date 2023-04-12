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
        List<IThinkModule> moduleList = new List<IThinkModule>();
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
        }

        public void Finalize()
        {
        }
        
        public void UpdateModule(float deltaTime)
        {
            if (questData == null)
            {
                return;
            }

            foreach (var module in moduleList)
            {
                if (!updateTimeStamps.ContainsKey(module.InstanceId))
                {
                    updateTimeStamps[module.InstanceId] = Time.time - TickRate - 1.0f;
                }

                if (updateTimeStamps[module.InstanceId] < Time.time - TickRate)
                {
                    updateTimeStamps[module.InstanceId] = Time.time;
                    
                    module.OnUpdateModule(deltaTime);
                }
            }
        }
    }
}