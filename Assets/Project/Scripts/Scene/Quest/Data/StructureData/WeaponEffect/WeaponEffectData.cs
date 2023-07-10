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
        public virtual int? AreaId { get; protected set; }
        public virtual Vector3 Position { get; protected set; }
        public virtual Quaternion Rotation { get; protected set; }

        // IReleasableData
        public bool IsReleased { get; private set; }

        public abstract IWeaponEffectSpecVO WeaponEffectSpecVO { get; }

        // 情報
        public WeaponData WeaponData { get; }

        public WeaponEffectGameObjectHandler WeaponEffectGameObjectHandler { get; private set; }

        public HashSet<CollisionEventEffectReceiverModule> CollisionEventEffectReceiverModuleList { get; private set; } = new HashSet<CollisionEventEffectReceiverModule>();

        public int CollideCount { get; private set; }

        protected WeaponEffectData(IWeaponEffectCreateOptionData optionData)
        {
            InstanceId = Guid.NewGuid();
            WeaponData = optionData.WeaponData;

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

        public void AddCollideCount()
        {
            CollideCount++;
            WeaponData.AddCollideCount();
        }
    }
}
