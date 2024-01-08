using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    /// <summary>
    /// ActorData
    /// </summary>
    public class ActorData : IPlayer, IPositionData, IReleasableData, IThinkModuleHolder, IOrderModuleHolder, IMovingModuleHolder, ICollisionEventEffectReceiverModuleHolder
    {
        public Guid InstanceId { get; }

        // Module
        public IThinkModule ThinkModule { get; private set; }
        public IOrderModule OrderModule { get; private set; }
        public MovingModule MovingModule { get; private set; }
        public CollisionEventModule CollisionEventModule { get; private set; }
        public CollisionEventEffectReceiverModule CollisionEventEffectReceiverModule { get; private set; }

        // IPlayer
        public Guid PlayerInstanceId { get; }

        // IPositionData
        public int? AreaId { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; } = Quaternion.identity;

        // IReleasableData
        public bool IsReleased { get; private set; }

        // 状態
        public ActorStateData ActorStateData { get; } = new ActorStateData();

        // 関連データ
        public ActorSpecVO ActorSpecVO { get; }
        public IWeaponSpecVO[] WeaponSpecVOs { get; }
        public SpecialEffectSpecVO[] ActorPresetSpecialEffectSpecVOs { get; }

        // インベントリ
        public InventoryData InventoryData { get; }

        // 装備
        public List<Guid>[] WeaponDataGroup { get; private set; }
        public Dictionary<Guid, WeaponData> WeaponData { get; private set; }

        // GameObjectHandler
        public ActorGameObjectHandler ActorGameObjectHandler { get; private set; }

        public ActorData(ActorPresetVO actorPresetVO, Guid playerInstanceId)
        {
            InstanceId = Guid.NewGuid();

            PlayerInstanceId = playerInstanceId;
            ActorSpecVO = actorPresetVO.ActorSpecVO;
            WeaponSpecVOs = actorPresetVO.WeaponSpecVOs;

            InventoryData = new InventoryData(actorPresetVO.ActorSpecVO.CapacityWidth, actorPresetVO.ActorSpecVO.CapacityHeight);
            WeaponDataGroup = Enumerable.Range(0, ConstantInt.WeaponGroupCount).Select(_ => new List<Guid>()).ToArray();
            WeaponData = actorPresetVO.WeaponSpecVOs
                .Where(vo => vo != null)
                .Select((vo, weaponIndex) => WeaponDataHelper.GetWeaponData(vo, this, weaponIndex))
                .ToDictionary(weaponData => weaponData.InstanceId, weaponData => weaponData);
            ActorPresetSpecialEffectSpecVOs = actorPresetVO.SpecialEffectSpecVOs;

            WeaponDataGroup[0].AddRange(WeaponData.Keys);

            ActorStateData.EnduranceValue = ActorSpecVO.EnduranceValue;
            ActorStateData.EnduranceValueMax = ActorSpecVO.EnduranceValue;

            ActorStateData.ShieldValue = ActorSpecVO.ShieldValue;
            ActorStateData.ShieldValueMax = ActorSpecVO.ShieldValue;

            ActorStateData.SpecialEffectDataList.AddRange(ActorSpecVO.SpecialEffectSpecVOs.Select(x => new SpecialEffectData(x, SpecialEffectSourceType.SelfActorSpec, InstanceId)));
            ActorStateData.SpecialEffectDataList.AddRange(ActorPresetSpecialEffectSpecVOs.Select(x => new SpecialEffectData(x, SpecialEffectSourceType.SelfActorPreset, InstanceId)));
            ActorStateData.SpecialEffectDataList.AddRange(WeaponSpecVOs.Where(vo => vo != null).SelectMany(x => x.SpecialEffectSpecVOs.Select(x => new SpecialEffectData(x, SpecialEffectSourceType.SelfWeapon, InstanceId))));

            MovingModule = new MovingModule(this);
            ThinkModule = new ActorThinkModule(this);
            OrderModule = new ActorOrderModule(this);
            CollisionEventModule = new ActorCollisionEventModule(InstanceId, this, new CollisionShapeSphere(this, 3.0f));
            CollisionEventEffectReceiverModule = new ActorCollisionEventEffectReceiverModule(InstanceId, this);
        }

        public void ActivateModules()
        {
            MovingModule.ActivateModule();
            ThinkModule.ActivateModule();
            OrderModule.ActivateModule();
            CollisionEventModule.ActivateModule();
            CollisionEventEffectReceiverModule.ActivateModule();

            foreach (var weaponData in WeaponData.Values)
            {
                weaponData.ActivateModules();
            }

            foreach (var specialEffectData in ActorStateData.SpecialEffectDataList)
            {
                specialEffectData.ActivateModules();
            }
        }

        public void DeactivateModules()
        {
            MovingModule.DeactivateModule();
            ThinkModule.DeactivateModule();
            OrderModule.DeactivateModule();
            CollisionEventModule.DeactivateModule();
            CollisionEventEffectReceiverModule.DeactivateModule();

            foreach (var weaponData in WeaponData.Values)
            {
                weaponData.DeactivateModules();
            }

            foreach (var specialEffectData in ActorStateData.SpecialEffectDataList)
            {
                specialEffectData.DeactivateModules();
            }

            // NOTE: 別にnull入れなくても良いがIsReleased見ずにModule見ようとしたらコケてくれるので
            MovingModule = null;
            ThinkModule = null;
            OrderModule = null;
            CollisionEventModule = null;
            CollisionEventEffectReceiverModule = null;
        }

        public void AddInteractOrder(InteractOrderState interactOrderState)
        {
            ActorStateData.InteractOrderStateList.Add(interactOrderState);
        }

        public void RemoveInteractOrder(InteractOrderState interactOrderState)
        {
            ActorStateData.InteractOrderStateList.Remove(interactOrderState);
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

        public void AddSpecialEffectData(SpecialEffectData specialEffectData)
        {
            ActorStateData.SpecialEffectDataList.Add(specialEffectData);
        }

        public void RemoveSpecialEffectStateData(SpecialEffectData specialEffectData)
        {
            ActorStateData.SpecialEffectDataList.Remove(specialEffectData);
        }

        public void SetMainTarget(IPositionData target)
        {
            ActorStateData.MainTarget = target;
        }

        public void SetActorGameObjectHandler(ActorGameObjectHandler actorGameObjectHandler)
        {
            ActorGameObjectHandler = actorGameObjectHandler;
        }

        public void AddDamageEventData(DamageEventData damageEventData)
        {
            ActorStateData.CurrentDamageEventDataList.Add(damageEventData);
        }

        public void Release()
        {
            IsReleased = true;

            foreach (var weaponData in WeaponData.Values)
            {
                weaponData.Release();
            }
        }
    }
}
