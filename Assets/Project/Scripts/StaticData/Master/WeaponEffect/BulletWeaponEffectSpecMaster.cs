using System.Linq;

namespace AloneSpace
{
    public class BulletWeaponEffectSpecMaster
    {
        public class Row : IWeaponSpecMaster
        {
            // ID
            public int Id { get; }

            // Path
            public CacheableGameObjectPath Path { get; }

            // BaseDamage
            public float BaseDamage { get; }

            // 速度
            public float Speed { get; }

            // 継続時間
            public float LifeTime { get; }
            
            // 有効射程距離
            public float EffectiveDistance { get; }

            // 衝突判定スケール
            public float SizeScale { get; }

            // オブジェクト貫通力
            public float Penetration { get; }

            public Row(
                int id,
                CacheableGameObjectPath path,
                float baseDamage,
                float speed,
                float lifeTime,
                float effectiveDistance,
                float sizeScale,
                float penetration)
            {
                Id = id;
                Path = path;
                BaseDamage = baseDamage;
                Speed = speed;
                LifeTime = lifeTime;
                EffectiveDistance = effectiveDistance;
                SizeScale = sizeScale;
                Penetration = penetration;
            }
        }

        Row[] rows;
        static BulletWeaponEffectSpecMaster instance;

        public static BulletWeaponEffectSpecMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BulletWeaponEffectSpecMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        BulletWeaponEffectSpecMaster()
        {
            rows = new[]
            {
                new Row(1, new CacheableGameObjectPath("Prefab/WeaponEffect/Projectile/Bullet/Bullet"), 2, 300.0f, 8.0f, 1.0f, 1000.0f, 0.0f),
                new Row(2, new CacheableGameObjectPath("Prefab/WeaponEffect/Projectile/Bullet/Bullet"), 2, 300.0f, 8.0f, 1.0f, 1000.0f, 0.0f),
            };
        }
    }
}
