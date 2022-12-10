using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public abstract class WeaponEffectData : ITargetData, ICollisionData, ICauseDamageData, IThreatData
    {
        public Guid PlayerInstanceId => WeaponData.PlayerInstanceId;
        
        public abstract int AreaId { get; protected set; }
        public abstract Vector3 Position { get; protected set; }
        
        public Guid InstanceId { get; }
        public abstract bool IsAlive { get; protected set; }
        public abstract Vector3 MoveDelta { get; protected set; }
        
        public abstract bool IsCollidable { get; protected set; }
        public abstract CollisionShape CollisionShape { get; protected set; }
        public abstract void OnCollision(ICollisionData otherCollisionData);
        
        public WeaponData WeaponData { get; }
        public abstract ITargetData TargetData { get; protected set; }

        public abstract CollisionShape HitCollidePrediction { get; protected set; }

        public abstract void OnLateUpdate(float delta);

        protected WeaponEffectData(WeaponData weaponData)
        {
            InstanceId = Guid.NewGuid();
            WeaponData = weaponData;
        }
    }
}