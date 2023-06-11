namespace AloneSpace
{
    public class MissileWeaponEffectSpecVO : IWeaponEffectSpecVO
    {
        // Path
        public CacheableGameObjectPath Path => row.Path;

        // BaseDamage
        public float BaseDamage => row.BaseDamage;

        // 射出初速
        public float LaunchSpeed => row.LaunchSpeed;

        // 速度
        public float Speed => row.Speed;

        MissileWeaponEffectSpecMaster.Row row;

        public MissileWeaponEffectSpecVO(int id)
        {
            row = MissileWeaponEffectSpecMaster.Instance.Get(id);
        }
    }
}
