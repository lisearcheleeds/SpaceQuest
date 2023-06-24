namespace AloneSpace
{
    public class MissileWeaponEffectSpecVO : IWeaponEffectSpecVO
    {
        // Path
        public CacheableGameObjectPath Path => row.Path;

        // BaseDamage
        public float BaseDamage => row.BaseDamage;

        // 射出待機時間
        public float LaunchWaitTime => row.LaunchWaitTime;

        // 誘導角
        public float HomingAngle => row.HomingAngle;

        // 速度
        public float Speed => row.Speed;

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
