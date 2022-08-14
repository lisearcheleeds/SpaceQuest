using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RoboQuest.Quest
{
    public class PlayerManager : MonoBehaviour
    {
        static readonly float UpdateInterval = 3.0f;
        
        QuestData questData;

        Dictionary<Guid, float> updateIntervals;
        
        int? currentAreaIndex;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            updateIntervals = questData.PlayerQuestData.ToDictionary(data => data.InstanceId, _ => Random.Range(0, UpdateInterval));
            
            MessageBus.Instance.PlayerCommandSetTacticsType.AddListener(PlayerCommandSetTacticsType);
            MessageBus.Instance.PlayerCommandSetDestinateAreaIndex.AddListener(PlayerCommandSetDestinateAreaIndex);
        }

        public void Finalize()
        {
            MessageBus.Instance.PlayerCommandSetTacticsType.RemoveListener(PlayerCommandSetTacticsType);
            MessageBus.Instance.PlayerCommandSetDestinateAreaIndex.RemoveListener(PlayerCommandSetDestinateAreaIndex);
        }
        
        void LateUpdate()
        {
            if (questData == null)
            {
                return;
            }

            var deltaTime = Time.deltaTime;

            // modifiedになる可能性があるのでコピー
            foreach (var key in updateIntervals.Keys.ToArray())
            {
                updateIntervals[key] = updateIntervals[key] - deltaTime;

                if (updateIntervals[key] < 0.0f)
                {
                    updateIntervals[key] = updateIntervals[key] + UpdateInterval;
                    
                    PlayerAI.Update(questData, questData.PlayerQuestData.First(x => x.InstanceId == key));
                }
            }
            
            // modifiedになる可能性があるのでコピー
            foreach (var actorData in questData.ActorData.ToArray())
            {
                actorData.Update(deltaTime);
                
                if (actorData.CurrentAreaIndex == currentAreaIndex)
                {
                    // 現在のエリアのActorはUpdateしない
                    continue;
                }
                
                ActorAI.Update(questData, actorData, deltaTime);
            }
        }
        
        public void ResetArea()
        {
            currentAreaIndex = null;
        }

        public void OnLoadedArea(int nextAreaIndex)
        {
            currentAreaIndex = nextAreaIndex;
        }

        void PlayerCommandSetTacticsType(Guid playerInstanceId, TacticsType tacticsType)
        {
            questData.PlayerQuestData.First(x => x.InstanceId == playerInstanceId).SetTacticsType(tacticsType);
        }
        
        void PlayerCommandSetDestinateAreaIndex(Guid playerInstanceId, int? areaIndex)
        {
            questData.PlayerQuestData.First(x => x.InstanceId == playerInstanceId).SetDestinateAreaIndex(areaIndex);
            
            foreach (var actorData in questData.ActorData)
            {
                if (actorData.PlayerQuestDataInstanceId != playerInstanceId)
                {
                    return;
                }

                actorData.SetDestinateAreaIndex(areaIndex);
            }
        }
    }
}