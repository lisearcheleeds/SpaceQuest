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

            // 射出物アセット
            public int WeaponEffectAssetId { get; }

            // マガジンサイズ
            public int WeaponResourceMaxCount { get; }

            // リロード時間
            public float ReloadTime { get; }

            // 連射速度(f/s)
            public float FireRate { get; }

            // 精度(0.0 ~ 1.0f)
            public float Accuracy { get; }

            // 速度
            public float Speed { get; }

            public Row(
                int id,
                AssetPath path,
                int weaponEffectAssetId,
                int weaponResourceMaxCount,
                float reloadTime,
                float fireRate,
                float accuracy,
                float speed)
            {
                Id = id;
                Path = path;
                WeaponEffectAssetId = weaponEffectAssetId;
                WeaponResourceMaxCount = weaponResourceMaxCount;
                ReloadTime = reloadTime;
                FireRate = fireRate;
                Accuracy = accuracy;
                Speed = speed;
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
                new Row(1, new AssetPath("Prefab/Weapon/BulletMaker1"), 1, 60, 3.0f, 0.05f, 1000.0f, 200.0f),
                new Row(2, new AssetPath("Prefab/Weapon/BulletMaker2"), 1, 60, 3.0f, 1.0f, 100.0f, 200.0f),
            };
        }
    }
}
