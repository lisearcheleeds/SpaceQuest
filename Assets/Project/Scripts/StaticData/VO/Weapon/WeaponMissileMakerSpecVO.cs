﻿using System;

namespace AloneSpace
{
    /// <summary>
    /// 基本スペックと品質
    /// </summary>
    public class WeaponMissileMakerSpecVO : IWeaponSpecVO, IExplosionGraphicEffectSpecVOHolder
    {
        // ID
        public int Id => row.Id;

        // Name
        public string Name => row.Name;

        // WeaponType
        public WeaponType WeaponType => WeaponType.MissileMaker;

        // AssetPath
        public AssetPath Path => row.Path;

        // 一度に使用するリソース
        public int MagazineSize => row.MagazineSize;

        // リロード時間
        public float ReloadTime => row.ReloadTime;

        // 連続射出時間
        public float FireRate => row.FireRate;

        // バーストサイズ
        public int BurstSize => row.BurstSize;

        // 同時発射数
        public int ShotCount => row.ShotCount;

        // 自動射撃
        public bool HasAutoFireMode => row.HasAutoFireMode;

        // 水平発射
        public bool HorizontalLaunch => row.HorizontalLaunch;

        // 発射スピード
        public float LaunchSpeed => row.LaunchSpeed;

        // ロックオン距離
        public float LockOnDistance => row.LockOnDistance;

        // ロックオン角度
        public float LockOnAngle => row.LockOnAngle;

        // 撃ち切るかどうか
        public bool ShootUp => row.ShootUp;

        public MissileWeaponEffectSpecVO MissileWeaponEffectSpecVO { get; }
        public ExplosionWeaponEffectSpecVO ExplosionWeaponEffectSpecVO { get; }
        public GraphicEffectSpecVO SmokeGraphicEffectSpecVO { get; }
        public GraphicEffectSpecVO ExplosionGraphicEffectSpecVO { get; }

        // SpecialEffect
        public SpecialEffectSpecVO[] SpecialEffectSpecVOs { get; }

        WeaponMissileMakerSpecMaster.Row row;

        public WeaponMissileMakerSpecVO(int id) : this(id, WeaponMissileMakerQualityType.Default, 1.0f)
        {
        }

        public WeaponMissileMakerSpecVO(int id, WeaponMissileMakerQualityType qualityType, float quality)
        {
            row = WeaponMissileMakerSpecMaster.Instance.Get(id);
            MissileWeaponEffectSpecVO = new MissileWeaponEffectSpecVO(row.MissileWeaponEffectSpecMasterId);
            ExplosionWeaponEffectSpecVO = new ExplosionWeaponEffectSpecVO(row.ExplosionWeaponEffectSpecMasterId);
            SmokeGraphicEffectSpecVO = new GraphicEffectSpecVO(row.SmokeGraphicEffectSpecMasterId);
            ExplosionGraphicEffectSpecVO = new GraphicEffectSpecVO(row.ExplosionGraphicEffectSpecMasterId);
            SpecialEffectSpecVOs = Array.Empty<SpecialEffectSpecVO>();
        }
    }
}
