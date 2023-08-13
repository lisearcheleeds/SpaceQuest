using System;
using System.Linq;

namespace AloneSpace
{
    public class ActorPresetVO
    {
        public ActorSpecVO ActorSpecVO { get; }
        public IWeaponSpecVO[] WeaponSpecVOs { get; }
        public SpecialEffectSpecVO[] SpecialEffectSpecVOs { get; }

        public ActorPresetVO(int actorPresetId)
        {
            var actorPreset = ActorPresetMaster.Instance.Get(actorPresetId);
            var actorPresetWeapons = ActorPresetWeaponMaster.Instance.GetRange(actorPresetId);

            ActorSpecVO = new ActorSpecVO(actorPreset.ActorSpecId);
            WeaponSpecVOs = Enumerable.Range(0, ActorSpecVO.WeaponSlotCount).Select(index =>
            {
                var weapon = actorPresetWeapons.FirstOrDefault(x => x.WeaponIndex == index);
                switch (weapon?.WeaponType)
                {
                    case WeaponType.BulletMaker: return (IWeaponSpecVO) new WeaponBulletMakerSpecVO(weapon.WeaponSpecId);
                    case WeaponType.ParticleBulletMaker: return (IWeaponSpecVO) new WeaponParticleBulletMakerSpecVO(weapon.WeaponSpecId);
                    case WeaponType.MissileMaker: return new WeaponMissileMakerSpecVO(weapon.WeaponSpecId);
                    default: return null;
                }
            }).ToArray();

            var specialEffectMasterRows = ActorPresetSpecialEffectRelationMaster.Instance.GetRange(actorPresetId);
            SpecialEffectSpecVOs = specialEffectMasterRows.Select(x => new SpecialEffectSpecVO(x.SpecialEffectId)).ToArray();
        }
    }
}
