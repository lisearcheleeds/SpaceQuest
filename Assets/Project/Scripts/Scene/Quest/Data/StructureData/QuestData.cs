using System;
using System.Collections.Generic;

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

        public IReadOnlyDictionary<Guid, PlayerData> PlayerData => playerData;
        public IReadOnlyDictionary<Guid, ActorData> ActorData => actorData;
        public IReadOnlyDictionary<Guid, WeaponEffectData> WeaponEffectData => weaponEffectData;
        public IReadOnlyDictionary<Guid, IInteractData> InteractData => interactData;

        Dictionary<Guid, PlayerData> playerData = new Dictionary<Guid, PlayerData>();
        Dictionary<Guid, ActorData> actorData = new Dictionary<Guid, ActorData>();
        Dictionary<Guid, WeaponEffectData> weaponEffectData = new Dictionary<Guid, WeaponEffectData>();
        Dictionary<Guid, IInteractData> interactData = new Dictionary<Guid, IInteractData>();

        public QuestData(StarSystemPresetVO starSystemPresetVo)
        {
            StarSystemData = new StarSystemData(starSystemPresetVo);
            UserData = new UserData();
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

        public void AddInteractData(IInteractData data)
        {
            interactData[data.InstanceId] = data;
        }

        public void RemoveInteractData(IInteractData data)
        {
            interactData.Remove(data.InstanceId);
        }
    }
}
