using System;

namespace AloneSpace
{
    public class MissileMakerWeaponData : WeaponData
    {
        public override Guid InstanceId { get; }
        public override IOrderModule OrderModule { get; protected set; }
        public override IWeaponSpecVO WeaponSpecVO => VO;
        public override WeaponStateData WeaponStateData => MissileMakerWeaponStateData;

        public MissileMakerWeaponStateData MissileMakerWeaponStateData { get; } = new MissileMakerWeaponStateData();
        public WeaponMissileMakerSpecVO VO { get; }

        public MissileMakerWeaponData(WeaponMissileMakerSpecVO weaponMissileMakerSpecVO, ActorData actorData, int weaponIndex) : base(actorData, weaponIndex)
        {
            InstanceId = Guid.NewGuid();
            VO = weaponMissileMakerSpecVO;

            OrderModule = new MissileMakerWeaponOrderModule(this);
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
