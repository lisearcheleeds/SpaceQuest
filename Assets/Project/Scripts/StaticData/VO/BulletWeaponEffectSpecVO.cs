namespace AloneSpace
{
    public class BulletWeaponEffectSpecVO : IWeaponEffectSpecVO
    {
        // Path
        public CacheableGameObjectPath Path => row.Path;

        // 速度
        public float Speed => row.Speed;

        BulletWeaponEffectSpecMaster.Row row;

        public BulletWeaponEffectSpecVO(int id)
        {
            row = BulletWeaponEffectSpecMaster.Instance.Get(id);
        }
    }
}
