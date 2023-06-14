using System;
using UnityEngine;

namespace AloneSpace
{
    /// <summary>
    /// Actorが装備しているWeaponData
    /// （ItemとしてのWeaponDataは存在せずActorに装備する時に生成する）
    /// </summary>
    public abstract class WeaponData : IReleasableData, IOrderModuleHolder
    {
        public abstract Guid InstanceId { get; }

        // Module
        public abstract IOrderModule OrderModule { get; protected set; }
        public abstract IWeaponSpecVO WeaponSpecVO { get; }
        public abstract WeaponStateData WeaponStateData { get; }

        public bool IsReleased { get; private set; }

        public ActorData WeaponHolder { get; }
        public int WeaponIndex { get; }

        public WeaponGameObjectHandler WeaponGameObjectHandler { get; private set; }

        public abstract void ActivateModules();
        public abstract void DeactivateModules();

        protected WeaponData(ActorData actorData, int weaponIndex)
        {
            WeaponHolder = actorData;
            WeaponIndex = weaponIndex;
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

        public void SetWeaponGameObjectHandler(WeaponGameObjectHandler weaponGameObjectHandler)
        {
            WeaponGameObjectHandler = weaponGameObjectHandler;
        }

        public abstract void Reload();

        public void Release()
        {
            IsReleased = true;
        }
    }
}
