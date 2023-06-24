namespace AloneSpace
{
    public class ExplosionWeaponEffectSpecVO : IWeaponEffectSpecVO
    {
        // Path
        public CacheableGameObjectPath Path => row.Path;

        // BaseDamage
        public float BaseDamage => row.BaseDamage;

        // 衝突判定スケール
        public float SizeScale => row.SizeScale;

        ExplosionWeaponEffectSpecMaster.Row row;

        public ExplosionWeaponEffectSpecVO(int id)
        {
            row = ExplosionWeaponEffectSpecMaster.Instance.Get(id);
        }
    }
}
