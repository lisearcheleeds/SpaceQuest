using RoboQuest;

namespace AloneSpace
{
    public class DamageData
    {
        public WeaponData WeaponData { get; }
        public ItemVO ResourceItemVO { get; }
        public IDamageableData DamageableData { get; }

        public int DamageValue { get; }

        public DamageData(WeaponData weaponData, ItemVO resourceItemVO, IDamageableData damageableData)
        {
            WeaponData = weaponData;
            ResourceItemVO = resourceItemVO;
            DamageableData = damageableData;

            DamageValue = 1;
        }
    }
}