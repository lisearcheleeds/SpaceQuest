using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class BulletMakerWeaponData : WeaponData
    {
        public override Guid InstanceId { get; }
        public override IOrderModule OrderModule { get; }
        public override IActorPartsWeaponParameterVO ActorPartsWeaponParameterVO => ParameterVO;
        public override WeaponStateData WeaponStateData { get; } = new BulletMakerWeaponStateData();
        
        public ActorPartsWeaponBulletMakerParameterVO ParameterVO { get; }
        
        public BulletMakerWeaponData(ActorPartsWeaponBulletMakerParameterVO actorPartsWeaponBulletMakerParameterVO)
        {
            InstanceId = Guid.NewGuid();
            OrderModule = new BulletMakerWeaponOrderModule(this);
            ParameterVO = actorPartsWeaponBulletMakerParameterVO;
        }

        public override void Reload()
        {
            WeaponStateData.ResourceIndex = 0;
            WeaponStateData.ReloadTime += ParameterVO.ReloadTime;
        }
    }
}