using System.Linq;

namespace AloneSpace
{
    public class ActorPartsWeaponRifleParameterMaster
    {
        public class Row
        {
            // ID
            public int Id { get; }

            // 射出物アセット
            public int ProjectileAssetId { get; }

            // マガジンサイズ
            public int WeaponResourceMaxCount { get; }
            
            // リロード時間
            public float ReloadTime { get; }

            // 連射速度(f/s)
            public float FireRate { get; }
            
            // 精度(0.0 ~ 1.0f)
            public float Accuracy { get; }

            // 横反動抑制値(0.0 ~ 1.0f)
            public float HorizontalRecoilResist { get; }
            
            // 縦反動抑制値(0.0 ~ 1.0f)
            public float VerticalRecoilResist { get; }

            // 照準最大距離（弾の性能に寄って下がる）
            public float SightingMaxRange { get; }

            public Row(
                int id,
                int projectileAssetId,
                int weaponResourceMaxCount,
                float reloadTime,
                float fireRate,
                float accuracy,
                float horizontalRecoilResist,
                float verticalRecoilResist,
                float sightingMaxRange)
            {
                Id = id;
                ProjectileAssetId = projectileAssetId;
                WeaponResourceMaxCount = weaponResourceMaxCount;
                ReloadTime = reloadTime;
                FireRate = fireRate;
                Accuracy = accuracy;
                HorizontalRecoilResist = horizontalRecoilResist;
                VerticalRecoilResist = verticalRecoilResist;
                SightingMaxRange = sightingMaxRange;
            }
        }

        Row[] rows;
        static ActorPartsWeaponRifleParameterMaster instance;

        public static ActorPartsWeaponRifleParameterMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPartsWeaponRifleParameterMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ActorPartsWeaponRifleParameterMaster()
        {
            rows = new[]
            {
                new Row(1, 1, 12, 3.0f, 0.1f, 1.0f, 1.0f, 1.0f, 500),
                new Row(2, 1, 12, 3.0f, 1.0f, 1.0f, 1.0f, 1.0f, 500),
            };
        }
    }
}
