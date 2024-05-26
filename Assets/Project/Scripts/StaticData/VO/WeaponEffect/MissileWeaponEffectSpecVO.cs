namespace AloneSpace
{
    /// <summary>
    /// 基本スペックと品質
    /// </summary>
    public class MissileWeaponEffectSpecVO : IWeaponEffectSpecVO
    {
        // Id
        public int Id => row.Id;

        // Path
        public CacheableGameObjectPath Path => row.Path;

        // WeaponEffectType
        public WeaponEffectType WeaponEffectType => WeaponEffectType.Missile;

        // BaseDamage
        public float BaseDamage => row.BaseDamage;

        // 射出待機時間
        public float LaunchWaitTime => row.LaunchWaitTime;

        // 誘導角
        public float HomingAngle => row.HomingAngle;

        // 速度
        public float Speed => row.Speed;
        
        // 有効射程距離
        public float EffectiveDistance => row.EffectiveDistance;

        // 継続時間
        public float LifeTime => row.LifeTime;

        // 衝突判定スケール
        public float SizeScale => row.SizeScale;

        MissileWeaponEffectSpecMaster.Row row;

        public MissileWeaponEffectSpecVO(int id)
        {
            row = MissileWeaponEffectSpecMaster.Instance.Get(id);
        }
    }
}
