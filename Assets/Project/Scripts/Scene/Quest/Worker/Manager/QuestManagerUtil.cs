using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public static class QuestManagerUtil
    {
        public static void InitializePlayer(QuestData questData)
        {
            /*
            Playerの開始位置を適当に決める
            Map左下を基準に1エリアずつ開けて、マップの外周に沿って配置する
            Mapサイズが 7x14 だった場合は
            
            1, 3, 5, = 0~x(f%2==1)
            13, 41, 69, = y-1, y*3-1, y*5-1
            28, 56, 84, = y*2, y*4, y*6
            92, 94, 96, = x*(y - 1) + 0~x(f%2==1)
            */

            var spawnAreaIndexList = new List<int>();
            var xOddNumber = Enumerable.Range(0, questData.MapData.MapSizeX).Where(x => x % 2 == 1).ToArray();
            var zOddNumber = Enumerable.Range(0, questData.MapData.MapSizeZ / 2).Where(z => z % 2 == 1).ToArray();
            
            spawnAreaIndexList.AddRange(xOddNumber);
            spawnAreaIndexList.AddRange(zOddNumber.Select(z => (z * questData.MapData.MapSizeZ) - 1).ToArray());
            spawnAreaIndexList.AddRange(zOddNumber.Select(z => (z + 1) * questData.MapData.MapSizeZ).ToArray());
            spawnAreaIndexList.AddRange(xOddNumber.Select(x => questData.MapData.MapSizeX * (questData.MapData.MapSizeZ - 1) + x).ToArray());

            foreach (var spawnAreaIndex in spawnAreaIndexList)
            {
                var playerStances = new []
                {
                    PlayerStance.Fugitive,
                    PlayerStance.ScavengerKiller,
                    PlayerStance.Collector,
                    PlayerStance.Hunter,
                    PlayerStance.Stalker,
                    PlayerStance.ConquestUser,
                    PlayerStance.FacilityUser,
                };
                    
                var playerStance = playerStances[Random.Range(0, playerStances.Length)];
                
                var playerQuestData = new PlayerQuestData(questData.MapData.MapSize - 1 - spawnAreaIndex, playerStance);
                playerQuestData.SetMoveTarget(questData.MapData.AreaData.First(x => x.AreaIndex == playerQuestData.ExitAreaIndex));
                MessageBus.Instance.AddPlayerQuestData.Broadcast(playerQuestData);
                
                var actors = GetTempAddActorData(questData, playerQuestData, spawnAreaIndex, new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f)));
                foreach (var actor in actors)
                {
                    MessageBus.Instance.AddActorData.Broadcast(actor);                    
                }
                
                playerQuestData.SetMainActorData(actors.FirstOrDefault());
            }

            MessageBus.Instance.UserCommandSetObservePlayer.Broadcast(questData.PlayerQuestData.First().InstanceId);
        }

        public static void PlaceEnemyAllArea(QuestData questData)
        {
            var scavPlayer = new PlayerQuestData(null, PlayerStance.Scavenger);
            MessageBus.Instance.AddPlayerQuestData.Broadcast(scavPlayer);
            
            var enemy1BluePrint = new ActorBluePrint(1001);
            
            foreach (var areaData in questData.MapData.AreaData)
            {
                var enemy1ActorSpecData = new ActorSpecData();
                enemy1ActorSpecData.Setup(enemy1BluePrint);
                
                var actorData = new ActorData(enemy1ActorSpecData, scavPlayer.InstanceId);
                actorData.SetAreaIndex(areaData.AreaIndex);
                actorData.Position = new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));
                MessageBus.Instance.AddActorData.Broadcast(actorData);       
            }
        }

        static ActorData[] GetTempAddActorData(QuestData questData, PlayerQuestData playerQuestData, int areaIndex, Vector3 position)
        {
            var ammoItemDataVO = new ItemVO(0);
            
            return Enumerable
                .Range(0, 1)
                .Select(_ => new ActorBluePrint())
                .Select(x =>
                {
                    var spec = new ActorSpecData();
                    spec.Setup(x);
                    return spec;
                })
                .Select((x, index) =>
                {
                    var actorData = new ActorData(x, playerQuestData.InstanceId);
                    actorData.SetAreaIndex(areaIndex);
                    actorData.SetMoveTarget(questData.MapData.AreaData.First(x => x.AreaIndex == playerQuestData.ExitAreaIndex));
                    actorData.Position = position;
                    
                    var ammoItemData = new ItemData(ammoItemDataVO, 120);
                    var inventory = actorData.InventoryDataList.FirstOrDefault();
                    var insertableId = inventory?.VariableInventoryViewData.GetInsertableId(ammoItemData);
                    if (insertableId.HasValue)
                    {
                        inventory.VariableInventoryViewData.InsertInventoryItem(insertableId.Value, ammoItemData);
                    }

                    return actorData;
                })
                .ToArray();
        }
    }
}