using System;
using System.Linq;

namespace AloneSpace
{
    public class WeaponMissileMakerSpecMaster
    {
        public class Row : IWeaponSpecMaster
        {
            // ID
            public int Id { get; }

            // Path
            public AssetPath Path { get; }

            // 射出物アセット
            public int MissileWeaponEffectSpecMasterId { get; }

            // 爆発物アセット
            public int ExplosionWeaponEffectSpecMasterId { get; }

            // 煙グラフィックアセット
            public int SmokeGraphicEffectSpecMasterId { get; }

            // 爆発グラフィックアセット
            public int ExplosionGraphicEffectSpecMasterId { get; }

            // 一度に使用するリソース
            public int WeaponResourceMaxCount { get; }

            // リロード時間
            public float ReloadTime { get; }

            // 連続射出時間
            public float FireRate { get; }

            public Row(
                int id,
                AssetPath path,
                int missileWeaponEffectSpecMasterId,
                int explosionWeaponEffectSpecMasterId,
                int smokeGraphicEffectSpecMasterId,
                int explosionGraphicEffectSpecMasterId,
                int weaponResourceMaxCount,
                float reloadTime,
                float fireRate)
            {
                Id = id;
                Path = path;
                MissileWeaponEffectSpecMasterId = missileWeaponEffectSpecMasterId;
                ExplosionWeaponEffectSpecMasterId = explosionWeaponEffectSpecMasterId;
                SmokeGraphicEffectSpecMasterId = smokeGraphicEffectSpecMasterId;
                ExplosionGraphicEffectSpecMasterId = explosionGraphicEffectSpecMasterId;
                WeaponResourceMaxCount = weaponResourceMaxCount;
                ReloadTime = reloadTime;
                FireRate = fireRate;
            }
        }

        Row[] rows;
        static WeaponMissileMakerSpecMaster instance;

        public static WeaponMissileMakerSpecMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WeaponMissileMakerSpecMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        WeaponMissileMakerSpecMaster()
        {
            rows = new[]
            {
                new Row(1, new AssetPath("Prefab/Weapon/MissileMaker1"), 1, 1, 1, 2, 4, 5.0f, 0.15f),
                new Row(2, new AssetPath("Prefab/Weapon/MissileMaker2"), 2, 1, 1, 2, 12, 4.0f, 0.1f),
            };
        }
    }
}
