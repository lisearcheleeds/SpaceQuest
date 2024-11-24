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

            // Name
            public string Name { get; }

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

            // マガジンサイズ
            public int MagazineSize { get; }

            // リロード時間(s)
            public float ReloadTime { get; }

            // 連射速度(f/s)
            public float FireRate { get; }

            // バーストサイズ
            public int BurstSize { get; }

            // 同時発射数
            public int ShotCount { get; }

            // 自動射撃
            public bool HasAutoFireMode { get; }

            // 水平発射
            public bool HorizontalLaunch { get; }

            // 発射スピード
            public float LaunchSpeed { get; }

            // ロックオン距離
            public float LockOnDistance { get; }

            // ロックオン角度
            public float LockOnAngle { get; }
            
            // 撃ち切るかどうか
            public bool ShootUp { get; }

            public Row(
                int id,
                string name,
                AssetPath path,
                int missileWeaponEffectSpecMasterId,
                int explosionWeaponEffectSpecMasterId,
                int smokeGraphicEffectSpecMasterId,
                int explosionGraphicEffectSpecMasterId,
                int magazineSize,
                float reloadTime,
                float fireRate,
                int burstSize,
                int shotCount,
                bool hasAutoFireMode,
                bool horizontalLaunch,
                float launchSpeed,
                float lockOnDistance,
                float lockOnAngle,
                bool shootUp)
            {
                Id = id;
                Name = name;
                Path = path;
                MissileWeaponEffectSpecMasterId = missileWeaponEffectSpecMasterId;
                ExplosionWeaponEffectSpecMasterId = explosionWeaponEffectSpecMasterId;
                SmokeGraphicEffectSpecMasterId = smokeGraphicEffectSpecMasterId;
                ExplosionGraphicEffectSpecMasterId = explosionGraphicEffectSpecMasterId;
                MagazineSize = magazineSize;
                ReloadTime = reloadTime;
                FireRate = fireRate;
                BurstSize = burstSize;
                ShotCount = shotCount;
                HasAutoFireMode = hasAutoFireMode;
                HorizontalLaunch = horizontalLaunch;
                LaunchSpeed = launchSpeed;
                LockOnDistance = lockOnDistance;
                LockOnAngle = lockOnAngle;
                ShootUp = shootUp;
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
                new Row(1, "MissileMaker1", new AssetPath("Prefab/Weapon/MissileMaker1"), 1, 1, 1, 2, 4, 5.0f, 0.15f, 2, 1, true, true, 0.3f, 1000.0f, 30.0f, false),
                new Row(2, "MissileMaker2", new AssetPath("Prefab/Weapon/MissileMaker2"), 2, 1, 1, 2, 24, 8.0f, 0.1f, 24, 1, true, false, 0.3f, 1000.0f, 180.0f, true),
                new Row(3, "MissileMaker3", new AssetPath("Prefab/Weapon/MissileMaker2"), 3, 1, 1, 2, 12, 6.0f, 0.1f, 12, 1, true, false, 0.3f, 1000.0f, 180.0f, true),
            };
        }
    }
}
