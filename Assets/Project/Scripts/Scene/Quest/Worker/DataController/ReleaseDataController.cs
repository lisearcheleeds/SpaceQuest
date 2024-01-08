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
        List<IInteractData> releaseInteractDataList = new List<IInteractData>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.Creator.ReleasePlayerData.AddListener(ReleasePlayerData);
            MessageBus.Instance.Creator.ReleaseActorData.AddListener(ReleaseActorData);
            MessageBus.Instance.Creator.ReleaseWeaponEffectData.AddListener(ReleaseWeaponEffectData);
            MessageBus.Instance.Creator.ReleaseInteractData.AddListener(ReleaseInteractData);
        }

        public void Finalize()
        {
            MessageBus.Instance.Creator.ReleasePlayerData.RemoveListener(ReleasePlayerData);
            MessageBus.Instance.Creator.ReleaseActorData.RemoveListener(ReleaseActorData);
            MessageBus.Instance.Creator.ReleaseWeaponEffectData.RemoveListener(ReleaseWeaponEffectData);
            MessageBus.Instance.Creator.ReleaseInteractData.RemoveListener(ReleaseInteractData);
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

                MessageBus.Instance.Creator.OnReleasePlayerData.Broadcast(playerData);
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

                MessageBus.Instance.Creator.OnReleaseActorData.Broadcast(actorData);
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

                MessageBus.Instance.Creator.OnReleaseWeaponEffectData.Broadcast(weaponEffectData);
            }

            releaseWeaponEffectDataList.Clear();

            foreach (var releaseInteractData in releaseInteractDataList)
            {
                releaseInteractData.DeactivateModules();
                questData.RemoveInteractData(releaseInteractData);
                MessageBus.Instance.Creator.OnReleaseInteractData.Broadcast(releaseInteractData);
            }

            releaseInteractDataList.Clear();
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

        void ReleaseInteractData(IInteractData interactData)
        {
            releaseInteractDataList.Add(interactData);
        }
    }
}
