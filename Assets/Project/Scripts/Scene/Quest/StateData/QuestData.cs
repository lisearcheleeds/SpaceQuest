using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RoboQuest.Quest
{
    public class QuestData
    {
        public MapData MapData { get; }
        public List<PlayerQuestData> PlayerQuestData { get; private set; } = new List<PlayerQuestData>();
        public List<ActorData> ActorData { get; private set; } = new List<ActorData>();

        public PlayerQuestData ObservePlayer => PlayerQuestData.First(x => x.InstanceId == observePlayerId);
        public ActorData ObserveActor => ActorData.First(x => x.InstanceId == observeActorId);
        public ActorData[] ObservePlayerActors => ActorData.Where(x => ObservePlayer.InstanceId == x.PlayerQuestDataInstanceId).ToArray();

        Guid observePlayerId;
        Guid observeActorId;
        
        public QuestData(MapPresetVO mapPresetVO)
        {
            MapData = new MapData(mapPresetVO);
            InitializePlayer();
            PlaceEnemyAllArea();
        }

        public void UserCommandSetObservePlayer(Guid playerInstanceId)
        {
            observePlayerId = playerInstanceId;
        }
        
        public void UserCommandSetObserveActor(Guid actorInstanceId)
        {
            observeActorId = actorInstanceId;
        }
        
        void InitializePlayer()
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
            var xOddNumber = Enumerable.Range(0, MapData.MapSizeX).Where(x => x % 2 == 1).ToArray();
            var yOddNumber = Enumerable.Range(0, MapData.MapSizeY / 2).Where(y => y % 2 == 1).ToArray();
            
            spawnAreaIndexList.AddRange(xOddNumber);
            spawnAreaIndexList.AddRange(yOddNumber.Select(y => (y * MapData.MapSizeY) - 1).ToArray());
            spawnAreaIndexList.AddRange(yOddNumber.Select(y => (y + 1) * MapData.MapSizeY).ToArray());
            spawnAreaIndexList.AddRange(xOddNumber.Select(x => MapData.MapSizeX * (MapData.MapSizeY - 1) + x).ToArray());

            foreach (var spawnAreaIndex in spawnAreaIndexList)
            {
                var playerStance = PlayerStance.None;
                if (PlayerQuestData.Count != 0)
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
                    
                    playerStance = playerStances[Random.Range(0, playerStances.Length)];
                }
                
                var playerQuestData = new PlayerQuestData(MapData.MapSize - 1 - spawnAreaIndex, playerStance);
                playerQuestData.SetDestinateAreaIndex(playerQuestData.ExitAreaIndex);
                PlayerQuestData.Add(playerQuestData);
                
                var actors = GetTempAddActorData(playerQuestData, spawnAreaIndex, new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f)));
                ActorData.AddRange(actors);
                playerQuestData.SetMainActorData(actors.FirstOrDefault());
            }

            var userPlayerQuestData = PlayerQuestData.First();
            UserCommandSetObservePlayer(userPlayerQuestData.InstanceId);
            UserCommandSetObserveActor(userPlayerQuestData.MainActorData.InstanceId);
        }

        void PlaceEnemyAllArea()
        {
            var scavPlayer = new PlayerQuestData(null, PlayerStance.Scavenger);
            PlayerQuestData.Add(scavPlayer);
            
            var enemy1BluePrint = new ActorBluePrint(1001);
            
            foreach (var areaData in MapData.AreaData)
            {
                var enemy1ActorSpecData = new ActorSpecData();
                enemy1ActorSpecData.Setup(enemy1BluePrint);
                
                var actorData = new ActorData(enemy1ActorSpecData, scavPlayer.InstanceId, MapData);
                actorData.SetCurrentAreaIndex(areaData.Index);
                actorData.SetPosition(new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f)));
                ActorData.Add(actorData);
            }
        }

        ActorData[] GetTempAddActorData(PlayerQuestData playerQuestData, int areaIndex, Vector3 position)
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
                    var actorData = new ActorData(x, playerQuestData.InstanceId, MapData);
                    actorData.SetCurrentAreaIndex(areaIndex);
                    actorData.SetDestinateAreaIndex(playerQuestData.DestinateAreaIndex);
                    actorData.SetPosition(position);
                    
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
