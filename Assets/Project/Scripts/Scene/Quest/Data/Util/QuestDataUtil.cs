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
                .Select((playerQuestData, i) =>
                {
                    var actorData = GetTempAddActorData(
                        playerQuestData,
                        // areaData[Random.Range(0, areaData.Length)],
                        areaData[0],
                        i != 1 ? 1 : 5,
                        new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f)));

                    playerQuestData.SetMainActorData(actorData);

                    return actorData;
                }).ToArray();

            return (playerQuestDataList, actorDataList);
        }

        static ActorData GetTempAddActorData(PlayerQuestData playerQuestData, AreaData areaData, int actorId, Vector3 position)
        {
            IWeaponSpecVO[] weapons;

            if (actorId == 5)
            {
                weapons = Enumerable.Range(0, 10).Select(_ => new WeaponBulletMakerSpecVO(1)).ToArray();
            }
            else
            {
                weapons = new IWeaponSpecVO[] {new WeaponBulletMakerSpecVO(1), new WeaponBulletMakerSpecVO(1), new WeaponMissileMakerSpecVO(2)};
            }

            var actorData = new ActorData(new ActorSpecVO(actorId), weapons, playerQuestData.InstanceId);
            MessageBus.Instance.PlayerCommandSetAreaId.Broadcast(actorData, areaData.AreaId);
            actorData.SetPosition(areaData.SpawnPoint.Position + position);
            return actorData;
        }
    }
}