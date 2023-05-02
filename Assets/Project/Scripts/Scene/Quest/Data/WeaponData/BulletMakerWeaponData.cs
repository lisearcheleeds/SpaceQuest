using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class BulletMakerWeaponData : WeaponData
    {
        public override Guid InstanceId { get; }
        public override IOrderModule OrderModule { get; protected set; }
        public override IActorPartsWeaponParameterVO ActorPartsWeaponParameterVO => ParameterVO;
        public override WeaponStateData WeaponStateData { get; } = new BulletMakerWeaponStateData();
        
        public ActorPartsWeaponBulletMakerParameterVO ParameterVO { get; }
        
        public BulletMakerWeaponData(ActorPartsWeaponBulletMakerParameterVO actorPartsWeaponBulletMakerParameterVO)
        {
            InstanceId = Guid.NewGuid();
            ParameterVO = actorPartsWeaponBulletMakerParameterVO;
                
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
            WeaponStateData.ReloadRemainTime += ParameterVO.ReloadTime;
        }
    }
}