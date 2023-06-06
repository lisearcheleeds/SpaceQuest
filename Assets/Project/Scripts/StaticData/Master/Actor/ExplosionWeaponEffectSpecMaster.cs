﻿using System.Linq;

namespace AloneSpace
{
    public class ExplosionWeaponEffectSpecMaster
    {
        public class Row : IWeaponSpecMaster
        {
            // ID
            public int Id { get; }

            // Path
            public CacheableGameObjectPath Path { get; }

            public Row(
                int id,
                CacheableGameObjectPath path)
            {
                Id = id;
                Path = path;
            }
        }

        Row[] rows;
        static ExplosionWeaponEffectSpecMaster instance;

        public static ExplosionWeaponEffectSpecMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExplosionWeaponEffectSpecMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ExplosionWeaponEffectSpecMaster()
        {
            rows = new[]
            {
                new Row(1, new CacheableGameObjectPath("Prefab/WeaponEffect/Explosion/MiddleMissileExplosion")),
            };
        }
    }
}
