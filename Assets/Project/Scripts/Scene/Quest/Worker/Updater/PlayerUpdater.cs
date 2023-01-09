using System;
using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AloneSpace
{
    public class PlayerUpdater : IUpdater
    {
        // 1秒間に更新を行うレート
        static readonly float TickRate = 1.0f / 1.0f;
        
        QuestData questData;

        Dictionary<Guid, float> updateTimeStamps = new Dictionary<Guid, float>();
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.PlayerCommandSetTacticsType.AddListener(PlayerCommandSetTacticsType);
        }

        public void Finalize()
        {
            MessageBus.Instance.PlayerCommandSetTacticsType.RemoveListener(PlayerCommandSetTacticsType);
        }
        
        public void OnLateUpdate()
        {
            if (questData == null)
            {
                return;
            }

            foreach (var playerQuestData in questData.PlayerQuestData)
            {
                if (!updateTimeStamps.ContainsKey(playerQuestData.InstanceId))
                {
                    updateTimeStamps[playerQuestData.InstanceId] = Time.time - TickRate - 1.0f;
                }

                if (updateTimeStamps[playerQuestData.InstanceId] < Time.time - TickRate)
                {
                    updateTimeStamps[playerQuestData.InstanceId] = Time.time;
                    
                    PlayerAI.Update(questData, playerQuestData);
                }
            }
        }

        void PlayerCommandSetTacticsType(Guid playerInstanceId, TacticsType tacticsType)
        {
            questData.PlayerQuestData.First(x => x.InstanceId == playerInstanceId).SetTacticsType(tacticsType);
        }
    }
}