namespace AloneSpace
{
    public class ExplosionWeaponEffectSpecVO : IWeaponEffectSpecVO
    {
        // Path
        public CacheableGameObjectPath Path => row.Path;

        ExplosionWeaponEffectSpecMaster.Row row;

        public ExplosionWeaponEffectSpecVO(int id)
        {
            row = ExplosionWeaponEffectSpecMaster.Instance.Get(id);
        }
    }
}