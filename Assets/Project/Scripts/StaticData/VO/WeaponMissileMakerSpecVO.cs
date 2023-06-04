namespace AloneSpace
{
    public class WeaponMissileMakerSpecVO : IWeaponSpecVO
    {
        // ID
        public int Id => row.Id;

        // AssetPath
        public AssetPath Path => row.Path;

        // 武器タイプ
        public WeaponType WeaponType => WeaponType.MissileLauncher;

        // 射出物アセット
        public CacheableGameObjectPath ProjectilePath => projectilePath.Path;

        // 一度に使用するリソース
        public int WeaponResourceMaxCount => row.WeaponResourceMaxCount;

        // リロード時間
        public float ReloadTime => row.ReloadTime;

        // 発射時の方向X
        // 設定されているとtargetの位置にかかわらずshooterの方向とBaseAngleに依存して射出する
        public float? LaunchAngleX => row.LaunchAngleX;

        // 発射時の方向Y
        // 設定されているとtargetの位置にかかわらずshooterの方向とBaseAngleに依存して射出する
        public float? LaunchAngleY => row.LaunchAngleY;

        // 射出初速
        public float LaunchSpeed => row.LaunchSpeed;

        // 速度
        public float Speed => row.Speed;

        // 連続射出時間
        public float FireRate => row.FireRate;

        // 照準最大距離
        public float SightingMaxRange => row.SightingMaxRange;

        WeaponMissileMakerSpecMaster.Row row;
        WeaponEffectObjectPathMaster.Row projectilePath;

        public WeaponMissileMakerSpecVO(int id) : this(id, WeaponMissileMakerQualityType.Default, 1.0f)
        {
        }

        public WeaponMissileMakerSpecVO(int id, WeaponMissileMakerQualityType qualityType, float quality)
        {
            row = WeaponMissileMakerSpecMaster.Instance.Get(id);
            projectilePath = WeaponEffectObjectPathMaster.Instance.Get(row.WeaponEffectAssetId);
        }
    }
}