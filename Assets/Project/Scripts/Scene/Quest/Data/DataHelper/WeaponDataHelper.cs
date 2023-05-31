using System;

namespace AloneSpace
{
    public static class WeaponDataHelper
    {
        public static WeaponData GetWeaponData(IWeaponSpecVO weaponSpecVO)
        {
            switch (weaponSpecVO)
            {
                case WeaponBulletMakerSpecVO rifleVO:
                    return new BulletMakerWeaponData(rifleVO);
                case WeaponMissileMakerSpecVO missileVO:
                    return new MissileMakerWeaponData(missileVO);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}