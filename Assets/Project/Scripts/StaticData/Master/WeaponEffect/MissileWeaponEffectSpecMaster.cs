using System.Linq;

namespace AloneSpace
{
    public class MissileWeaponEffectSpecMaster
    {
        public class Row : IWeaponSpecMaster
        {
            // ID
            public int Id { get; }

            // Path
            public CacheableGameObjectPath Path { get; }

            // BaseDamage
            public float BaseDamage { get; }

            // 射出待機時間
            public float LaunchWaitTime { get; }

            // 誘導角(s)
            public float HomingAngle { get; }

            // 速度(s)
            public float Speed { get; }

            // 継続時間
            public float LifeTime { get; }

            // 衝突判定スケール
            public float SizeScale { get; }

            public Row(
                int id,
                CacheableGameObjectPath path,
                float baseDamage,
                float launchWaitTime,
                float homingAngle,
                float speed,
                float lifeTime,
                float sizeScale)
            {
                Id = id;
                Path = path;
                BaseDamage = baseDamage;
                LaunchWaitTime = launchWaitTime;
                HomingAngle = homingAngle;
                Speed = speed;
                LifeTime = lifeTime;
                SizeScale = sizeScale;
            }
        }

        Row[] rows;
        static MissileWeaponEffectSpecMaster instance;

        public static MissileWeaponEffectSpecMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MissileWeaponEffectSpecMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        MissileWeaponEffectSpecMaster()
        {
            rows = new[]
            {
                new Row(1, new CacheableGameObjectPath("Prefab/WeaponEffect/Projectile/Missile/MiddleMissile"), 1, 0.5f, 150.0f, 150.0f, 8.0f, 1.0f),
                new Row(2, new CacheableGameObjectPath("Prefab/WeaponEffect/Projectile/Missile/ShortMissile"), 1, 0.5f, 150.0f, 150.0f, 8.0f, 1.0f),
            };
        }
    }
}
