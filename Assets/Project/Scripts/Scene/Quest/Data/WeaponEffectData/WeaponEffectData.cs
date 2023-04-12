using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public abstract class WeaponEffectData : IPlayer, IPositionData, IMovingModuleHolder, ICollisionEffectSenderModuleHolder
    {
        public Guid InstanceId { get; }
        
        // Module
        public MovingModule MovingModule { get; }
        public abstract CollisionEffectSenderModule CollisionEffectSenderModule { get; }
        public abstract CollisionData CollisionData { get; }

        // IPlayer
        public Guid PlayerInstanceId => WeaponData.WeaponHolder.PlayerInstanceId;
        
        // IPositionData
        public int? AreaId { get; protected set; }
        public Vector3 Position { get; protected set; }
        public Quaternion Rotation { get; protected set; }
        
        // 情報
        public bool IsAlive { get; protected set; }
        public WeaponData WeaponData { get; }
        public IPositionData TargetData { get; protected set; }

        protected WeaponEffectData(WeaponData weaponData)
        {
            InstanceId = Guid.NewGuid();
            MovingModule = new MovingModule(this, OnBeginModuleUpdate);
            WeaponData = weaponData;
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

        public void AddHit(ICollisionDataHolder otherCollisionDataHolder)
        {
            CollisionEffectSenderModule.AddHit(otherCollisionDataHolder);
        }

        protected abstract void OnBeginModuleUpdate(float deltaTime);
    }
}