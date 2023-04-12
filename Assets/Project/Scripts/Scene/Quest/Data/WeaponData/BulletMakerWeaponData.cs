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
        
        public ActorPartsWeaponRifleParameterVO ParameterVO { get; }
        
        public BulletMakerWeaponData(ActorPartsWeaponRifleParameterVO actorPartsWeaponRifleParameterVO)
        {
            InstanceId = Guid.NewGuid();
            OrderModule = new BulletMakerWeaponOrderModule(this);
            ParameterVO = actorPartsWeaponRifleParameterVO;
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
            return WeaponStateData.ReloadTime == 0;
        }

        public override void Execute(IPositionData targetData)
        {
            this.WeaponStateData.TargetData = targetData;
        }
    }
}