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
        public override IActorPartsWeaponParameterVO ActorPartsWeaponParameterVO => ParameterVO;
        public override WeaponStateData WeaponStateData { get; } = new MissileMakerWeaponStateData();
        
        public ActorPartsWeaponMissileMakerParameterVO ParameterVO { get; }
        
        public MissileMakerWeaponData(ActorPartsWeaponMissileMakerParameterVO actorPartsWeaponMissileMakerParameterVO)
        {
            InstanceId = Guid.NewGuid();
            ParameterVO = actorPartsWeaponMissileMakerParameterVO;
                
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
            WeaponStateData.ResourceIndex = 0;
            WeaponStateData.ReloadTime += ParameterVO.ReloadTime;
        }
    }
}