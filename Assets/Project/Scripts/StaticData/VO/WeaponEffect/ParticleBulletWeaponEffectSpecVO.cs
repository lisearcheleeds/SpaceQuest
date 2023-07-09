namespace AloneSpace
{
    /// <summary>
    /// 基本スペックと品質
    /// </summary>
    public class ParticleBulletWeaponEffectSpecVO : IWeaponEffectSpecVO
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

        ParticleBulletWeaponEffectSpecMaster.Row row;

        public ParticleBulletWeaponEffectSpecVO(int id)
        {
            row = ParticleBulletWeaponEffectSpecMaster.Instance.Get(id);
        }
    }
}
