using System;

namespace AloneSpace
{
    /// <summary>
    /// 基本スペックと品質
    /// </summary>
    public class WeaponParticleBulletMakerSpecVO : IWeaponSpecVO
    {
        // ID
        public int Id => row.Id;

        // Name
        public string Name => row.Name;

        // WeaponType
        public WeaponType WeaponType => WeaponType.BulletMaker;

        // AssetPath
        public AssetPath Path => row.Path;

        // マガジンサイズ
        public int MagazineSize => 0;

        // リロード時間
        public float ReloadTime => row.ReloadTime;

        // 連射速度(f/s)
        public float FireRate => row.FireRate;

        // 精度 1.0f以上
        public float Accuracy => row.Accuracy;

        // 射角(0.0f ~ 180.0f)
        public float AngleOfFire => row.AngleOfFire;

        // 予測射撃
        public bool IsPredictiveShoot => row.IsPredictiveShoot;

        // 自動射撃
        public bool HasAutoFireMode => row.HasAutoFireMode;

        // 旋回速度
        public float TurningSpeed => row.TurningSpeed;

        public ParticleBulletWeaponEffectSpecVO ParticleBulletWeaponEffectSpecVO { get; }

        // SpecialEffect
        public SpecialEffectSpecVO[] SpecialEffectSpecVOs { get; }

        WeaponParticleBulletMakerSpecMaster.Row row;

        public WeaponParticleBulletMakerSpecVO(int id) : this(id, WeaponBulletMakerQualityType.Default, 1.0f)
        {
        }

        public WeaponParticleBulletMakerSpecVO(int id, WeaponBulletMakerQualityType qualityType, float quality)
        {
            row = WeaponParticleBulletMakerSpecMaster.Instance.Get(id);
            ParticleBulletWeaponEffectSpecVO = new ParticleBulletWeaponEffectSpecVO(row.ParticleBulletWeaponEffectSpecMasterId);
            SpecialEffectSpecVOs = Array.Empty<SpecialEffectSpecVO>();
        }
    }
}
