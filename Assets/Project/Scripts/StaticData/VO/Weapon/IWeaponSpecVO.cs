﻿namespace AloneSpace
{
    /// <summary>
    /// 武器データ
    /// </summary>
    public interface IWeaponSpecVO
    {
        // ID
        int Id { get; }

        // Name
        string Name { get; }

        // WeaponType
        WeaponType WeaponType { get; }

        // AssetPath
        AssetPath Path { get; }

        // 武器リソース最大値（マガジン最大値）
        int MagazineSize { get; }

        // リロード時間
        float ReloadTime { get; }

        // SpecialEffect
        public SpecialEffectSpecVO[] SpecialEffectSpecVOs { get; }
    }
}
