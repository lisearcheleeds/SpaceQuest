namespace AloneSpace
{
    public class BulletWeaponEffectSpecVO : IWeaponEffectSpecVO
    {
        // Path
        public CacheableGameObjectPath Path => row.Path;

        // BaseDamage
        public float BaseDamage => row.BaseDamage;

        // 速度
        public float Speed => row.Speed;

        BulletWeaponEffectSpecMaster.Row row;

        public BulletWeaponEffectSpecVO(int id)
        {
            row = BulletWeaponEffectSpecMaster.Instance.Get(id);
        }
    }
}
