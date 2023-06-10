namespace AloneSpace
{
    public class WeaponBulletMakerSpecVO : IWeaponSpecVO
    {
        // ID
        public int Id => row.Id;

        // AssetPath
        public AssetPath Path => row.Path;

        // マガジンサイズ
        public int WeaponResourceMaxCount => row.WeaponResourceMaxCount;

        // リロード時間
        public float ReloadTime => row.ReloadTime;

        // 連射速度(f/s)
        public float FireRate => row.FireRate;

        // 精度 1.0f以上
        public float Accuracy => row.Accuracy;

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