using System;
using AloneSpace;
using UnityEditor;
using UnityEngine;

namespace AloneSpace
{
    public class MissileMakerWeaponData : WeaponData
    {
     
        public override Guid InstanceId { get; }
        public override IOrderModule OrderModule { get; }
        public override IActorPartsWeaponParameterVO ActorPartsWeaponParameterVO => ParameterVO;
        
        public ActorPartsWeaponMissileLauncherParameterVO ParameterVO { get; }
        public IPositionData TargetData { get; set; }
        
        public MissileMakerWeaponData(ActorPartsWeaponMissileLauncherParameterVO actorPartsWeaponMissileLauncherParameterVO)
        {
            InstanceId = Guid.NewGuid();
            OrderModule = new MissileMakerWeaponOrderModule(this);
            ParameterVO = actorPartsWeaponMissileLauncherParameterVO;
        }
        
        public override float GetAvailability()
        {
            if (WeaponStateData.ReloadTime != 0)
            {
                return 0.0f;
            }

            return 1.0f;
        }

        public override bool IsReloadable()
        {
            return WeaponStateData.ReloadTime == 0 && WeaponStateData.ResourceIndex != 0;
        }
        
        public override void Reload()
        {
            WeaponStateData.ResourceIndex = 0;
            WeaponStateData.ReloadTime += ParameterVO.ReloadTime;
        }

        public override bool IsExecutable(IPositionData targetData)
        {
            return WeaponStateData.ReloadTime == 0 && WeaponStateData.FireTime == 0 && WeaponStateData.ResourceIndex != 0 && targetData != null;
        }

        public override void Execute(IPositionData targetData)
        {
            this.TargetData = targetData;
        }
    }
}