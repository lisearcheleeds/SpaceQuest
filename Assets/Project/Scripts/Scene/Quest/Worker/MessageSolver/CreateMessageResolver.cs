using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AloneSpace
{
    public class CreateMessageResolver
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.AddListener(CreatePlayerDataFromPresetIdAndAreaIdRandomPosition);
            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaId.AddListener(CreatePlayerDataFromPresetIdAndAreaId);
            MessageBus.Instance.CreatePlayerDataFromPresetId.AddListener(CreatePlayerDataFromPresetId);
            MessageBus.Instance.CreatePlayerDataFromPreset.AddListener(CreatePlayerDataFromPreset);
            MessageBus.Instance.ReleasePlayerData.AddListener(ReleasePlayerData);
            MessageBus.Instance.AddedPlayerData.AddListener(AddedPlayerData);
            MessageBus.Instance.RemovedPlayerData.AddListener(RemovedPlayerData);

            MessageBus.Instance.CreateActorDataFromPresetId.AddListener(CreateActorDataFromPresetId);
            MessageBus.Instance.CreateActorDataFromPreset.AddListener(CreateActorDataFromPreset);
            MessageBus.Instance.ReleaseActorData.AddListener(ReleaseActorData);
            MessageBus.Instance.AddedActorData.AddListener(AddedActorData);
            MessageBus.Instance.RemovedActorData.AddListener(RemovedActorData);

            MessageBus.Instance.CreateWeaponEffectData.AddListener(CreateWeaponEffectData);
            MessageBus.Instance.ReleaseWeaponEffectData.AddListener(ReleaseWeaponEffectData);
            MessageBus.Instance.AddedWeaponEffectData.AddListener(AddedWeaponEffectData);
            MessageBus.Instance.RemovedWeaponEffectData.AddListener(RemovedWeaponEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.RemoveListener(CreatePlayerDataFromPresetIdAndAreaIdRandomPosition);
            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaId.RemoveListener(CreatePlayerDataFromPresetIdAndAreaId);
            MessageBus.Instance.CreatePlayerDataFromPresetId.RemoveListener(CreatePlayerDataFromPresetId);
            MessageBus.Instance.CreatePlayerDataFromPreset.RemoveListener(CreatePlayerDataFromPreset);
            MessageBus.Instance.ReleasePlayerData.RemoveListener(ReleasePlayerData);
            MessageBus.Instance.AddedPlayerData.RemoveListener(AddedPlayerData);
            MessageBus.Instance.RemovedPlayerData.RemoveListener(RemovedPlayerData);

            MessageBus.Instance.CreateActorDataFromPresetId.RemoveListener(CreateActorDataFromPresetId);
            MessageBus.Instance.CreateActorDataFromPreset.RemoveListener(CreateActorDataFromPreset);
            MessageBus.Instance.ReleaseActorData.RemoveListener(ReleaseActorData);
            MessageBus.Instance.AddedActorData.RemoveListener(AddedActorData);
            MessageBus.Instance.RemovedActorData.RemoveListener(RemovedActorData);

            MessageBus.Instance.CreateWeaponEffectData.RemoveListener(CreateWeaponEffectData);
            MessageBus.Instance.ReleaseWeaponEffectData.RemoveListener(ReleaseWeaponEffectData);
            MessageBus.Instance.AddedWeaponEffectData.RemoveListener(AddedWeaponEffectData);
            MessageBus.Instance.RemovedWeaponEffectData.RemoveListener(RemovedWeaponEffectData);
        }

        void CreatePlayerDataFromPresetIdAndAreaIdRandomPosition(int playerPresetId, int areaId)
        {
            var randomOffset = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
            CreatePlayerDataFromPresetIdAndAreaId(playerPresetId, areaId, randomOffset);
        }

        void CreatePlayerDataFromPresetIdAndAreaId(int playerPresetId, int areaId, Vector3 position)
        {
            CreatePlayerDataFromPresetId(playerPresetId, questData.StarSystemData.AreaData.First(x => x.AreaId == areaId), position);
        }

        void CreatePlayerDataFromPresetId(int playerPresetId, AreaData areaData, Vector3 position)
        {
            CreatePlayerDataFromPreset(new PlayerPresetVO(playerPresetId),  areaData, position);
        }

        void CreatePlayerDataFromPreset(PlayerPresetVO playerPresetVO, AreaData areaData, Vector3 position)
        {
            var (playerQuestData, actorDataList) = QuestDataHelper.CreatePlayerData(playerPresetVO, areaData, position);

            questData.AddPlayerQuestData(playerQuestData);
            MessageBus.Instance.AddedPlayerData.Broadcast(playerQuestData);
            foreach (var actorData in actorDataList)
            {
                questData.AddActorData(actorData);
                MessageBus.Instance.AddedActorData.Broadcast(actorData);
            }
        }

        void ReleasePlayerData(PlayerQuestData playerData)
        {
            questData.RemovePlayerQuestData(playerData);
            MessageBus.Instance.RemovedPlayerData.Broadcast(playerData);
        }

        void AddedPlayerData(PlayerQuestData playerData)
        {
        }

        void RemovedPlayerData(PlayerQuestData playerData)
        {
        }

        void CreateActorDataFromPresetId(PlayerQuestData playerQuestData, int actorPresetId, AreaData areaData, Vector3 position)
        {
            CreateActorDataFromPreset(playerQuestData, new ActorPresetVO(actorPresetId), areaData, position);
        }

        void CreateActorDataFromPreset(PlayerQuestData playerQuestData, ActorPresetVO actorPresetVO, AreaData areaData, Vector3 position)
        {
            var actorData = QuestDataHelper.CreateActorData(playerQuestData, actorPresetVO, areaData, position);
            questData.AddActorData(actorData);
            MessageBus.Instance.AddedActorData.Broadcast(actorData);
        }

        void ReleaseActorData(ActorData actorData)
        {
            questData.RemoveActorData(actorData);
            MessageBus.Instance.RemovedActorData.Broadcast(actorData);
        }

        void AddedActorData(ActorData actorData)
        {
        }

        void RemovedActorData(ActorData actorData)
        {
        }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponEffectSpecVO">武器データ</param>
        /// <param name="weaponData">武器データ</param>
        /// <param name="fromPositionData">発射位置</param>
        /// <param name="rotation">方向</param>
        /// <param name="targetData">ターゲット</param>
        void CreateWeaponEffectData(IWeaponEffectSpecVO weaponEffectSpecVO, WeaponData weaponData, IPositionData fromPositionData, Quaternion rotation, IPositionData targetData)
        {
            WeaponEffectData weaponEffectData = weaponEffectSpecVO switch
            {
                BulletWeaponEffectSpecVO specVO => new BulletWeaponEffectData(specVO, weaponData, fromPositionData, rotation, targetData),
                MissileWeaponEffectSpecVO specVO => new MissileWeaponEffectData(specVO, weaponData, fromPositionData, rotation, targetData),
                ExplosionWeaponEffectSpecVO specVO => new ExplosionWeaponEffectData(specVO, weaponData, fromPositionData, rotation, targetData),
                _ => throw new NotImplementedException(),
            };

            questData.AddWeaponEffectData(weaponEffectData);
            MessageBus.Instance.AddedWeaponEffectData.Broadcast(weaponEffectData);
        }

        void ReleaseWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            questData.RemoveWeaponEffectData(weaponEffectData);
            MessageBus.Instance.RemovedWeaponEffectData.Broadcast(weaponEffectData);
        }

        void AddedWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            questData.ActorData[weaponEffectData.WeaponData.WeaponHolder.InstanceId].AddWeaponEffectData(weaponEffectData);
        }

        void RemovedWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            questData.ActorData[weaponEffectData.WeaponData.WeaponHolder.InstanceId].RemoveWeaponEffectData(weaponEffectData);
        }
    }
}
