namespace AloneSpace
{
    public class ExplosionWeaponEffectSpecVO : IWeaponEffectSpecVO
    {
        // Path
        public CacheableGameObjectPath Path => row.Path;

        // BaseDamage
        public float BaseDamage => row.BaseDamage;

        ExplosionWeaponEffectSpecMaster.Row row;

        public ExplosionWeaponEffectSpecVO(int id)
        {
            row = ExplosionWeaponEffectSpecMaster.Instance.Get(id);
        }
    }
}
