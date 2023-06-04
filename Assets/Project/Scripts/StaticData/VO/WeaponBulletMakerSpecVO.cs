using AloneSpace;

namespace AloneSpace
{
    public class WeaponBulletMakerSpecVO : IWeaponSpecVO
    {
        // ID
        public int Id => row.Id;

        // AssetPath
        public AssetPath Path => row.Path;

        // 武器タイプ
        public WeaponType WeaponType => WeaponType.Rifle;

        // 射出物アセット
        public CacheableGameObjectPath ProjectilePath => projectilePath.Path;

        // マガジンサイズ
        public int WeaponResourceMaxCount => row.WeaponResourceMaxCount;

        // リロード時間
        public float ReloadTime => row.ReloadTime;

        // 連射速度(f/s)
        public float FireRate => row.FireRate;

        // 精度 1.0f以上
        public float Accuracy => row.Accuracy;

        // 速度
        public float Speed => row.Speed;

        WeaponBulletMakerSpecMaster.Row row;
        WeaponEffectObjectPathMaster.Row projectilePath;

        public WeaponBulletMakerSpecVO(int id) : this(id, WeaponBulletMakerQualityType.Default, 1.0f)
        {
        }

        public WeaponBulletMakerSpecVO(int id, WeaponBulletMakerQualityType qualityType, float quality)
        {
            row = WeaponBulletMakerSpecMaster.Instance.Get(id);
            projectilePath = WeaponEffectObjectPathMaster.Instance.Get(row.WeaponEffectAssetId);
        }
    }
}