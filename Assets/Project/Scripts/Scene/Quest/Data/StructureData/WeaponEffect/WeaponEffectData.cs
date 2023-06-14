using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public abstract class WeaponEffectData : IPlayer, IPositionData, IReleasableData, IMovingModuleHolder, ICollisionEventEffectSenderModuleHolder, IOrderModuleHolder
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

        // IReleasableData
        public bool IsReleased { get; private set; }

        public abstract IWeaponEffectSpecVO WeaponEffectSpecVO { get; }

        // 情報
        public WeaponData WeaponData { get; }
        public IPositionData TargetData { get; protected set; }

        public WeaponEffectGameObjectHandler WeaponEffectGameObjectHandler { get; private set; }

        public HashSet<CollisionEventEffectReceiverModule> CollisionEventEffectReceiverModuleList { get; private set; } = new HashSet<CollisionEventEffectReceiverModule>();

        protected WeaponEffectData(WeaponData weaponData, IPositionData targetData)
        {
            InstanceId = Guid.NewGuid();
            WeaponData = weaponData;
            TargetData = targetData;

            MovingModule = new MovingModule(this);
        }

        public virtual void ActivateModules()
        {
            MovingModule.ActivateModule();
        }

        public virtual void DeactivateModules()
        {
            MovingModule.DeactivateModule();

            // NOTE: 別にnull入れなくても良いがIsReleased見ずにModule見ようとしたらコケてくれるので
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

        public void AddCollisionEventEffectList(IEnumerable<CollisionEventEffectReceiverModule> receiverList)
        {
            CollisionEventEffectReceiverModuleList.UnionWith(receiverList);
        }

        public void Release()
        {
            IsReleased = true;
        }
    }
}
