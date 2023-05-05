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
        
        List<IThinkModule> registerModuleList = new List<IThinkModule>();
        List<IThinkModule> unRegisterModuleList = new List<IThinkModule>();

        
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

            foreach (var removeModule in unRegisterModuleList)
            {
                moduleList.Remove(removeModule);
                updateTimeStamps.Remove(removeModule.InstanceId);
            }
            
            unRegisterModuleList.Clear();

            foreach (var module in moduleList)
            {
                if (updateTimeStamps[module.InstanceId] + TickRate < Time.time)
                {
                    module.OnUpdateModule(Time.time - updateTimeStamps[module.InstanceId]);
                    updateTimeStamps[module.InstanceId] += TickRate;
                }
            }

            foreach (var registerModule in registerModuleList)
            {
                moduleList.Add(registerModule);
                updateTimeStamps[registerModule.InstanceId] = Time.time - TickRate - 1.0f;
            }
            
            registerModuleList.Clear();
        }

        void RegisterThinkModule(IThinkModule thinkModule)
        {
            registerModuleList.Add(thinkModule);
        }
        
        void UnRegisterThinkModule(IThinkModule thinkModule)
        {
            unRegisterModuleList.Add(thinkModule);
        }
    }
}