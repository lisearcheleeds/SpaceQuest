using System;
using System.Linq;

namespace AloneSpace
{
    public class ActorPresetVO
    {
        public ActorSpecVO ActorSpecVO { get; }
        public IWeaponSpecVO[] WeaponSpecVOs { get; }

        public ActorPresetVO(int actorPresetId)
        {
            var actorPreset = ActorPresetMaster.Instance.Get(actorPresetId);
            var actorPresetWeapons = ActorPresetWeaponMaster.Instance.GetRange(actorPresetId);

            ActorSpecVO = new ActorSpecVO(actorPreset.ActorSpecId);
            WeaponSpecVOs = actorPresetWeapons.Select(x =>
            {
                switch (x.WeaponType)
                {
                    case WeaponType.BulletMaker: return (IWeaponSpecVO) new WeaponBulletMakerSpecVO(x.WeaponSpecId);
                    case WeaponType.ParticleBulletMaker: return (IWeaponSpecVO) new WeaponParticleBulletMakerSpecVO(x.WeaponSpecId);
                    case WeaponType.MissileMaker: return new WeaponMissileMakerSpecVO(x.WeaponSpecId);
                    default: throw new ArgumentException();
                }
            }).ToArray();
        }
    }
}
