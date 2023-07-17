using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AloneSpace
{
    public class CreateDataController
    {
        QuestData questData;

        List<PlayerData> createPlayerDataList = new List<PlayerData>();
        List<ActorData> createActorDataList = new List<ActorData>();
        List<WeaponEffectData> createWeaponEffectDataList = new List<WeaponEffectData>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.AddListener(CreatePlayerDataFromPresetIdAndAreaIdRandomPosition);
            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaId.AddListener(CreatePlayerDataFromPresetIdAndAreaId);
            MessageBus.Instance.CreatePlayerDataFromPresetId.AddListener(CreatePlayerDataFromPresetId);
            MessageBus.Instance.CreatePlayerDataFromPreset.AddListener(CreatePlayerDataFromPreset);

            MessageBus.Instance.CreateActorDataFromPresetId.AddListener(CreateActorDataFromPresetId);
            MessageBus.Instance.CreateActorDataFromPreset.AddListener(CreateActorDataFromPreset);

            MessageBus.Instance.CreateWeaponEffectData.AddListener(CreateWeaponEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.RemoveListener(CreatePlayerDataFromPresetIdAndAreaIdRandomPosition);
            MessageBus.Instance.CreatePlayerDataFromPresetIdAndAreaId.RemoveListener(CreatePlayerDataFromPresetIdAndAreaId);
            MessageBus.Instance.CreatePlayerDataFromPresetId.RemoveListener(CreatePlayerDataFromPresetId);
            MessageBus.Instance.CreatePlayerDataFromPreset.RemoveListener(CreatePlayerDataFromPreset);

            MessageBus.Instance.CreateActorDataFromPresetId.RemoveListener(CreateActorDataFromPresetId);
            MessageBus.Instance.CreateActorDataFromPreset.RemoveListener(CreateActorDataFromPreset);

            MessageBus.Instance.CreateWeaponEffectData.RemoveListener(CreateWeaponEffectData);
        }

        public void OnUpdate(float deltaTime)
        {
            if (questData == null)
            {
                return;
            }

            foreach (var playerData in createPlayerDataList)
            {
                playerData.ActivateModules();
                questData.AddPlayerData(playerData);

                MessageBus.Instance.CreatedPlayerData.Broadcast(playerData);
            }

            createPlayerDataList.Clear();

            foreach (var actorData in createActorDataList)
            {
                actorData.ResetState();
                actorData.ActivateModules();

                questData.AddActorData(actorData);

                // ありえないと思うけど一応合わせる
                if (questData.PlayerData.ContainsKey(actorData.PlayerInstanceId))
                {
                    questData.PlayerData[actorData.PlayerInstanceId].AddActorData(actorData);
                }

                MessageBus.Instance.CreatedActorData.Broadcast(actorData);
            }

            createActorDataList.Clear();

            foreach (var weaponEffectData in createWeaponEffectDataList)
            {
                weaponEffectData.ActivateModules();
                questData.AddWeaponEffectData(weaponEffectData);

                // 最後っ屁を除く
                if (questData.ActorData.ContainsKey(weaponEffectData.WeaponData.WeaponHolder.InstanceId))
                {
                    questData.ActorData[weaponEffectData.WeaponData.WeaponHolder.InstanceId].AddWeaponEffectData(weaponEffectData);
                }

                MessageBus.Instance.CreatedWeaponEffectData.Broadcast(weaponEffectData);
            }

            createWeaponEffectDataList.Clear();
        }

        void CreatePlayerDataFromPresetIdAndAreaIdRandomPosition(int playerPresetId, Dictionary<PlayerPropertyKey, IPlayerPropertyValue> playerProperty, int areaId)
        {
            var randomOffset = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f)) * 10.0f;
            CreatePlayerDataFromPresetIdAndAreaId(playerPresetId, playerProperty, areaId, randomOffset);
        }

        void CreatePlayerDataFromPresetIdAndAreaId(int playerPresetId, Dictionary<PlayerPropertyKey, IPlayerPropertyValue> playerProperty, int areaId, Vector3 position)
        {
            CreatePlayerDataFromPresetId(playerPresetId, playerProperty, questData.StarSystemData.AreaData.First(x => x.AreaId == areaId), position);
        }

        void CreatePlayerDataFromPresetId(int playerPresetId, Dictionary<PlayerPropertyKey, IPlayerPropertyValue> playerProperty, AreaData areaData, Vector3 position)
        {
            CreatePlayerDataFromPreset(new PlayerPresetVO(playerPresetId), playerProperty,  areaData, position);
        }

        void CreatePlayerDataFromPreset(PlayerPresetVO playerPresetVO, Dictionary<PlayerPropertyKey, IPlayerPropertyValue> playerProperty, AreaData areaData, Vector3 position)
        {
            var (playerData, actorDataList) = QuestDataHelper.CreatePlayerData(playerPresetVO, playerProperty, areaData, position);
            createPlayerDataList.Add(playerData);

            foreach (var actorData in actorDataList)
            {
                createActorDataList.Add(actorData);
            }
        }

        void CreateActorDataFromPresetId(PlayerData playerData, int actorPresetId, AreaData areaData, Vector3 position)
        {
            CreateActorDataFromPreset(playerData, new ActorPresetVO(actorPresetId), areaData, position);
        }

        void CreateActorDataFromPreset(PlayerData playerData, ActorPresetVO actorPresetVO, AreaData areaData, Vector3 position)
        {
            var actorData = QuestDataHelper.CreateActorData(playerData, actorPresetVO, areaData, position);
            createActorDataList.Add(actorData);
        }

        void CreateWeaponEffectData(
            IWeaponEffectSpecVO weaponEffectSpecVO,
            IWeaponEffectCreateOptionData weaponEffectCreateOptionData)
        {
            WeaponEffectData weaponEffectData = weaponEffectSpecVO switch
            {
                BulletWeaponEffectSpecVO specVO => new BulletWeaponEffectData(specVO, (BulletWeaponEffectCreateOptionData)weaponEffectCreateOptionData),
                ParticleBulletWeaponEffectSpecVO specVO => new ParticleBulletWeaponEffectData(specVO, (ParticleBulletWeaponEffectCreateOptionData)weaponEffectCreateOptionData),
                MissileWeaponEffectSpecVO specVO => new MissileWeaponEffectData(specVO, (MissileWeaponEffectCreateOptionData)weaponEffectCreateOptionData),
                ExplosionWeaponEffectSpecVO specVO => new ExplosionWeaponEffectData(specVO, (ExplosionWeaponEffectCreateOptionData)weaponEffectCreateOptionData),
                _ => throw new NotImplementedException(),
            };

            createWeaponEffectDataList.Add(weaponEffectData);
        }
    }
}
