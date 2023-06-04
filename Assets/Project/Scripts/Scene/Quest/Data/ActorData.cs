using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    /// <summary>
    /// ActorData
    /// </summary>
    public class ActorData : IPlayer, IPositionData, IThinkModuleHolder, IOrderModuleHolder, IMovingModuleHolder, ICollisionEventEffectReceiverModuleHolder
    {
        public Guid InstanceId { get; }

        // Module
        public IThinkModule ThinkModule { get; private set; }
        public IOrderModule OrderModule { get; private set; }
        public MovingModule MovingModule { get; private set; }
        public CollisionEventModule CollisionEventModule { get; protected set; }
        public CollisionEventEffectReceiverModule CollisionEventEffectReceiverModule { get; private set; }

        // IPlayer
        public Guid PlayerInstanceId { get; }

        // IPositionData
        public int? AreaId { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; } = Quaternion.identity;

        // 状態
        public ActorStateData ActorStateData { get; }

        // 関連データ
        public ActorSpecVO ActorSpecVO { get; }
        public IWeaponSpecVO[] WeaponSpecVOs { get; }

        public InventoryData InventoryData { get; }
        public List<Guid>[] WeaponDataGroup { get; private set; }
        public Dictionary<Guid, WeaponData> WeaponData { get; private set; }

        public ActorGameObjectHandler ActorGameObjectHandler { get; private set; }

        public ActorData(ActorSpecVO actorSpecVO, IWeaponSpecVO[] weaponSpecVOs, Guid playerInstanceId)
        {
            InstanceId = Guid.NewGuid();

            PlayerInstanceId = playerInstanceId;
            ActorSpecVO = actorSpecVO;
            WeaponSpecVOs = weaponSpecVOs;

            InventoryData = new InventoryData(actorSpecVO.CapacityWidth, actorSpecVO.CapacityHeight);
            ActorStateData = new ActorStateData();
            WeaponDataGroup = new[] { new List<Guid>(), new List<Guid>(), new List<Guid>() };
            WeaponData = weaponSpecVOs
                .Select((vo, weaponIndex) => WeaponDataHelper.GetWeaponData(vo, this, weaponIndex))
                .ToDictionary(weaponData => weaponData.InstanceId, weaponData => weaponData);

            WeaponDataGroup[0].AddRange(WeaponData.Keys);

            ActivateModules();
        }

        public void ActivateModules()
        {
            MovingModule = new MovingModule(this);
            ThinkModule = new ActorThinkModule(this);
            OrderModule = new ActorOrderModule(this);
            CollisionEventModule = new CollisionEventModule(InstanceId, new CollisionShapeSphere(this, 3.0f));
            CollisionEventEffectReceiverModule = new CollisionEventEffectReceiverModule(InstanceId);

            MovingModule.ActivateModule();
            ThinkModule.ActivateModule();
            OrderModule.ActivateModule();
            CollisionEventModule.ActivateModule();
            CollisionEventEffectReceiverModule.ActivateModule();
        }

        public void DeactivateModules()
        {
            MovingModule.DeactivateModule();
            ThinkModule.DeactivateModule();
            OrderModule.DeactivateModule();
            CollisionEventModule.DeactivateModule();
            CollisionEventEffectReceiverModule.DeactivateModule();

             MovingModule = null;
             ThinkModule = null;
             OrderModule = null;
             CollisionEventModule = null;
             CollisionEventEffectReceiverModule = null;
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

        public void AddWeaponEffectData(WeaponEventEffectData weaponEventEffectData)
        {
            WeaponData[weaponEventEffectData.WeaponData.InstanceId].AddWeaponEffectData(weaponEventEffectData);
        }

        public void RemoveWeaponEffectData(WeaponEventEffectData weaponEventEffectData)
        {
            WeaponData[weaponEventEffectData.WeaponData.InstanceId].RemoveWeaponEffectData(weaponEventEffectData);
        }

        public void SetMainTarget(IPositionData target)
        {
            ActorStateData.MainTarget = target;
        }

        public void SetAroundTargets(IPositionData[] targets)
        {
            ActorStateData.AroundTargets = targets;
        }

        public void SetActorGameObjectHandler(ActorGameObjectHandler actorGameObjectHandler)
        {
            ActorGameObjectHandler = actorGameObjectHandler;
        }
    }
}
