using System;

namespace AloneSpace
{
    public static class WeaponDataHelper
    {
        public static WeaponData GetWeaponData(IActorPartsWeaponParameterVO actorPartsWeaponParameterVO)
        {
            switch (actorPartsWeaponParameterVO)
            {
                case ActorPartsWeaponRifleParameterVO rifleVO:
                    return new BulletMakerWeaponData(rifleVO);
                case ActorPartsWeaponMissileLauncherParameterVO missileVO:
                    return new MissileMakerWeaponData(missileVO);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}