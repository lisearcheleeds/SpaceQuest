﻿using System;
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
        List<IInteractData> createInteractDataList = new List<IInteractData>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.Data.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.AddListener(CreatePlayerDataFromPresetIdAndAreaIdRandomPosition);
            MessageBus.Instance.Data.CreatePlayerDataFromPresetIdAndAreaId.AddListener(CreatePlayerDataFromPresetIdAndAreaId);
            MessageBus.Instance.Data.CreatePlayerDataFromPresetId.AddListener(CreatePlayerDataFromPresetId);
            MessageBus.Instance.Data.CreatePlayerDataFromPreset.AddListener(CreatePlayerDataFromPreset);

            MessageBus.Instance.Data.CreateActorDataFromPresetId.AddListener(CreateActorDataFromPresetId);
            MessageBus.Instance.Data.CreateActorDataFromPreset.AddListener(CreateActorDataFromPreset);

            MessageBus.Instance.Data.CreateWeaponEffectData.AddListener(CreateWeaponEffectData);

            MessageBus.Instance.Data.CreateAreaInteractData.AddListener(CreateAreaInteractData);
            MessageBus.Instance.Data.CreateInventoryInteractData.AddListener(CreateInventoryInteractData);
            MessageBus.Instance.Data.CreateItemInteractData.AddListener(CreateItemInteractData);
        }

        public void Finalize()
        {
            MessageBus.Instance.Data.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition.RemoveListener(CreatePlayerDataFromPresetIdAndAreaIdRandomPosition);
            MessageBus.Instance.Data.CreatePlayerDataFromPresetIdAndAreaId.RemoveListener(CreatePlayerDataFromPresetIdAndAreaId);
            MessageBus.Instance.Data.CreatePlayerDataFromPresetId.RemoveListener(CreatePlayerDataFromPresetId);
            MessageBus.Instance.Data.CreatePlayerDataFromPreset.RemoveListener(CreatePlayerDataFromPreset);

            MessageBus.Instance.Data.CreateActorDataFromPresetId.RemoveListener(CreateActorDataFromPresetId);
            MessageBus.Instance.Data.CreateActorDataFromPreset.RemoveListener(CreateActorDataFromPreset);

            MessageBus.Instance.Data.CreateWeaponEffectData.RemoveListener(CreateWeaponEffectData);

            MessageBus.Instance.Data.CreateAreaInteractData.RemoveListener(CreateAreaInteractData);
            MessageBus.Instance.Data.CreateInventoryInteractData.RemoveListener(CreateInventoryInteractData);
            MessageBus.Instance.Data.CreateItemInteractData.RemoveListener(CreateItemInteractData);
        }

        public void OnUpdate(float deltaTime)
        {
            if (questData == null)
            {
                return;
            }

            // Player
            foreach (var playerData in createPlayerDataList)
            {
                playerData.ActivateModules();
                questData.AddPlayerData(playerData);

                MessageBus.Instance.Data.OnCreatePlayerData.Broadcast(playerData);
            }

            createPlayerDataList.Clear();

            // Actor
            foreach (var actorData in createActorDataList)
            {
                actorData.ActivateModules();
                questData.AddActorData(actorData);

                // ありえないと思うけど一応合わせる
                if (questData.PlayerData.ContainsKey(actorData.PlayerInstanceId))
                {
                    questData.PlayerData[actorData.PlayerInstanceId].AddActorData(actorData);
                }

                MessageBus.Instance.Data.OnCreateActorData.Broadcast(actorData);
            }

            createActorDataList.Clear();

            // WeaponEffect
            foreach (var weaponEffectData in createWeaponEffectDataList)
            {
                weaponEffectData.ActivateModules();
                questData.AddWeaponEffectData(weaponEffectData);

                // 最後っ屁を除く
                if (questData.ActorData.ContainsKey(weaponEffectData.WeaponData.WeaponHolder.InstanceId))
                {
                    questData.ActorData[weaponEffectData.WeaponData.WeaponHolder.InstanceId].AddWeaponEffectData(weaponEffectData);
                }

                MessageBus.Instance.Data.OnCreateWeaponEffectData.Broadcast(weaponEffectData);
            }

            createWeaponEffectDataList.Clear();

            foreach (var createInteractData in createInteractDataList)
            {
                createInteractData.ActivateModules();
                questData.AddInteractData(createInteractData);
                MessageBus.Instance.Data.OnCreateInteractData.Broadcast(createInteractData);
            }

            createInteractDataList.Clear();
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
            var (playerData, actorDataList) = CreatePlayerData(playerPresetVO, playerProperty, areaData, position);
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
            var actorData = CreateActorData(playerData, actorPresetVO, areaData, position);
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

        void CreateAreaInteractData(AreaData areaData, Vector3 fromStaySystemPosition)
        {
            createInteractDataList.Add(new AreaInteractData(areaData, fromStaySystemPosition));
        }

        void CreateInventoryInteractData(InventoryData[] inventoryData, int areaId, Vector3 position, Quaternion rotation)
        {
            createInteractDataList.Add(new InventoryInteractData(inventoryData, areaId, position, rotation));
        }

        void CreateItemInteractData(ItemData itemData, int areaId, Vector3 position, Quaternion rotation)
        {
            createInteractDataList.Add(new ItemInteractData(itemData, areaId, position, rotation));
        }

        static (PlayerData, ActorData[]) CreatePlayerData(PlayerPresetVO playerPresetVO, Dictionary<PlayerPropertyKey, IPlayerPropertyValue> playerProperty, AreaData areaData, Vector3 position)
        {
            var playerData = new PlayerData(playerProperty);
            var actorData = playerPresetVO.ActorPresetVOs.Select(vo => CreateActorData(playerData, vo, areaData, position)).ToArray();
            return (playerData, actorData);
        }

        static ActorData CreateActorData(PlayerData playerData, ActorPresetVO actorPresetVO, AreaData areaData, Vector3 position)
        {
            var actorData = new ActorData(actorPresetVO, playerData.InstanceId);
            actorData.SetAreaId(areaData.AreaId);
            actorData.SetPosition(areaData.SpawnPosition + position);
            return actorData;
        }
    }
}
