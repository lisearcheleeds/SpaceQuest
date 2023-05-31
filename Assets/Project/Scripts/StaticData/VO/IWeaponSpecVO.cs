namespace AloneSpace
{
    /// <summary>
    /// パーツの武器データ
    /// </summary>
    public interface IWeaponSpecVO
    {
        // ID
        int Id { get; }

        // AssetPath
        public IAssetPath AssetPath { get; }

        // 武器タイプ
        WeaponType WeaponType { get; }

        // 武器リソース最大値（マガジン最大値）
        int WeaponResourceMaxCount { get; }

        // リロード時間
        float ReloadTime { get; }
    }
}