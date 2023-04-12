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
        public IPositionData PositionData { get; private set; }
        public Quaternion OffsetRotation { get; private set; }

        public Vector3 LookAtDirection { get; private set; }

        public void SetHolderActor(ActorData weaponHolder, IPositionData positionData)
        {
            WeaponHolder = weaponHolder;
            PositionData = positionData;
        }

        public void SetOffsetRotation(Quaternion offsetRotation)
        {
            OffsetRotation = offsetRotation;
        }

        public void SetLookAtDirection(Vector3 lookAtDirection)
        {
            LookAtDirection = lookAtDirection;
        }

        public abstract bool IsReloadable();
        public abstract void Reload();
        public abstract bool IsExecutable(IPositionData targetData);
        public abstract void Execute(IPositionData targetData);
    }
}