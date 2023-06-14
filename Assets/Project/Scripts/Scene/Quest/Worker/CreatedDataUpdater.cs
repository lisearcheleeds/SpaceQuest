using System.Collections.Generic;

namespace AloneSpace
{
    public class CreatedDataUpdater
    {
        QuestData questData;

        List<PlayerData> createdPlayerDataList = new List<PlayerData>();
        List<ActorData> createdActorDataList = new List<ActorData>();
        List<WeaponEffectData> createdWeaponEffectDataList = new List<WeaponEffectData>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.CreatedPlayerData.AddListener(CreatedPlayerData);
            MessageBus.Instance.CreatedActorData.AddListener(CreatedActorData);
            MessageBus.Instance.CreatedWeaponEffectData.AddListener(CreatedWeaponEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.CreatedPlayerData.RemoveListener(CreatedPlayerData);
            MessageBus.Instance.CreatedActorData.RemoveListener(CreatedActorData);
            MessageBus.Instance.CreatedWeaponEffectData.RemoveListener(CreatedWeaponEffectData);
        }

        public void OnLateUpdate(float deltaTime)
        {
            if (questData == null)
            {
                return;
            }

            foreach (var playerData in createdPlayerDataList)
            {
                playerData.ActivateModules();
                questData.AddPlayerData(playerData);
            }

            createdPlayerDataList.Clear();

            foreach (var actorData in createdActorDataList)
            {
                actorData.ActivateModules();
                questData.AddActorData(actorData);
            }

            createdActorDataList.Clear();

            foreach (var weaponEffectData in createdWeaponEffectDataList)
            {
                weaponEffectData.ActivateModules();
                questData.AddWeaponEffectData(weaponEffectData);
            }

            createdWeaponEffectDataList.Clear();
        }

        void CreatedPlayerData(PlayerData playerData)
        {
            createdPlayerDataList.Add(playerData);
        }

        void CreatedActorData(ActorData actorData)
        {
            createdActorDataList.Add(actorData);
        }

        void CreatedWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            createdWeaponEffectDataList.Add(weaponEffectData);
        }
    }
}
