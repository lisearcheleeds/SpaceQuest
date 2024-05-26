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

            // リロード時間(s)
            public float ReloadTime { get; }

            // 連射速度(f/s)
            public float FireRate { get; }

            // 精度(1.0f/Accuracy)
            public float Accuracy { get; }

            // 射角(0.0f ~ 180.0f)
            public float AngleOfFire { get; }

            // 予測射撃
            public bool IsPredictiveShoot { get; }

            // 自動射撃
            public bool HasAutoFireMode { get; }

            public Row(
                int id,
                string name,
                AssetPath path,
                int particleBulletWeaponEffectSpecMasterId,
                float reloadTime,
                float fireRate,
                float accuracy,
                float angleOfFire,
                bool isPredictiveShoot,
                bool hasAutoFireMode)
            {
                Id = id;
                Name = name;
                Path = path;
                ParticleBulletWeaponEffectSpecMasterId = particleBulletWeaponEffectSpecMasterId;
                ReloadTime = reloadTime;
                FireRate = fireRate;
                Accuracy = accuracy;
                AngleOfFire = angleOfFire;
                IsPredictiveShoot = isPredictiveShoot;
                HasAutoFireMode = hasAutoFireMode;
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
                new Row(1, "ParticleBulletMaker1", new AssetPath("Prefab/Weapon/ParticleBulletMaker1"), 1, 3.0f, 0.24f, 1000.0f, 15.0f, true, true),
                new Row(2, "ParticleBulletMaker2", new AssetPath("Prefab/Weapon/ParticleBulletMaker2"), 1, 3.0f, 0.12f, 50.0f, 180.0f, true, true),
            };
        }
    }
}
