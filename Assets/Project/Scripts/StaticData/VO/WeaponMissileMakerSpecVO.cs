﻿namespace AloneSpace
{
    public class WeaponMissileMakerSpecVO : IWeaponSpecVO
    {
        // ID
        public int Id => row.Id;

        // AssetPath
        public AssetPath Path => row.Path;

        // 一度に使用するリソース
        public int WeaponResourceMaxCount => row.WeaponResourceMaxCount;

        // リロード時間
        public float ReloadTime => row.ReloadTime;

        // 連続射出時間
        public float FireRate => row.FireRate;

        public MissileWeaponEffectSpecVO MissileWeaponEffectSpecVO { get; }

        WeaponMissileMakerSpecMaster.Row row;

        public WeaponMissileMakerSpecVO(int id) : this(id, WeaponMissileMakerQualityType.Default, 1.0f)
        {
        }

        public WeaponMissileMakerSpecVO(int id, WeaponMissileMakerQualityType qualityType, float quality)
        {
            row = WeaponMissileMakerSpecMaster.Instance.Get(id);
            MissileWeaponEffectSpecVO = new MissileWeaponEffectSpecVO(row.MissileWeaponEffectSpecMasterId);
        }
    }
}