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
        public AssetPath Path { get; }

        // 武器リソース最大値（マガジン最大値）
        int MagazineSize { get; }

        // リロード時間
        float ReloadTime { get; }
    }
}
