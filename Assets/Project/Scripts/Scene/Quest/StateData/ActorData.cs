﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoboQuest.Quest
{
    public class ActorData : ITargetData, IDamageableData
    {
        public Guid InstanceId { get; }
        public Guid PlayerQuestDataInstanceId { get; }

        public ActorState ActorState { get; private set; }

        public int CurrentAreaIndex { get; private set; }
        public int? DestinateAreaIndex { get; private set; }

        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        
        public ActorSpecData ActorSpecData { get; }
        public MapData MapData { get; }
        public InventoryData[] InventoryDataList { get; }
        
        public WeaponData[] WeaponData { get; }

        public bool IsAlive => ActorState == ActorState.Running;
        public bool IsTargetable => ActorState == ActorState.Running;
        public bool IsBroken => ActorState == ActorState.Broken;

        public float HitPoint { get; private set; }

        public List<IInteractData> InteractOrder { get; } = new List<IInteractData>();

        public bool IsInteracting => InteractingData != null;
        public IInteractData InteractingData { get; private set; }
        public float InteractingTime { get; private set; }
        public bool IsInteractComplete => InteractingTime > InteractingData.InteractTime;

        public ActorData(ActorSpecData actorSpecData, Guid playerQuestDataInstanceId, MapData mapData)
        {
            InstanceId = Guid.NewGuid();
            
            ActorSpecData = actorSpecData;
            PlayerQuestDataInstanceId = playerQuestDataInstanceId;
            MapData = mapData;

            InventoryDataList = actorSpecData.ActorPartsExclusiveInventoryParameterVOs
                .Select(vo => new InventoryData(vo.CapacityWidth, vo.CapacityHeight)).ToArray();
            
            WeaponData = ActorSpecData.ActorPartsWeaponParameterVOs.Select(x => Quest.WeaponData.CreateData(playerQuestDataInstanceId, InstanceId, x)).ToArray();
            
            // FIXME ここから移動する
            HitPoint = ActorSpecData.Endurance;
        }

        public void SetActorState(ActorState actorState)
        {
            ActorState = actorState;
        }
        
        public void SetCurrentAreaIndex(int areaIndex)
        {
            CurrentAreaIndex = areaIndex;
        }
        
        public void SetDestinateAreaIndex(int? areaIndex)
        {
            DestinateAreaIndex = areaIndex;
        }

        public RouteAreaData[] GetRouteAreaData()
        {
            if (!DestinateAreaIndex.HasValue || DestinateAreaIndex.Value == CurrentAreaIndex)
            {
                return null;
            }

            return MapData.GetRouteAreaData(CurrentAreaIndex, DestinateAreaIndex.Value);
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;

            foreach (var weaponData in WeaponData)
            {
                weaponData.SetPosition(position);
            }
        }

        public void SetRotation(Quaternion rotation)
        {
            Rotation = rotation;
            
            foreach (var weaponData in WeaponData)
            {
                weaponData.SetRotation(rotation);
            }
        }

        public void OnDamage(DamageData damageData)
        {
            HitPoint -= damageData.DamageValue;
            
            if (HitPoint <= 0 && ActorState != ActorState.Broken)
            {
                SetActorState(ActorState.Broken);
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var weaponData in WeaponData)
            {
                weaponData.Update(deltaTime);
            }

            if (InteractingData != null)
            {
                InteractingTime += deltaTime;
                if (IsInteractComplete)
                {
                    switch (InteractingData)
                    {
                        case ItemInteractData itemInteractData:
                            InteractOrder.Remove(InteractingData);
                            SetInteract(null);
                            var insertableInventory = InventoryDataList.FirstOrDefault(x => x.VariableInventoryViewData.GetInsertableId(itemInteractData.ItemData).HasValue);
                            MessageBus.Instance.ManagerCommandStoreItem.Broadcast(itemInteractData.AreaIndex, insertableInventory, itemInteractData.ItemData);
                            break;
                        case AreaTransitionInteractData areaTransitionInteractData:
                            InteractOrder.Remove(InteractingData);
                            SetInteract(null);
                            MessageBus.Instance.ManagerCommandActorAreaTransition.Broadcast(this, areaTransitionInteractData.TransitionAreaIndex);
                            break;
                        case BrokenActorInteractData brokenActorInteractData:
                            throw new NotImplementedException();
                            break;
                        case InventoryInteractData inventoryInteractData:
                            // ユーザー操作待ち
                            break;
                    }
                }
            }
        }

        public IInteractData GetNextInteractOrder()
        {
            var nextOrder = InteractOrder.FirstOrDefault(x => !(x is AreaTransitionInteractData));
            if (nextOrder != null)
            {
                return nextOrder;
            }
            
            return InteractOrder.FirstOrDefault(); 
        }

        public void SetInteract(IInteractData interactData)
        {
            InteractingData = interactData;
            InteractingTime = 0;
        }
    }
}
