using System;

namespace AloneSpace
{
    public static class WeaponDataHelper
    {
        public static WeaponData GetWeaponData(IWeaponSpecVO weaponSpecVO, ActorData actorData, int weaponIndex)
        {
            switch (weaponSpecVO)
            {
                case WeaponBulletMakerSpecVO rifleVO:
                    return new BulletMakerWeaponData(rifleVO, actorData, weaponIndex);
                case WeaponMissileMakerSpecVO missileVO:
                    return new MissileMakerWeaponData(missileVO, actorData, weaponIndex);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}