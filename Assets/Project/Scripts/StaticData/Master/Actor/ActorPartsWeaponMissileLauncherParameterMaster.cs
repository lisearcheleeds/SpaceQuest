using System.Linq;

namespace AloneSpace
{
    public class ActorPartsWeaponMissileLauncherParameterMaster
    {
        public class Row
        {
            // ID
            public int Id { get; }

            // 射出物アセット
            public int WeaponEffectAssetId { get; }
        
            // 一度に使用するリソース
            public int WeaponResourceMaxCount { get; }
            
            // リロード時間
            public float ReloadTime { get; }

            // 発射時の方向X
            // 設定されているとtargetの位置にかかわらずshooterの方向とBaseAngleに依存して射出する
            public float? LaunchAngleX { get; }
            
            // 発射時の方向Y
            // 設定されているとtargetの位置にかかわらずshooterの方向とBaseAngleに依存して射出する
            public float? LaunchAngleY { get; }
            
            // 射出初速
            public float LaunchSpeed { get; }
            
            // 速度
            public float Speed { get; }
            
            // 連続射出時間
            public float FireRate { get; }
            
            // 照準最大距離（弾の性能に寄って下がる）
            public float SightingMaxRange { get; }

            public Row(
                int id,
                int weaponEffectAssetId,
                int weaponResourceMaxCount,
                float reloadTime,
                float? launchAngleX,
                float? launchAngleY,
                float launchSpeed,
                float speed,
                float fireRate,
                float sightingMaxRange)
            {
                Id = id;
                WeaponEffectAssetId = weaponEffectAssetId;
                WeaponResourceMaxCount = weaponResourceMaxCount;
                ReloadTime = reloadTime;
                LaunchAngleX = launchAngleX;
                LaunchAngleY = launchAngleY;
                LaunchSpeed = launchSpeed;
                Speed = speed;
                FireRate = fireRate;
                SightingMaxRange = sightingMaxRange;
            }
        }

        Row[] rows;
        static ActorPartsWeaponMissileLauncherParameterMaster instance;

        public static ActorPartsWeaponMissileLauncherParameterMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPartsWeaponMissileLauncherParameterMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ActorPartsWeaponMissileLauncherParameterMaster()
        {
            rows = new[]
            {
                new Row(1, 2, 2, 4.0f, null, null, 1.0f, 80.0f, 1.0f, 500),
                new Row(2, 2, 4, 4.0f, null, null, 1.0f, 80.0f, 1.0f, 500),
            };
        }
    }
}
