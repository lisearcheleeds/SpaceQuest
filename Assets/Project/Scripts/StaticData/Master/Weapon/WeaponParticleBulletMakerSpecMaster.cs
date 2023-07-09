using System.Linq;

namespace AloneSpace
{
    public class WeaponParticleBulletMakerSpecMaster
    {
        public class Row : IWeaponSpecMaster
        {
            // ID
            public int Id { get; }

            // Name
            public string Name { get; }

            // Path
            public AssetPath Path { get; }

            // WeaponEffect
            public int ParticleBulletWeaponEffectSpecMasterId { get; }

            // マガジンサイズ
            public int MagazineSize { get; }

            // リロード時間(s)
            public float ReloadTime { get; }

            // 連射速度(f/s)
            public float FireRate { get; }

            // 精度(1.0f/Accuracy)
            public float Accuracy { get; }

            // 射角(0.0f ~ 180.0f)
            public float AngleOfFire { get; }

            // バーストサイズ
            public int BurstSize { get; }

            // 同時発射数
            public int ShotCount { get; }

            // 予測射撃
            public bool IsPredictiveShoot { get; }

            // 自動射撃
            public bool HasAutoFireMode { get; }

            // 旋回速度
            public float TurningSpeed { get; }

            // 撃ち切るかどうか
            public bool ShootUp { get; }

            public Row(
                int id,
                string name,
                AssetPath path,
                int particleBulletWeaponEffectSpecMasterId,
                int magazineSize,
                float reloadTime,
                float fireRate,
                float accuracy,
                float angleOfFire,
                int burstSize,
                int shotCount,
                bool isPredictiveShoot,
                bool hasAutoFireMode,
                float turningSpeed,
                bool shootUp)
            {
                Id = id;
                Name = name;
                Path = path;
                ParticleBulletWeaponEffectSpecMasterId = particleBulletWeaponEffectSpecMasterId;
                MagazineSize = magazineSize;
                ReloadTime = reloadTime;
                FireRate = fireRate;
                Accuracy = accuracy;
                AngleOfFire = angleOfFire;
                BurstSize = burstSize;
                ShotCount = shotCount;
                IsPredictiveShoot = isPredictiveShoot;
                HasAutoFireMode = hasAutoFireMode;
                TurningSpeed = turningSpeed;
                ShootUp = shootUp;
            }
        }

        Row[] rows;
        static WeaponParticleBulletMakerSpecMaster instance;

        public static WeaponParticleBulletMakerSpecMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WeaponParticleBulletMakerSpecMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        WeaponParticleBulletMakerSpecMaster()
        {
            rows = new[]
            {
                new Row(1, "ParticleBulletMaker1", new AssetPath("Prefab/Weapon/ParticleBulletMaker1"), 1, 60, 3.0f, 0.05f, 100.0f, 90.0f, 60, 1, true, true, 150.0f, false),
                new Row(2, "ParticleBulletMaker2", new AssetPath("Prefab/Weapon/ParticleBulletMaker2"), 1, 60, 3.0f, 1.0f, 100.0f, 90.0f, 60, 1, true, true, 150.0f, false),
            };
        }
    }
}
