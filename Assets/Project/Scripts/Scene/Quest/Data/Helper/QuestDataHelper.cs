using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public static class QuestDataHelper
    {
        public static (PlayerData, ActorData[]) CreatePlayerData(PlayerPresetVO playerPresetVO, AreaData areaData, Vector3 position)
        {
            var playerQuestData = new PlayerData();
            var actorData = playerPresetVO.ActorPresetVOs.Select(vo => CreateActorData(playerQuestData, vo, areaData, position)).ToArray();
            playerQuestData.SetMainActorData(actorData.First());
            return (playerQuestData, actorData);
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
