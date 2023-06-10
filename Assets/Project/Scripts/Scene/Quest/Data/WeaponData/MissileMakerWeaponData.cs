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

        public GraphicMissileMakerSpecVO VO { get; }

        public MissileMakerWeaponData(GraphicMissileMakerSpecVO graphicMissileMakerSpecVO, ActorData actorData, int weaponIndex) : base(actorData, weaponIndex)
        {
            InstanceId = Guid.NewGuid();
            VO = graphicMissileMakerSpecVO;

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
            WeaponStateData.ReloadRemainTime = VO.ReloadTime;
        }
    }
}