using System.Collections.Generic;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ExplosionWeaponEffectData : WeaponEffectData
    {
        public override IOrderModule OrderModule { get; protected set; }
        public override CollisionEventModule CollisionEventModule { get; protected set; }
        public override CollisionEventEffectSenderModule CollisionEventEffectSenderModule { get; protected set; }

        public override IWeaponEffectSpecVO WeaponEffectSpecVO => SpecVO;
        public ExplosionWeaponEffectSpecVO SpecVO { get; }

        public float LifeTime { get; set; }
        public float CurrentLifeTime { get; set; }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="specVO">WeaponEffectデータ</param>
        /// <param name="weaponData">weaponData</param>
        /// <param name="fromPositionData">発射位置</param>
        /// <param name="rotation">方向</param>
        /// <param name="targetData">ターゲット</param>
        public ExplosionWeaponEffectData(
            ExplosionWeaponEffectSpecVO specVO,
            WeaponData weaponData,
            IPositionData fromPositionData,
            Quaternion rotation,
            IPositionData targetData) : base(weaponData, targetData)
        {
            AreaId = fromPositionData.AreaId;
            Position = fromPositionData.Position;
            Rotation = rotation;

            SpecVO = specVO;

            LifeTime = 4;
            CurrentLifeTime = 0;
        }

        public override void ActivateModules()
        {
            base.ActivateModules();

            OrderModule = new ExplosionWeaponEffectOrderModule(this);
            OrderModule.ActivateModule();
            CollisionEventModule = new ExplosionWeaponEffectCollisionEventModule(InstanceId, this, new CollisionShapeSphere(this, 1.0f));
            CollisionEventModule.ActivateModule();
            CollisionEventEffectSenderModule = new ExplosionWeaponEffectCollisionEventEffectSenderModule(InstanceId, this);
            CollisionEventEffectSenderModule.ActivateModule();
        }

        public override void DeactivateModules()
        {
            base.DeactivateModules();

            OrderModule.DeactivateModule();
            OrderModule = null;
            CollisionEventModule.DeactivateModule();
            CollisionEventModule = null;
            CollisionEventEffectSenderModule.DeactivateModule();
            CollisionEventEffectSenderModule = null;
        }
    }
}
