using UnityEngine;

namespace AloneSpace
{
    public class MissileWeaponEffectData : WeaponEffectData
    {
        public override IOrderModule OrderModule { get; protected set; }
        public override CollisionEffectSenderModule CollisionEffectSenderModule { get; protected set; }
        public override CollisionData CollisionData { get; }

        public ActorPartsWeaponMissileMakerParameterVO ParameterVO { get; }

        public float LifeTime { get; set; }
        public float CurrentLifeTime { get; set; }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="parameterVO">武器Paramデータ</param>
        /// <param name="fromPositionData">発射位置</param>
        /// <param name="rotation">方向</param>
        /// <param name="targetData">ターゲット</param>
        public MissileWeaponEffectData(
            WeaponData weaponData,
            ActorPartsWeaponMissileMakerParameterVO parameterVO,
            IPositionData fromPositionData,
            Quaternion rotation,
            IPositionData targetData) : base(weaponData)
        {
            AreaId = fromPositionData.AreaId;
            Position = fromPositionData.Position;
            Rotation = rotation;

            ParameterVO = parameterVO;

            CollisionData = new CollisionData(this, new CollisionShapeSphere(this, 1.0f));

            TargetData = targetData;

            LifeTime = 8.0f;
            CurrentLifeTime = 0;
        }

        public override void ActivateModules()
        {
            base.ActivateModules();

            OrderModule = new MissileWeaponEffectOrderModule(this);
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
    }
}
