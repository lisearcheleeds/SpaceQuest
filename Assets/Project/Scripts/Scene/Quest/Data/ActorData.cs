using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class ActorData : IPlayer, IPositionData, IThinkModuleHolder, IOrderModuleHolder, IMovingModuleHolder, ICollisionEffectReceiverModuleHolder
    {
        public Guid InstanceId { get; }
        
        // Module
        public IThinkModule ThinkModule { get; }
        public IOrderModule OrderModule { get; }
        public MovingModule MovingModule { get; }
        public CollisionEffectReceiverModule CollisionEffectReceiverModule { get; }
        
        // ModuleData
        public CollisionData CollisionData { get; }

        // IPlayer
        public Guid PlayerInstanceId { get; }
        // int TeamId { get; }
        
        // IPositionData
        public int? AreaId { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; } = Quaternion.identity;
        
        // 状態
        public ActorMode ActorMode { get; private set; }
        public ActorState ActorState { get; private set; }
        public ActorCombatMode ActorCombatMode { get; private set; } = ActorCombatMode.Fighter;

        public ActorSpecData ActorSpecData { get; }
        public InventoryData[] InventoryDataList { get; }
        
        public ActorStateData ActorStateData { get; } = new ActorStateData();
        
        public ActorData(ActorSpecData actorSpecData, Guid playerInstanceId)
        {
            InstanceId = Guid.NewGuid();
            MovingModule = new MovingModule(this, OnBeginModuleUpdate);
            ThinkModule = new ActorThinkModule(this);
            OrderModule = new ActorOrderModule(this);
            CollisionEffectReceiverModule = new CollisionEffectReceiverModule();
            CollisionData = new CollisionData(this, new CollisionShapeSphere(this, 3.0f));

            PlayerInstanceId = playerInstanceId;
            
            ActorSpecData = actorSpecData;

            InventoryDataList = actorSpecData.ActorPartsExclusiveInventoryParameterVOs
                .Select(vo => new InventoryData(vo.CapacityWidth, vo.CapacityHeight)).ToArray();
            
            ActorStateData.WeaponData = ActorSpecData.ActorPartsWeaponParameterVOs.Select(x =>
            {
                var weaponData = WeaponDataHelper.GetWeaponData(x);
                weaponData.SetHolderActor(this, this);
                return weaponData;
            }).ToArray();
        }

        public void SetActorState(ActorState actorState)
        {
            ActorState = actorState;
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
                ActorMode = ActorMode.ThirdPersonViewpoint;
                ActorStateData.MoveTarget = null;
                return;
            }

            // 今どのエリアにも居ない時、もしくは移動先のエリアが違う時ワープ状態とする
            if (AreaId != moveTarget.AreaId)
            {
                ActorMode = ActorMode.Warp;
            }
            else
            {
                ActorMode = ActorMode.ThirdPersonViewpoint;
            }

            ActorStateData.MoveTarget = moveTarget;
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
            ActorMode = actorMode;
        }

        public void SetActorCombatMode(ActorCombatMode actorCombatMode)
        {
            ActorCombatMode = actorCombatMode;
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
