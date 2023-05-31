using System;
using UnityEngine;

namespace AloneSpace
{
    /// <summary>
    /// Actorが装備しているWeaponData
    /// （ItemとしてのWeaponDataは存在せずActorに装備する時に生成する）
    /// </summary>
    public abstract class WeaponData : IOrderModuleHolder
    {
        public abstract Guid InstanceId { get; }

        // Module
        public abstract IOrderModule OrderModule { get; protected set; }
        public abstract IWeaponSpecVO WeaponSpecVO { get; }
        public abstract WeaponStateData WeaponStateData { get; }

        public ActorData WeaponHolder { get; private set; }
        public IPositionData BasePositionData { get; private set; }

        public abstract void ActivateModules();
        public abstract void DeactivateModules();

        public void SetHolderActor(ActorData weaponHolder, IPositionData basePositionData)
        {
            WeaponHolder = weaponHolder;
            BasePositionData = basePositionData;
        }

        public void SetLookAtDirection(Vector3 lookAtDirection)
        {
            WeaponStateData.LookAtDirection = lookAtDirection;
        }

        public void SetTargetData(IPositionData targetData)
        {
            WeaponStateData.TargetData = targetData;
        }

        public void SetExecute(bool isExecute)
        {
            WeaponStateData.IsExecute = isExecute;
        }

        public void AddWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            WeaponStateData.WeaponEffectDataList.Add(weaponEffectData);
        }

        public void RemoveWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            WeaponStateData.WeaponEffectDataList.Remove(weaponEffectData);
        }

        public abstract void Reload();
    }
}