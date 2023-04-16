using System;

namespace AloneSpace
{
    public static class WeaponDataHelper
    {
        public static WeaponData GetWeaponData(IActorPartsWeaponParameterVO actorPartsWeaponParameterVO)
        {
            switch (actorPartsWeaponParameterVO)
            {
                case ActorPartsWeaponBulletMakerParameterVO rifleVO:
                    return new BulletMakerWeaponData(rifleVO);
                case ActorPartsWeaponMissileMakerParameterVO missileVO:
                    return new MissileMakerWeaponData(missileVO);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}