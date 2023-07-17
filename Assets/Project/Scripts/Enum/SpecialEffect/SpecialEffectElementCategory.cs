namespace AloneSpace
{
    public enum SpecialEffectElementCategory
    {
        // 例: 移動速度を上げたい ActorSpecModifier
        // 例: 武器の性能を上げたい WeaponSpecModifier
        // 例: 定期的にHPを回復したい SpecialEffectTrigger -> ActorStateModifier
        // 例: リロード後1回目の攻撃だけ強化したい SpecialEffectTrigger -> WeaponSpecModifier
        // 例: リロード後1回目の攻撃時にミサイルを追加で飛ばしたい SpecialEffectTrigger -> WeaponEffectMaker
        // 例: 攻撃を受けた時、自動でシールドを追加したい SpecialEffectTrigger -> ActorStateModifier
        // 例: マイナスのActorSpecModifierを受けた時、マイナスのActorSpecModifierを解除したい SpecialEffectTrigger -> SpecialEffectModifier

        // 主要
        SpecialEffectTrigger, // 別のSpecialEffectの追加
        SpecialEffectModifier, // 別のSpecialEffectの変更

        // Modifier
        SpecModifier, // 性能の変更
        StateModifier, // 状態の変更
        DamageModifier, // 受けるダメージの変更

        // 生成
        AddWeaponEffect, // WeaponEffectの生成
        AddDamage, // ダメージの生成

        // その他
        // お金を獲得
        // アイテムを獲得
    }
}
