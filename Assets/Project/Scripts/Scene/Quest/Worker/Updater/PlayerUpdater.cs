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

        Dictionary<Guid, float> updateIntervals;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            updateIntervals = questData.PlayerQuestData.ToDictionary(data => data.InstanceId, _ => Random.Range(0, UpdateInterval));
            
            MessageBus.Instance.PlayerCommandSetTacticsType.AddListener(PlayerCommandSetTacticsType);
            MessageBus.Instance.PlayerCommandSetMoveTarget.AddListener(PlayerCommandSetMoveTarget);
        }

        public void Finalize()
        {
            MessageBus.Instance.PlayerCommandSetTacticsType.RemoveListener(PlayerCommandSetTacticsType);
            MessageBus.Instance.PlayerCommandSetMoveTarget.RemoveListener(PlayerCommandSetMoveTarget);
        }
        
        public void OnLateUpdate()
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
                ActorAI.Update(questData, actorData, deltaTime);
            }
        }

        void PlayerCommandSetTacticsType(Guid playerInstanceId, TacticsType tacticsType)
        {
            questData.PlayerQuestData.First(x => x.InstanceId == playerInstanceId).SetTacticsType(tacticsType);
        }
        
        void PlayerCommandSetMoveTarget(Guid playerInstanceId, IPosition moveTarget)
        {
            questData.PlayerQuestData.First(x => x.InstanceId == playerInstanceId).SetMoveTarget(moveTarget);
            
            foreach (var actorData in questData.ActorData)
            {
                if (actorData.PlayerInstanceId != playerInstanceId)
                {
                    return;
                }

                actorData.SetMoveTarget(moveTarget);
            }
        }
    }
}