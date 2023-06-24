namespace AloneSpace
{
    public class WeaponBulletMakerSpecVO : IWeaponSpecVO
    {
        // ID
        public int Id => row.Id;

        // AssetPath
        public AssetPath Path => row.Path;

        // マガジンサイズ
        public int MagazineSize => row.MagazineSize;

        // リロード時間
        public float ReloadTime => row.ReloadTime;

        // 連射速度(f/s)
        public float FireRate => row.FireRate;

        // 精度 1.0f以上
        public float Accuracy => row.Accuracy;

        // 射角(0.0f ~ 180.0f)
        public float AngleOfFire => row.AngleOfFire;

        // バーストサイズ
        public int BurstSize => row.BurstSize;

        // 同時発射数
        public int ShotCount => row.ShotCount;

        // 予測射撃
        public bool IsPredictiveShoot => row.IsPredictiveShoot;

        // 自動射撃
        public bool HasAutoFireMode => row.HasAutoFireMode;

        // 旋回速度
        public float TurningSpeed => row.TurningSpeed;

        public BulletWeaponEffectSpecVO BulletWeaponEffectSpecVO { get; }

        WeaponBulletMakerSpecMaster.Row row;

        public WeaponBulletMakerSpecVO(int id) : this(id, WeaponBulletMakerQualityType.Default, 1.0f)
        {
        }

        public WeaponBulletMakerSpecVO(int id, WeaponBulletMakerQualityType qualityType, float quality)
        {
            row = WeaponBulletMakerSpecMaster.Instance.Get(id);
            BulletWeaponEffectSpecVO = new BulletWeaponEffectSpecVO(row.BulletWeaponEffectSpecMasterId);
        }
    }
}
