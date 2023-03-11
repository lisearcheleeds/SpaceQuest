namespace AloneSpace
{
    /// <summary>
    /// パーツの武器データ
    /// </summary>
    public interface IActorPartsWeaponParameterVO
    {
        // ID
        int Id { get; }

        // 武器タイプ
        WeaponType WeaponType { get; }

        // 武器リソース最大値（マガジン最大値）
        int WeaponResourceMaxCount { get; }
    }
}