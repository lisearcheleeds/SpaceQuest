﻿using System.Linq;
using AloneSpace;

namespace AloneSpace
{
    public static class PlayerAI
    {
        public static void Update(QuestData questData, PlayerQuestData playerQuestData)
        {
            UpdateTactics(questData, playerQuestData);
            UpdateInteract(questData, playerQuestData);
        }

        static void UpdateTactics(QuestData questData, PlayerQuestData playerQuestData)
        {
            if (playerQuestData.PlayerStance == PlayerStance.Scavenger)
            {
                return;
            }

            var areaActorData = questData.ActorData
                .Where(actorData => playerQuestData.MainActorData.AreaId.HasValue && playerQuestData.MainActorData.AreaId == actorData.AreaId)
                .ToArray();
            
            var scavengerPlayer = questData.PlayerQuestData.FirstOrDefault(x => x.PlayerStance == PlayerStance.Scavenger);
            var isExistScavenger = areaActorData.Any(x => x.PlayerInstanceId == scavengerPlayer?.InstanceId);
            var isExistOtherPlayer = areaActorData.Any(x => x.PlayerInstanceId != scavengerPlayer?.InstanceId && x.PlayerInstanceId != playerQuestData.InstanceId);
            
            switch (playerQuestData.PlayerStance)
            {
                case PlayerStance.None:
                case PlayerStance.Scavenger:
                    return;
                case PlayerStance.Fugitive:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Escape);
                    return;
                case PlayerStance.ScavengerKiller:
                    if (isExistScavenger && !isExistOtherPlayer)
                    {
                        MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Combat);
                    }
                    else
                    {
                        MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Survey);
                    }
                    return;
                case PlayerStance.Collector:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Survey);
                    return;
                case PlayerStance.Hunter:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Combat);
                    return;
                case PlayerStance.Stalker:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Combat);
                    return;
                case PlayerStance.ConquestUser:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Combat);
                    return;
                case PlayerStance.FacilityUser:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Combat);
                    return;
            }
        }
        
        static void UpdateInteract(QuestData questData, PlayerQuestData playerQuestData)
        {
            if (playerQuestData.PlayerStance == PlayerStance.Scavenger)
            {
                // FIXME: ScavでもInteract出来るようにする
                return;
            }

            // var areaData = questData.StarSystemData.AreaData[playerQuestData.MainActorData.AreaId];
        }
    }
}