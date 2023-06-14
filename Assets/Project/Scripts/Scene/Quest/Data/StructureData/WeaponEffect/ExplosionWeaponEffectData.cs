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

            LifeTime = 400;
            CurrentLifeTime = 0;

            OrderModule = new ExplosionWeaponEffectOrderModule(this);
            CollisionEventModule = new ExplosionWeaponEffectCollisionEventModule(InstanceId, this, new CollisionShapeSphere(this, 1.0f));
            CollisionEventEffectSenderModule = new ExplosionWeaponEffectCollisionEventEffectSenderModule(InstanceId, this);
        }

        public override void ActivateModules()
        {
            base.ActivateModules();

            OrderModule.ActivateModule();
            CollisionEventModule.ActivateModule();
            CollisionEventEffectSenderModule.ActivateModule();
        }

        public override void DeactivateModules()
        {
            base.DeactivateModules();

            OrderModule.DeactivateModule();
            CollisionEventModule.DeactivateModule();
            CollisionEventEffectSenderModule.DeactivateModule();

            // NOTE: 別にnull入れなくても良いがIsReleased見ずにModule見ようとしたらコケてくれるので
            OrderModule = null;
            CollisionEventModule = null;
            CollisionEventEffectSenderModule = null;
        }
    }
}
