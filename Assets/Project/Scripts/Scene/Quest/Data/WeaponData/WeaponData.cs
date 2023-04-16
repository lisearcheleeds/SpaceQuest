using System;
using UnityEngine;

namespace AloneSpace
{
    public abstract class WeaponData : IOrderModuleHolder
    {
        public abstract Guid InstanceId { get; }
        public abstract IOrderModule OrderModule { get; }
        public abstract IActorPartsWeaponParameterVO ActorPartsWeaponParameterVO { get; }
        public abstract WeaponStateData WeaponStateData { get; }
        
        public ActorData WeaponHolder { get; private set; }
        public IPositionData BasePositionData { get; private set; }

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

        public abstract void Reload();
    }
}