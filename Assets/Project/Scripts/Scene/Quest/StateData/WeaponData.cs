using System;
using UnityEngine;
using VariableInventorySystem;

namespace RoboQuest.Quest
{
    public abstract class WeaponData
    {
        public abstract Guid InstanceId { get; }
        public Guid PlayerInstanceId { get; private set; }
        public Guid ActorInstanceId { get; private set; }
        
        public IActorPartsWeaponParameterVO ActorPartsWeaponParameterVO { get; private set; }

        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        public static WeaponData CreateData(Guid playerInstanceId, Guid actorInstanceId, IActorPartsWeaponParameterVO actorPartsWeaponParameterVO)
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
            weaponData.ActorPartsWeaponParameterVO = actorPartsWeaponParameterVO;
            return weaponData;
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            Rotation = rotation;
        }

        public abstract float GetAvailability(); 

        public abstract bool IsReloadable();
        
        public abstract void Reload(ItemVO[] resources);

        public abstract bool IsExecutable(ITargetData targetData);
        
        public abstract void Execute(ITargetData targetData);

        public abstract void Update(float deltaTime);
    }
}