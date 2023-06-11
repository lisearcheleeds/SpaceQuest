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

            // 射出初速
            public float LaunchSpeed { get; }

            // 速度
            public float Speed { get; }

            public Row(
                int id,
                CacheableGameObjectPath path,
                float baseDamage,
                float launchSpeed,
                float speed)
            {
                Id = id;
                Path = path;
                BaseDamage = baseDamage;
                LaunchSpeed = launchSpeed;
                Speed = speed;
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
                new Row(1, new CacheableGameObjectPath("Prefab/WeaponEffect/Projectile/Missile/MiddleMissile"), 1, 1.0f, 80.0f),
                new Row(2, new CacheableGameObjectPath("Prefab/WeaponEffect/Projectile/Missile/ShortMissile"), 1, 1.0f, 80.0f),
            };
        }
    }
}
