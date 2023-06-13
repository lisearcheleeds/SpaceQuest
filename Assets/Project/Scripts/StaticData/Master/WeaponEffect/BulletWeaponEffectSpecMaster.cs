﻿using System.Linq;

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

            public Row(
                int id,
                CacheableGameObjectPath path,
                float baseDamage,
                float speed)
            {
                Id = id;
                Path = path;
                BaseDamage = baseDamage;
                Speed = speed;
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
                new Row(1, new CacheableGameObjectPath("Prefab/WeaponEffect/Projectile/Bullet/Bullet"), 2, 200.0f),
                new Row(2, new CacheableGameObjectPath("Prefab/WeaponEffect/Projectile/Bullet/Bullet"), 2, 200.0f),
            };
        }
    }
}