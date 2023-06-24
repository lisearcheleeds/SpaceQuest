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

        // 継続時間
        public float LifeTime => row.LifeTime;

        // 衝突判定スケール
        public float SizeScale => row.SizeScale;

        // オブジェクト貫通力
        public float Penetration => row.Penetration;

        BulletWeaponEffectSpecMaster.Row row;

        public BulletWeaponEffectSpecVO(int id)
        {
            row = BulletWeaponEffectSpecMaster.Instance.Get(id);
        }
    }
}
