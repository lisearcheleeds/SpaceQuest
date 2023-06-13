using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AloneSpace
{
    /// <summary>
    /// QuestData
    /// Initializeのみで渡される
    /// </summary>
    public class QuestData
    {
        public StarSystemData StarSystemData { get; }

        public UserData UserData { get; }
        public ReadOnlyDictionary<Guid, PlayerData> PlayerData { get; }
        public ReadOnlyDictionary<Guid, ActorData> ActorData { get; }
        public ReadOnlyDictionary<Guid, WeaponEffectData> WeaponEffectData { get; }

        Dictionary<Guid, PlayerData> playerData;
        Dictionary<Guid, ActorData> actorData;
        Dictionary<Guid, WeaponEffectData> weaponEffectData;

        public QuestData(StarSystemPresetVO starSystemPresetVo)
        {
            StarSystemData = new StarSystemData(starSystemPresetVo);
            UserData = new UserData();

            playerData = new Dictionary<Guid, PlayerData>();
            actorData = new Dictionary<Guid, ActorData>();
            weaponEffectData = new Dictionary<Guid, WeaponEffectData>();

            PlayerData = new ReadOnlyDictionary<Guid, PlayerData>(playerData);
            ActorData = new ReadOnlyDictionary<Guid, ActorData>(actorData);
            WeaponEffectData = new ReadOnlyDictionary<Guid, WeaponEffectData>(weaponEffectData);
        }

        public void AddPlayerData(PlayerData　data)
        {
            playerData[data.InstanceId] = data;
        }

        public void RemovePlayerData(PlayerData　data)
        {
            playerData.Remove(data.InstanceId);
        }

        public void AddActorData(ActorData　data)
        {
            actorData[data.InstanceId] = data;
        }

        public void RemoveActorData(ActorData　data)
        {
            actorData.Remove(data.InstanceId);
        }

        public void AddWeaponEffectData(WeaponEffectData　data)
        {
            weaponEffectData[data.InstanceId] = data;
        }

        public void RemoveWeaponEffectData(WeaponEffectData　data)
        {
            weaponEffectData.Remove(data.InstanceId);
        }
    }
}
