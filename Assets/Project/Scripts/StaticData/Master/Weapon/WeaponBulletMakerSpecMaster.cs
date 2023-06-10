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
            public int WeaponResourceMaxCount { get; }

            // リロード時間
            public float ReloadTime { get; }

            // 連射速度(f/s)
            public float FireRate { get; }

            // 精度(0.0 ~ 1.0f)
            public float Accuracy { get; }

            public Row(
                int id,
                AssetPath path,
                int bulletWeaponEffectSpecMasterId,
                int weaponResourceMaxCount,
                float reloadTime,
                float fireRate,
                float accuracy)
            {
                Id = id;
                Path = path;
                BulletWeaponEffectSpecMasterId = bulletWeaponEffectSpecMasterId;
                WeaponResourceMaxCount = weaponResourceMaxCount;
                ReloadTime = reloadTime;
                FireRate = fireRate;
                Accuracy = accuracy;
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
                new Row(1, new AssetPath("Prefab/Weapon/BulletMaker1"), 1, 60, 3.0f, 0.05f, 1000.0f),
                new Row(2, new AssetPath("Prefab/Weapon/BulletMaker2"), 1, 60, 3.0f, 1.0f, 100.0f),
            };
        }
    }
}
