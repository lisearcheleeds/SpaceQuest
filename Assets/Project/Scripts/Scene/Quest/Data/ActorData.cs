using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    /// <summary>
    /// ActorData
    /// </summary>
    public class ActorData : IPlayer, IPositionData, IThinkModuleHolder, IOrderModuleHolder, IMovingModuleHolder, ICollisionEffectReceiverModuleHolder
    {
        public Guid InstanceId { get; }
        
        // Module
        public IThinkModule ThinkModule { get; private set; }
        public IOrderModule OrderModule { get; private set; }
        public MovingModule MovingModule { get; private set; }
        public CollisionEffectReceiverModule CollisionEffectReceiverModule { get; private set; }
        
        // ModuleData
        public CollisionData CollisionData { get; }

        // IPlayer
        public Guid PlayerInstanceId { get; }
        
        // IPositionData
        public int? AreaId { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; } = Quaternion.identity;
        
        // 状態
        public ActorStateData ActorStateData { get; }
        
        // 関連データ
        public ActorSpecData ActorSpecData { get; }
        public InventoryData[] InventoryDataList { get; }
        public List<Guid>[] WeaponDataGroup { get; private set; }
        public Dictionary<Guid, WeaponData> WeaponData { get; private set; }

        public ActorData(ActorSpecData actorSpecData, Guid playerInstanceId)
        {
            InstanceId = Guid.NewGuid();
            
            PlayerInstanceId = playerInstanceId;
            ActorSpecData = actorSpecData;
            
            CollisionData = new CollisionData(this, new CollisionShapeSphere(this, 3.0f));

            InventoryDataList = actorSpecData.ActorPartsExclusiveInventoryParameterVOs
                .Select(vo => new InventoryData(vo.CapacityWidth, vo.CapacityHeight)).ToArray();

            ActorStateData = new ActorStateData();
            
            WeaponDataGroup = new[] { new List<Guid>(), new List<Guid>(), new List<Guid>() };
            
            WeaponData = ActorSpecData.ActorPartsWeaponParameterVOs.Select(x =>
            {
                var weaponData = WeaponDataHelper.GetWeaponData(x);
                weaponData.SetHolderActor(this, this);
                WeaponDataGroup[0].Add(weaponData.InstanceId);
                return weaponData;
            }).ToDictionary(weaponData => weaponData.InstanceId, weaponData => weaponData);
            
            ActivateModules();
        }

        public void ActivateModules()
        {
            MovingModule = new MovingModule(this, OnBeginModuleUpdate);
            ThinkModule = new ActorThinkModule(this);
            OrderModule = new ActorOrderModule(this);
            CollisionEffectReceiverModule = new CollisionEffectReceiverModule();
            
            MovingModule.ActivateModule();
            ThinkModule.ActivateModule();
            OrderModule.ActivateModule();
            CollisionEffectReceiverModule.ActivateModule();
        }

        public void DeactivateModules()
        {
            MovingModule.DeactivateModule();
            ThinkModule.DeactivateModule();
            OrderModule.DeactivateModule();
            CollisionEffectReceiverModule.DeactivateModule();
            
             MovingModule = null;
             ThinkModule = null;
             OrderModule = null;
             CollisionEffectReceiverModule = null;
        }

        public void SetInteractOrder(IInteractData interactData)
        {
            ActorStateData.InteractOrder = interactData;
        }

        public void SetAreaId(int? areaId)
        {
            AreaId = areaId;
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            Rotation = rotation;
        }

        public void SetMoveTarget(IPositionData moveTarget)
        {
            if (moveTarget == null)
            {
                ActorStateData.ActorMode = ActorMode.ThirdPersonViewpoint;
                ActorStateData.MoveTarget = null;
                return;
            }

            // 今どのエリアにも居ない時、もしくは移動先のエリアが違う時ワープ状態とする
            if (AreaId != moveTarget.AreaId)
            {
                ActorStateData.ActorMode = ActorMode.Warp;
            }
            else
            {
                ActorStateData.ActorMode = ActorMode.ThirdPersonViewpoint;
            }

            ActorStateData.MoveTarget = moveTarget;
        }

        public void SetWeaponExecute(bool isExecute)
        {
            foreach (var key in WeaponData.Keys)
            {
                // 現在のWeaponDataGroupのWeaponDataだけではなく全てのWeaponDataに対して更新をかける
                var isCurrentWeaponDataGroup = WeaponDataGroup[ActorStateData.CurrentWeaponGroupIndex]
                    .Any(weaponDataInstanceId => weaponDataInstanceId == key);
                
                WeaponData[key].SetExecute(isExecute && isCurrentWeaponDataGroup);
            }
        }

        public void ReloadWeapon()
        {
            foreach (var weaponData in WeaponData.Values)
            {
                if (weaponData.WeaponStateData.IsReloadable)
                {
                    weaponData.Reload();
                }
            }
        }

        public void SetForwardBoosterPowerRatio(float power)
        {
            ActorStateData.ForwardBoosterPowerRatio = power;
        }
        
        public void SetBackBoosterPowerRatio(float power)
        {
            ActorStateData.BackBoosterPowerRatio = power;
        }
        
        public void SetRightBoosterPowerRatio(float power)
        {
            ActorStateData.RightBoosterPowerRatio = power;
        }
        
        public void SetLeftBoosterPowerRatio(float power)
        {
            ActorStateData.LeftBoosterPowerRatio = power;
        }
        
        public void SetTopBoosterPowerRatio(float power)
        {
            ActorStateData.TopBoosterPowerRatio = power;
        }
        
        public void SetBottomBoosterPowerRatio(float power)
        {
            ActorStateData.BottomBoosterPowerRatio = power;
        }
        
        public void SetPitchBoosterPowerRatio(float power)
        {
            ActorStateData.PitchBoosterPowerRatio = power;
        }
        
        public void SetRollBoosterPowerRatio(float power)
        {
            ActorStateData.RollBoosterPowerRatio = power;
        }
        
        public void SetYawBoosterPowerRatio(float power)
        {
            ActorStateData.YawBoosterPowerRatio = power;
        }

        public void SetLookAtDirection(Vector3 lookAt)
        {
            ActorStateData.LookAtDirection = lookAt;
        }

        public void SetActorMode(ActorMode actorMode)
        {
            ActorStateData.ActorMode = actorMode;
        }

        public void SetActorCombatMode(ActorCombatMode actorCombatMode)
        {
            ActorStateData.ActorCombatMode = actorCombatMode;
        }

        public void SetCurrentWeaponGroupIndex(int weaponGroupIndex)
        {
            ActorStateData.CurrentWeaponGroupIndex = weaponGroupIndex;
        }

        public void AddWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            WeaponData[weaponEffectData.WeaponData.InstanceId].AddWeaponEffectData(weaponEffectData);
        }

        public void RemoveWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            WeaponData[weaponEffectData.WeaponData.InstanceId].RemoveWeaponEffectData(weaponEffectData);
        }

        void OnBeginModuleUpdate(float deltaTime)
        {
        }

        public void AddHit(ICollisionDataHolder otherCollisionDataHolder)
        {
            CollisionEffectReceiverModule.AddHit(otherCollisionDataHolder);
        }
    }
}
