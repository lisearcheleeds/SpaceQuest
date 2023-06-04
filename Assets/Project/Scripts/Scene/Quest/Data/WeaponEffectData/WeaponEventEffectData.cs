using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public abstract class WeaponEventEffectData : IPlayer, IPositionData, IMovingModuleHolder, ICollisionEventEffectSenderModuleHolder, IOrderModuleHolder
    {
        public Guid InstanceId { get; }

        // Module
        public MovingModule MovingModule { get; private set; }
        public abstract IOrderModule OrderModule { get; protected set; }
        public abstract CollisionEventModule CollisionEventModule { get; protected set; }
        public abstract CollisionEventEffectSenderModule CollisionEventEffectSenderModule { get; protected set; }

        // IPlayer
        public Guid PlayerInstanceId => WeaponData.WeaponHolder.PlayerInstanceId;

        // IPositionData
        public int? AreaId { get; protected set; }
        public Vector3 Position { get; protected set; }
        public Quaternion Rotation { get; protected set; }

        // 情報
        public bool IsAlive { get; set; }
        public WeaponData WeaponData { get; }
        public IPositionData TargetData { get; protected set; }

        public WeaponEffectGameObjectHandler WeaponEffectGameObjectHandler { get; private set; }

        protected WeaponEventEffectData(WeaponData weaponData, IPositionData targetData)
        {
            InstanceId = Guid.NewGuid();
            WeaponData = weaponData;
            TargetData = targetData;

            ActivateModules();
        }

        public virtual void ActivateModules()
        {
            MovingModule = new MovingModule(this);
            MovingModule.ActivateModule();
        }

        public virtual void DeactivateModules()
        {
            MovingModule.DeactivateModule();
            MovingModule = null;
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

        public void SetWeaponEffectGameObjectHandler(WeaponEffectGameObjectHandler weaponEffectGameObjectHandler)
        {
            WeaponEffectGameObjectHandler = weaponEffectGameObjectHandler;
        }
    }
}