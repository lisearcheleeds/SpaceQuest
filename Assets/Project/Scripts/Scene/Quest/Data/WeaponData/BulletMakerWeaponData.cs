using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class BulletMakerWeaponData : WeaponData
    {
        public override Guid InstanceId { get; }
        public override IOrderModule OrderModule { get; protected set; }
        public override IWeaponSpecVO WeaponSpecVO => VO;
        public override WeaponStateData WeaponStateData { get; } = new BulletMakerWeaponStateData();

        public WeaponBulletMakerSpecVO VO { get; }

        public BulletMakerWeaponData(WeaponBulletMakerSpecVO weaponBulletMakerSpecVO, ActorData actorData, int weaponIndex) : base(actorData, weaponIndex)
        {
            InstanceId = Guid.NewGuid();
            VO = weaponBulletMakerSpecVO;

            ActivateModules();
        }

        public override void ActivateModules()
        {
            OrderModule = new BulletMakerWeaponOrderModule(this);
            OrderModule.ActivateModule();
        }

        public override void DeactivateModules()
        {
            OrderModule.DeactivateModule();
            OrderModule = null;
        }

        public override void Reload()
        {
            WeaponStateData.ReloadRemainTime += VO.ReloadTime;
        }
    }
}