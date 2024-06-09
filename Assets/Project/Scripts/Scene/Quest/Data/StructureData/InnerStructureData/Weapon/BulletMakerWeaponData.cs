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
        public override WeaponStateData WeaponStateData => BulletMakerWeaponStateData;

        public override bool IsInfinityResource => false;

        public BulletMakerWeaponStateData BulletMakerWeaponStateData = new BulletMakerWeaponStateData();
        public WeaponBulletMakerSpecVO VO { get; }

        public BulletMakerWeaponData(WeaponBulletMakerSpecVO weaponBulletMakerSpecVO, ActorData actorData, int weaponIndex) : base(actorData, weaponIndex)
        {
            InstanceId = Guid.NewGuid();
            VO = weaponBulletMakerSpecVO;

            OrderModule = new BulletMakerWeaponOrderModule(this);
        }

        public override void ActivateModules()
        {
            OrderModule.ActivateModule();
        }

        public override void DeactivateModules()
        {
            OrderModule.DeactivateModule();

            // NOTE: 別にnull入れなくても良いがIsReleased見ずにModule見ようとしたらコケてくれるので
            OrderModule = null;
        }

        public override void Reload()
        {
            WeaponStateData.ReloadRemainTime = VO.ReloadTime;
        }
    }
}
