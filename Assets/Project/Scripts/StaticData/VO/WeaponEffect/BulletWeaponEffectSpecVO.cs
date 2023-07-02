namespace AloneSpace
{
    /// <summary>
    /// 基本スペックと品質
    /// </summary>
    public class BulletWeaponEffectSpecVO : IWeaponEffectSpecVO
    {
        // Id
        public int Id => row.Id;

        // Path
        public CacheableGameObjectPath Path => row.Path;

        // WeaponEffectType
        public WeaponEffectType WeaponEffectType => WeaponEffectType.Bullet;

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
