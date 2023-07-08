using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class ReleaseDataController
    {
        QuestData questData;

        List<PlayerData> releasePlayerDataList = new List<PlayerData>();
        List<ActorData> releaseActorDataList = new List<ActorData>();
        List<WeaponEffectData> releaseWeaponEffectDataList = new List<WeaponEffectData>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.ReleasePlayerData.AddListener(ReleasePlayerData);
            MessageBus.Instance.ReleaseActorData.AddListener(ReleaseActorData);
            MessageBus.Instance.ReleaseWeaponEffectData.AddListener(ReleaseWeaponEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.ReleasePlayerData.RemoveListener(ReleasePlayerData);
            MessageBus.Instance.ReleaseActorData.RemoveListener(ReleaseActorData);
            MessageBus.Instance.ReleaseWeaponEffectData.RemoveListener(ReleaseWeaponEffectData);
        }

        public void OnUpdate(float deltaTime)
        {
            if (questData == null)
            {
                return;
            }

            foreach (var playerData in releasePlayerDataList)
            {
                playerData.DeactivateModules();
                questData.RemovePlayerData(playerData);

                MessageBus.Instance.ReleasedPlayerData.Broadcast(playerData);
            }

            releasePlayerDataList.Clear();

            foreach (var actorData in releaseActorDataList)
            {
                actorData.DeactivateModules();
                questData.RemoveActorData(actorData);

                if (questData.PlayerData.ContainsKey(actorData.PlayerInstanceId))
                {
                    questData.PlayerData[actorData.PlayerInstanceId].RemoveActorData(actorData);
                }

                MessageBus.Instance.ReleasedActorData.Broadcast(actorData);
            }

            releaseActorDataList.Clear();

            foreach (var weaponEffectData in releaseWeaponEffectDataList)
            {
                weaponEffectData.DeactivateModules();
                questData.RemoveWeaponEffectData(weaponEffectData);

                if (questData.ActorData.ContainsKey(weaponEffectData.WeaponData.WeaponHolder.InstanceId))
                {
                    questData.ActorData[weaponEffectData.WeaponData.WeaponHolder.InstanceId].RemoveWeaponEffectData(weaponEffectData);
                }

                MessageBus.Instance.ReleasedWeaponEffectData.Broadcast(weaponEffectData);
            }

            releaseWeaponEffectDataList.Clear();
        }

        void ReleasePlayerData(PlayerData playerData)
        {
            playerData.Release();
            releasePlayerDataList.Add(playerData);

            // TODO: ActorのRelease
        }

        void ReleaseActorData(ActorData actorData)
        {
            actorData.Release();
            releaseActorDataList.Add(actorData);
        }

        void ReleaseWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            weaponEffectData.Release();
            releaseWeaponEffectDataList.Add(weaponEffectData);
        }
    }
}
