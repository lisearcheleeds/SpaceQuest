using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ParticleBulletMakerWeaponData : WeaponData
    {
        public override Guid InstanceId { get; }
        public override IOrderModule OrderModule { get; protected set; }
        public override IWeaponSpecVO WeaponSpecVO => VO;
        public override WeaponStateData WeaponStateData => ParticleBulletMakerWeaponStateData;

        public ParticleBulletMakerWeaponStateData ParticleBulletMakerWeaponStateData = new ParticleBulletMakerWeaponStateData();
        public WeaponParticleBulletMakerSpecVO VO { get; }

        public ParticleBulletMakerWeaponData(WeaponParticleBulletMakerSpecVO weaponParticleBulletMakerSpecVO, ActorData actorData, int weaponIndex) : base(actorData, weaponIndex)
        {
            InstanceId = Guid.NewGuid();
            VO = weaponParticleBulletMakerSpecVO;

            OrderModule = new ParticleBulletMakerWeaponOrderModule(this);
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
