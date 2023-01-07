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
        static readonly float UpdateInterval = 3.0f;
        
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

            var deltaTime = Time.deltaTime;

            // modifiedになる可能性があるのでコピー
            foreach (var key in updateTimeStamps.Keys.ToArray())
            {
                if (!updateTimeStamps.ContainsKey(key))
                {
                    updateTimeStamps[key] = Time.time - UpdateInterval - 1.0f;
                }

                if (updateTimeStamps[key] < Time.time - UpdateInterval)
                {
                    updateTimeStamps[key] = Time.time;
                    
                    PlayerAI.Update(questData, questData.PlayerQuestData.First(x => x.InstanceId == key));
                }
            }
            
            // modifiedになる可能性があるのでコピー
            foreach (var actorData in questData.ActorData.ToArray())
            {
                actorData.Update(deltaTime);
                ActorAI.Update(questData, actorData, deltaTime);
            }
        }

        void PlayerCommandSetTacticsType(Guid playerInstanceId, TacticsType tacticsType)
        {
            questData.PlayerQuestData.First(x => x.InstanceId == playerInstanceId).SetTacticsType(tacticsType);
        }
    }
}