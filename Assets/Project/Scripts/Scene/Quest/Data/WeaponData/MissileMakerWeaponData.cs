using System;
using AloneSpace;
using UnityEditor;
using UnityEngine;

namespace AloneSpace
{
    public class MissileMakerWeaponData : WeaponData
    {
        public override Guid InstanceId { get; }
        public override IOrderModule OrderModule { get; protected set; }
        public override IWeaponSpecVO WeaponSpecVO => VO;
        public override WeaponStateData WeaponStateData { get; } = new MissileMakerWeaponStateData();

        public WeaponMissileMakerSpecVO VO { get; }

        public MissileMakerWeaponData(WeaponMissileMakerSpecVO weaponMissileMakerSpecVO)
        {
            InstanceId = Guid.NewGuid();
            VO = weaponMissileMakerSpecVO;

            ActivateModules();
        }

        public override void ActivateModules()
        {
            OrderModule = new MissileMakerWeaponOrderModule(this);
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