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
            MessageBus.Instance.CreatedPlayerData.AddListener(AddedPlayerData);
            MessageBus.Instance.ReleasedPlayerData.AddListener(RemovedPlayerData);

            MessageBus.Instance.CreateActorDataFromPresetId.AddListener(CreateActorDataFromPresetId);
            MessageBus.Instance.CreateActorDataFromPreset.AddListener(CreateActorDataFromPreset);
            MessageBus.Instance.ReleaseActorData.AddListener(ReleaseActorData);
            MessageBus.Instance.CreatedActorData.AddListener(AddedActorData);
            MessageBus.Instance.ReleasedActorData.AddListener(RemovedActorData);

            MessageBus.Instance.CreateWeaponEffectData.AddListener(CreateWeaponEffectData);
            MessageBus.Instance.ReleaseWeaponEffectData.AddListener(ReleaseWeaponEffectData);
            MessageBus.Instance.CreatedWeaponEffectData.AddListener(AddedWeaponEffectData);
            MessageBus.Instance.ReleasedWeaponEffectData.AddListener(RemovedWeaponEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.RemoveListener(CreatePlayerDataFromPresetIdAndAreaIdRandomPosition);
            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaId.RemoveListener(CreatePlayerDataFromPresetIdAndAreaId);
            MessageBus.Instance.CreatePlayerDataFromPresetId.RemoveListener(CreatePlayerDataFromPresetId);
            MessageBus.Instance.CreatePlayerDataFromPreset.RemoveListener(CreatePlayerDataFromPreset);
            MessageBus.Instance.ReleasePlayerData.RemoveListener(ReleasePlayerData);
            MessageBus.Instance.CreatedPlayerData.RemoveListener(AddedPlayerData);
            MessageBus.Instance.ReleasedPlayerData.RemoveListener(RemovedPlayerData);

            MessageBus.Instance.CreateActorDataFromPresetId.RemoveListener(CreateActorDataFromPresetId);
            MessageBus.Instance.CreateActorDataFromPreset.RemoveListener(CreateActorDataFromPreset);
            MessageBus.Instance.ReleaseActorData.RemoveListener(ReleaseActorData);
            MessageBus.Instance.CreatedActorData.RemoveListener(AddedActorData);
            MessageBus.Instance.ReleasedActorData.RemoveListener(RemovedActorData);

            MessageBus.Instance.CreateWeaponEffectData.RemoveListener(CreateWeaponEffectData);
            MessageBus.Instance.ReleaseWeaponEffectData.RemoveListener(ReleaseWeaponEffectData);
            MessageBus.Instance.CreatedWeaponEffectData.RemoveListener(AddedWeaponEffectData);
            MessageBus.Instance.ReleasedWeaponEffectData.RemoveListener(RemovedWeaponEffectData);
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
            MessageBus.Instance.CreatedPlayerData.Broadcast(playerQuestData);
            foreach (var actorData in actorDataList)
            {
                MessageBus.Instance.CreatedActorData.Broadcast(actorData);
            }
        }

        void ReleasePlayerData(PlayerData playerData)
        {
            playerData.Release();
            MessageBus.Instance.ReleasedPlayerData.Broadcast(playerData);

            // TODO: ActorのRelease
        }

        void AddedPlayerData(PlayerData playerData)
        {
        }

        void RemovedPlayerData(PlayerData playerData)
        {
        }

        void CreateActorDataFromPresetId(PlayerData playerData, int actorPresetId, AreaData areaData, Vector3 position)
        {
            CreateActorDataFromPreset(playerData, new ActorPresetVO(actorPresetId), areaData, position);
        }

        void CreateActorDataFromPreset(PlayerData playerData, ActorPresetVO actorPresetVO, AreaData areaData, Vector3 position)
        {
            var actorData = QuestDataHelper.CreateActorData(playerData, actorPresetVO, areaData, position);
            MessageBus.Instance.CreatedActorData.Broadcast(actorData);
        }

        void ReleaseActorData(ActorData actorData)
        {
            actorData.Release();
            MessageBus.Instance.ReleasedActorData.Broadcast(actorData);
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

            MessageBus.Instance.CreatedWeaponEffectData.Broadcast(weaponEffectData);
        }

        void ReleaseWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            weaponEffectData.Release();
            MessageBus.Instance.ReleasedWeaponEffectData.Broadcast(weaponEffectData);
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
