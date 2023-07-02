namespace AloneSpace
{
    /// <summary>
    /// 基本スペックと品質
    /// </summary>
    public class ExplosionWeaponEffectSpecVO : IWeaponEffectSpecVO
    {
        // Id
        public int Id => row.Id;

        // Path
        public CacheableGameObjectPath Path => row.Path;

        // WeaponEffectType
        public WeaponEffectType WeaponEffectType => WeaponEffectType.Explosion;

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
