using System;
using System.Linq;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using AloneSpace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AloneSpace
{
    public class QuestData
    {
        public MapData MapData { get; }

        public List<PlayerQuestData> PlayerQuestData { get; } = new List<PlayerQuestData>();
        public List<ActorData> ActorData { get; } = new List<ActorData>();
        public List<WeaponEffectData> WeaponEffectData { get; } = new List<WeaponEffectData>();

        public (AreaDirection? AreaDirection, AreaData AreaData)[] ObserveAdjacentAreaData { get; private set; }
        
        public PlayerQuestData ObservePlayer => PlayerQuestData.First(x => x.InstanceId == observePlayerId);
        public ActorData ObserveActor => ActorData.First(x => x.InstanceId == observeActorId);
        public ActorData[] ObservePlayerActors => ActorData.Where(x => ObservePlayer.InstanceId == x.PlayerInstanceId).ToArray();

        public int ObserveAreaIndex { get; private set; }
        Guid observePlayerId;
        Guid observeActorId;
        
        public QuestData(MapPresetVO mapPresetVO)
        {
            MapData = new MapData(mapPresetVO);
            // PlaceEnemyAllArea();
        }

        public void UserCommandSetObservePlayer(Guid playerInstanceId)
        {
            observePlayerId = playerInstanceId;
        }
        
        public void UserCommandSetObserveActor(Guid actorInstanceId)
        {
            observeActorId = actorInstanceId;
        }

        public void SetObserveArea(int observeAreaIndex)
        {
            ObserveAreaIndex = observeAreaIndex;

            var adjacentAreaIndexes = MapData.AreaData[observeAreaIndex].AdjacentAreaIndexes
                .Select(x => ((AreaDirection?) x.AreaDirection, x.Index))
                .Concat(new (AreaDirection? AreaDirection, int Index)[] {(null, observeAreaIndex)});

            ObserveAdjacentAreaData = adjacentAreaIndexes.Select(x => (x.Item1, MapData.AreaData[x.Index])).ToArray();
        }

        public void AddPlayerQuestData(PlayerQuestData playerQuestData)
        {
            PlayerQuestData.Add(playerQuestData);
        }

        public void AddActorData(ActorData actorData)
        {
            ActorData.Add(actorData);
        }

        public void AddWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            WeaponEffectData.Add(weaponEffectData);
        }

        public void InitializePlayer()
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
            var zOddNumber = Enumerable.Range(0, MapData.MapSizeZ / 2).Where(z => z % 2 == 1).ToArray();
            
            spawnAreaIndexList.AddRange(xOddNumber);
            spawnAreaIndexList.AddRange(zOddNumber.Select(z => (z * MapData.MapSizeZ) - 1).ToArray());
            spawnAreaIndexList.AddRange(zOddNumber.Select(z => (z + 1) * MapData.MapSizeZ).ToArray());
            spawnAreaIndexList.AddRange(xOddNumber.Select(x => MapData.MapSizeX * (MapData.MapSizeZ - 1) + x).ToArray());

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
                playerQuestData.SetMoveTarget(MapData.AreaData.First(x => x.AreaIndex == playerQuestData.ExitAreaIndex));
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
                
                var actorData = new ActorData(enemy1ActorSpecData, scavPlayer.InstanceId);
                actorData.SetAreaIndex(areaData.AreaIndex);
                actorData.Position = new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));
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
                    var actorData = new ActorData(x, playerQuestData.InstanceId);
                    actorData.SetAreaIndex(areaIndex);
                    actorData.SetMoveTarget(MapData.AreaData.First(x => x.AreaIndex == playerQuestData.ExitAreaIndex));
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
