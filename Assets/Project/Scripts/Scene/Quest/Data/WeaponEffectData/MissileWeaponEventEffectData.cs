using UnityEngine;

namespace AloneSpace
{
    public class MissileWeaponEventEffectData : WeaponEventEffectData
    {
        public override IOrderModule OrderModule { get; protected set; }
        public override CollisionEventModule CollisionEventModule { get; protected set; }
        public override CollisionEventEffectSenderModule CollisionEventEffectSenderModule { get; protected set; }

        public WeaponMissileMakerSpecVO VO { get; }

        public float LifeTime { get; set; }
        public float CurrentLifeTime { get; set; }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="vorameterVO">武器Paramデータ</param>
        /// <param name="fromPositionData">発射位置</param>
        /// <param name="rotation">方向</param>
        /// <param name="targetData">ターゲット</param>
        public MissileWeaponEventEffectData(
            WeaponData weaponData,
            WeaponMissileMakerSpecVO vo,
            IPositionData fromPositionData,
            Quaternion rotation,
            IPositionData targetData) : base(weaponData, targetData)
        {
            AreaId = fromPositionData.AreaId;
            Position = fromPositionData.Position;
            Rotation = rotation;

            VO = vo;

            LifeTime = 8.0f;
            CurrentLifeTime = 0;
        }

        public override void ActivateModules()
        {
            base.ActivateModules();

            OrderModule = new MissileWeaponEffectOrderModule(this);
            OrderModule.ActivateModule();
            CollisionEventModule = new CollisionEventModule(InstanceId, new CollisionShapeSphere(this, 1.0f));
            CollisionEventModule.ActivateModule();
            CollisionEventEffectSenderModule = new CollisionEventEffectSenderModule(InstanceId);
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
