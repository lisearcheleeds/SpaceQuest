using UnityEngine;

namespace AloneSpace
{
    public class ParticleBulletWeaponEffectData : WeaponEffectData
    {
        public override IOrderModule OrderModule { get; protected set; }
        public override CollisionEventModule CollisionEventModule { get; protected set; }
        public override CollisionEventEffectSenderModule CollisionEventEffectSenderModule { get; protected set; }

        public override IWeaponEffectSpecVO WeaponEffectSpecVO => SpecVO;

        public override int? AreaId => OptionData.GetFromPositionData().AreaId;
        public override Vector3 Position => OptionData.GetFromPositionData().Position;
        public override Quaternion Rotation => OptionData.GetOffsetRotation() * OptionData.GetFromPositionData().Rotation;

        public ParticleBulletWeaponEffectSpecVO SpecVO { get; }

        public ParticleBulletWeaponEffectCreateOptionData OptionData { get; }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="specVO">WeaponEffectデータ</param>
        /// <param name="optionData">optionData</param>
        public ParticleBulletWeaponEffectData(
            ParticleBulletWeaponEffectSpecVO specVO,
            ParticleBulletWeaponEffectCreateOptionData optionData) : base(optionData)
        {
            SpecVO = specVO;
            OptionData = optionData;

            OrderModule = new ParticleBulletWeaponEffectOrderModule(this);
            CollisionEventModule = new ParticleBulletWeaponEffectCollisionEventModule(InstanceId, this, new CollisionShapeSphere(this, 1.0f));
            CollisionEventEffectSenderModule = new ParticleBulletWeaponEffectCollisionEventEffectSenderModule(InstanceId, this);
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
