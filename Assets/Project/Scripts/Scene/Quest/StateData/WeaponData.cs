using System;
using AloneSpace;
using UnityEngine;
using VariableInventorySystem;

namespace AloneSpace
{
    public abstract class WeaponData
    {
        public abstract Guid InstanceId { get; }
        public Guid PlayerInstanceId { get; private set; }
        public Guid ActorInstanceId { get; private set; }
        
        public IActorPartsWeaponParameterVO ActorPartsWeaponParameterVO { get; private set; }

        public IPositionData BasePositionData { get; private set; }
        public Vector3 OffsetPosition { get; private set; }
        public Quaternion OffsetRotation { get; private set; }

        public static WeaponData CreateData(Guid playerInstanceId, Guid actorInstanceId, IPositionData basePositionData, IActorPartsWeaponParameterVO actorPartsWeaponParameterVO)
        {
            WeaponData weaponData;
            
            switch (actorPartsWeaponParameterVO)
            {
                case ActorPartsWeaponRifleParameterVO actorPartsWeaponRifleParameterVO:
                    weaponData = new RifleWeaponData(actorPartsWeaponRifleParameterVO);
                    break;
                case ActorPartsWeaponMissileLauncherParameterVO actorPartsWeaponMissileLauncherParameterVO:
                    weaponData = new MissileLauncherWeaponData(actorPartsWeaponMissileLauncherParameterVO);
                    break;
                default:
                    throw new NotImplementedException();
            }

            weaponData.PlayerInstanceId = playerInstanceId;
            weaponData.ActorInstanceId = actorInstanceId;
            weaponData.BasePositionData = basePositionData;
            weaponData.ActorPartsWeaponParameterVO = actorPartsWeaponParameterVO;
            return weaponData;
        }

        public void SetOffsetPosition(Vector3 offsetPosition)
        {
            OffsetPosition = offsetPosition;
        }

        public void SetOffsetRotation(Quaternion offsetRotation)
        {
            OffsetRotation = offsetRotation;
        }

        public abstract float GetAvailability(); 

        public abstract bool IsReloadable();
        
        public abstract void Reload();

        public abstract bool IsExecutable(ITargetData targetData);
        
        public abstract void Execute(ITargetData targetData);

        public abstract void OnLateUpdate(float deltaTime);
    }
}