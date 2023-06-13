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
        public ReadOnlyDictionary<Guid, PlayerQuestData> PlayerQuestData { get; }
        public ReadOnlyDictionary<Guid, ActorData> ActorData { get; }
        public ReadOnlyDictionary<Guid, WeaponEffectData> WeaponEffectData { get; }

        Dictionary<Guid, PlayerQuestData> playerQuestData;
        Dictionary<Guid, ActorData> actorData;
        Dictionary<Guid, WeaponEffectData> weaponEffectData;

        public QuestData(StarSystemPresetVO starSystemPresetVo)
        {
            StarSystemData = new StarSystemData(starSystemPresetVo);
            UserData = new UserData();

            playerQuestData = new Dictionary<Guid, PlayerQuestData>();
            actorData = new Dictionary<Guid, ActorData>();
            weaponEffectData = new Dictionary<Guid, WeaponEffectData>();

            PlayerQuestData = new ReadOnlyDictionary<Guid, PlayerQuestData>(playerQuestData);
            ActorData = new ReadOnlyDictionary<Guid, ActorData>(actorData);
            WeaponEffectData = new ReadOnlyDictionary<Guid, WeaponEffectData>(weaponEffectData);
        }

        public void AddPlayerQuestData(PlayerQuestData　data)
        {
            playerQuestData[data.InstanceId] = data;
        }

        public void RemovePlayerQuestData(PlayerQuestData　data)
        {
            playerQuestData.Remove(data.InstanceId);
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
