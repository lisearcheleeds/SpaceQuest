namespace AloneSpace
{
    // SpecialEffectで値を参照する場合の参照先
    // このEnumとValueを組み合わせて使う
    public enum SpecialEffectElementValueReference
    {
        SpecifiedValue, // 固定値

        MaxEnduranceValue, // 最大HP
        CurrentEnduranceValue, // 現在HP
        MissingEnduranceValue, // 減少HP

        MaxShieldValue, // 最大シールド
        CurrentShieldValue, // 現在シールド
        MissingShieldValue, // 減少シールド

        MaxElectronicProtectionValue, // 最大シールド
        CurrentElectronicProtectionValue, // 現在シールド
        MissingElectronicProtectionValue, // 減少シールド

        MaxSpeedValue, // 最大スピード

        BaseDamageValue, // ベースダメージ
        EffectedDamageValue, // 増加ダメージ
        DecayDamageValue, // 軽減後ダメージ

        ReloadTimeValue, // リロード時間
        CurrentReloadTimeValue, // 現在のリロード時間
        RemainingReloadTimeValue, // 残りリロード時間

        MaxResourceValue, // リソース数
        CurrentResourceValue, // 現在のリソース数
        MissingResourceValue, // 残りリソース数

        CurrentSpecialEffectTimeRatio, // SpecialEffectの進捗割合
        RemainingSpecialEffectTimeRatio, // SpecialEffectの残り時間割合
    }
}
