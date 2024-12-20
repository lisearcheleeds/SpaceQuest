﻿using System;

namespace AloneSpace
{
    /// <summary>
    /// 基本スペックと品質
    /// </summary>
    public class WeaponBulletMakerSpecVO : IWeaponSpecVO
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

        // WeaponEffect
        public BulletWeaponEffectSpecVO BulletWeaponEffectSpecVO { get; }

        // SpecialEffect
        public SpecialEffectSpecVO[] SpecialEffectSpecVOs { get; }

        WeaponBulletMakerSpecMaster.Row row;

        public WeaponBulletMakerSpecVO(int id) : this(id, WeaponBulletMakerQualityType.Default, 1.0f)
        {
        }

        public WeaponBulletMakerSpecVO(int id, WeaponBulletMakerQualityType qualityType, float quality)
        {
            row = WeaponBulletMakerSpecMaster.Instance.Get(id);
            BulletWeaponEffectSpecVO = new BulletWeaponEffectSpecVO(row.BulletWeaponEffectSpecMasterId);
            SpecialEffectSpecVOs = Array.Empty<SpecialEffectSpecVO>();
        }
    }
}
