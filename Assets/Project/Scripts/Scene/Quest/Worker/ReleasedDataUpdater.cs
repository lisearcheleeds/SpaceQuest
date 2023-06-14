using System.Collections.Generic;

namespace AloneSpace
{
    public class ReleasedDataUpdater
    {
        QuestData questData;

        List<PlayerData> releasedPlayerDataList = new List<PlayerData>();
        List<ActorData> releasedActorDataList = new List<ActorData>();
        List<WeaponEffectData> releasedWeaponEffectDataList = new List<WeaponEffectData>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.ReleasedPlayerData.AddListener(ReleasedPlayerData);
            MessageBus.Instance.ReleasedActorData.AddListener(ReleasedActorData);
            MessageBus.Instance.ReleasedWeaponEffectData.AddListener(ReleasedWeaponEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.ReleasedPlayerData.RemoveListener(ReleasedPlayerData);
            MessageBus.Instance.ReleasedActorData.RemoveListener(ReleasedActorData);
            MessageBus.Instance.ReleasedWeaponEffectData.RemoveListener(ReleasedWeaponEffectData);
        }

        public void OnLateUpdate(float deltaTime)
        {
            if (questData == null)
            {
                return;
            }

            foreach (var playerData in releasedPlayerDataList)
            {
                playerData.DeactivateModules();
                questData.RemovePlayerData(playerData);
            }

            releasedPlayerDataList.Clear();

            foreach (var actorData in releasedActorDataList)
            {
                actorData.DeactivateModules();
                questData.RemoveActorData(actorData);
            }

            releasedActorDataList.Clear();

            foreach (var weaponEffectData in releasedWeaponEffectDataList)
            {
                weaponEffectData.DeactivateModules();
                questData.RemoveWeaponEffectData(weaponEffectData);
            }

            releasedWeaponEffectDataList.Clear();
        }

        void ReleasedPlayerData(PlayerData playerData)
        {
            releasedPlayerDataList.Add(playerData);
        }

        void ReleasedActorData(ActorData actorData)
        {
            releasedActorDataList.Add(actorData);
        }

        void ReleasedWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            releasedWeaponEffectDataList.Add(weaponEffectData);
        }
    }
}
