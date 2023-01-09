using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public static class QuestDataUtil
    {
        public static (PlayerQuestData[], ActorData[]) GetRandomPlayerDataList(int playerCount, AreaData[] areaData)
        {
            var playerQuestDataList = Enumerable.Range(0, playerCount).Select(_ => new PlayerQuestData()).ToArray();
            var actorDataList = playerQuestDataList
                .Select(playerQuestData =>
                {
                    var actorData = GetTempAddActorData(
                        playerQuestData,
                        areaData[Random.Range(0, areaData.Length)],
                        new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f)));
                    
                    playerQuestData.SetMainActorData(actorData);
                    
                    return actorData;
                }).ToArray();
            
            return (playerQuestDataList, actorDataList);
        }

        static ActorData GetTempAddActorData(PlayerQuestData playerQuestData, AreaData areaData, Vector3 position)
        {
            var bp = new ActorBluePrint();
            var specData = new ActorSpecData();
            specData.Setup(bp);
            
            var actorData = new ActorData(specData, playerQuestData.InstanceId);
            MessageBus.Instance.UtilGetStarSystemPosition.Broadcast(areaData.SpawnPoint, starSystemPosition => actorData.Position = starSystemPosition);
            actorData.SetMoveTarget(areaData.SpawnPoint);
            return actorData;
        }
    }
}