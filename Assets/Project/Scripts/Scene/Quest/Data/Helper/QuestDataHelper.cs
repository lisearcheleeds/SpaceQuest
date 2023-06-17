using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public static class QuestDataHelper
    {
        public static (PlayerData, ActorData[]) CreatePlayerData(PlayerPresetVO playerPresetVO, Dictionary<PlayerPropertyKey, IPlayerPropertyValue> playerProperty, AreaData areaData, Vector3 position)
        {
            var playerData = new PlayerData(playerProperty);
            var actorData = playerPresetVO.ActorPresetVOs.Select(vo => CreateActorData(playerData, vo, areaData, position)).ToArray();
            return (playerData, actorData);
        }

        public static ActorData CreateActorData(PlayerData playerData, ActorPresetVO actorPresetVO, AreaData areaData, Vector3 position)
        {
            var actorData = new ActorData(actorPresetVO, playerData.InstanceId);
            actorData.SetAreaId(areaData.AreaId);
            actorData.SetPosition(areaData.SpawnPoint.Position + position);
            return actorData;
        }
    }
}
