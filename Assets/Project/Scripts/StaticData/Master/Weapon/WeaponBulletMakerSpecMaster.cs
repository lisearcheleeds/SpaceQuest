using System.Linq;

namespace AloneSpace
{
    public class WeaponBulletMakerSpecMaster
    {
        public class Row : IWeaponSpecMaster
        {
            // ID
            public int Id { get; }

            // Path
            public AssetPath Path { get; }

            // WeaponEffect
            public int BulletWeaponEffectSpecMasterId { get; }

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
                AssetPath path,
                int bulletWeaponEffectSpecMasterId,
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
                Path = path;
                BulletWeaponEffectSpecMasterId = bulletWeaponEffectSpecMasterId;
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
        static WeaponBulletMakerSpecMaster instance;

        public static WeaponBulletMakerSpecMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WeaponBulletMakerSpecMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        WeaponBulletMakerSpecMaster()
        {
            rows = new[]
            {
                new Row(1, new AssetPath("Prefab/Weapon/BulletMaker1"), 1, 60, 3.0f, 0.05f, 1000.0f, 90.0f, 60, 1, true, true, 150.0f, false),
                new Row(2, new AssetPath("Prefab/Weapon/BulletMaker2"), 1, 60, 3.0f, 1.0f, 100.0f, 90.0f, 60, 1, true, true, 150.0f, false),
            };
        }
    }
}
