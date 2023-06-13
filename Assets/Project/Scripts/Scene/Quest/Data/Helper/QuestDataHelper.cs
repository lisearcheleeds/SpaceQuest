using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public static class QuestDataHelper
    {
        public static (PlayerQuestData, ActorData[]) CreatePlayerData(PlayerPresetVO playerPresetVO, AreaData areaData, Vector3 position)
        {
            var playerQuestData = new PlayerQuestData();
            var actorData = playerPresetVO.ActorPresetVOs.Select(vo => CreateActorData(playerQuestData, vo, areaData, position)).ToArray();
            playerQuestData.SetMainActorData(actorData.First());
            return (playerQuestData, actorData);
        }

        public static ActorData CreateActorData(PlayerQuestData playerQuestData, ActorPresetVO actorPresetVO, AreaData areaData, Vector3 position)
        {
            var actorData = new ActorData(actorPresetVO, playerQuestData.InstanceId);
            actorData.SetAreaId(areaData.AreaId);
            actorData.SetPosition(areaData.SpawnPoint.Position + position);
            return actorData;
        }
    }
}
