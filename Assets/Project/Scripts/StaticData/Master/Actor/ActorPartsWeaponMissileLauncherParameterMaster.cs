using System.Linq;

namespace AloneSpace
{
    public class ActorPartsWeaponMissileLauncherParameterMaster
    {
        public class Row
        {
            // ID
            public int Id { get; }

            // 射出する弾の種類
            public AmmoType AmmoType { get; }

            // 射出物アセット
            public int ProjectileAssetId { get; }
        
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
            
            // 連続射出時間
            public float LaunchIntervalTime { get; }
            
            // 照準最大距離（弾の性能に寄って下がる）
            public float SightingMaxRange { get; }

            public Row(
                int id,
                AmmoType ammoType,
                int projectileAssetId,
                int weaponResourceMaxCount,
                float reloadTime,
                float? launchAngleX,
                float? launchAngleY,
                float launchSpeed,
                float launchIntervalTime,
                float sightingMaxRange)
            {
                Id = id;
                AmmoType = ammoType;
                ProjectileAssetId = projectileAssetId;
                WeaponResourceMaxCount = weaponResourceMaxCount;
                ReloadTime = reloadTime;
                LaunchAngleX = launchAngleX;
                LaunchAngleY = launchAngleY;
                LaunchSpeed = launchSpeed;
                LaunchIntervalTime = launchIntervalTime;
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
                new Row(1, AmmoType.Missile, 2, 1, 7.0f, null, null, 1.0f, 1.0f, 500),
                new Row(2, AmmoType.Missile, 2, 4, 7.0f, null, null, 1.0f, 1.0f, 500),
            };
        }
    }
}
