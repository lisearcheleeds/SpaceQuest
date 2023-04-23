﻿using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class BulletWeaponEffectData : WeaponEffectData
    {
        public override IOrderModule OrderModule { get; protected set; }
        public override CollisionEffectSenderModule CollisionEffectSenderModule { get; protected set; }
        public override CollisionData CollisionData { get; }
        
        public float LifeTime { get; set; }
        public float CurrentLifeTime { get; set; }
        
        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="fromPositionData">発射位置</param>
        /// <param name="rotation">方向</param>
        /// <param name="targetData">ターゲット</param>
        public BulletWeaponEffectData(WeaponData weaponData, IPositionData fromPositionData, Quaternion rotation, IPositionData targetData) : base(weaponData)
        {
            AreaId = fromPositionData.AreaId;
            Position = fromPositionData.Position;
            Rotation = rotation;

            CollisionData = new CollisionData(this, new CollisionShapeSphere(this, 1.0f));

            MovingModule.InertiaTensor = rotation * Vector3.forward * 2.0f;
            
            LifeTime = 4;
            CurrentLifeTime = 0;
        }
        
        public override void ActivateModules()
        {
            base.ActivateModules();
            
            OrderModule = new BulletWeaponEffectOrderModule(this);
            OrderModule.ActivateModule();
            CollisionEffectSenderModule = new CollisionEffectSenderModule();
            CollisionEffectSenderModule.ActivateModule();
        }

        public override void DeactivateModules()
        {
            base.DeactivateModules();
            
            OrderModule.DeactivateModule();
            OrderModule = null;
            CollisionEffectSenderModule.DeactivateModule();
            CollisionEffectSenderModule = null;
        }

        protected override void OnBeginModuleUpdate(float deltaTime)
        {
        }
    }
}
