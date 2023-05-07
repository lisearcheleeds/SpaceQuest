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
                int weaponEffectAssetId,
                int weaponResourceMaxCount,
                float reloadTime,
                float fireRate,
                float accuracy,
                float speed)
            {
                Id = id;
                WeaponEffectAssetId = weaponEffectAssetId;
                WeaponResourceMaxCount = weaponResourceMaxCount;
                ReloadTime = reloadTime;
                FireRate = fireRate;
                Accuracy = accuracy;
                Speed = speed;
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
                new Row(1, 1, 60, 3.0f, 0.05f, 100.0f, 200.0f),
                new Row(2, 1, 60, 3.0f, 1.0f, 100.0f, 200.0f),
            };
        }
    }
}
