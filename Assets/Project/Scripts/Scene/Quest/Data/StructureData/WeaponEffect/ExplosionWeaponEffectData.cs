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

        public ExplosionWeaponEffectCreateOptionData OptionData { get; }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="specVO">WeaponEffectデータ</param>
        /// <param name="optionData">optionData</param>
        public ExplosionWeaponEffectData(
            ExplosionWeaponEffectSpecVO specVO,
            ExplosionWeaponEffectCreateOptionData optionData) : base(optionData)
        {
            AreaId = optionData.AreaId;
            Position = optionData.Position;
            Rotation = optionData.Rotation;

            SpecVO = specVO;
            OptionData = optionData;

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
