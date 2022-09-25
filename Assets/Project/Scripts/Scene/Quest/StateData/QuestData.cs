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

        public void RemoveActorData(ActorData actorData)
        {
            ActorData.Remove(actorData);
        }

        public void AddWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            WeaponEffectData.Add(weaponEffectData);
        }

        public void RemoveWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            WeaponEffectData.Remove(weaponEffectData);
        }
    }
}
