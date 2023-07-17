namespace AloneSpace
{
    // SpecialEffectType.SpecialEffectTriggerのトリガー
    // このEnumとValueを組み合わせて使う（使わないものもある）
    public enum SpecialEffectElementTrigger
    {
        StartFight, // 戦闘開始時
        EndFight, // 戦闘終了時
        ExecuteWeapon, // 兵装を使用した時
        ReloadWeapon, // 兵装をリロードした時
        InflictDamage, // ダメージを与えた時
        ReceiveDamage, // ダメージを受けた時
        BreakEnemyShield, // シールドを破壊した時
        BreakMyShield, // シールドが破壊された時
        LockOn, // ロックオンした時
        ReceiveLockOn, // ロックオンされた時
        Kill, // キルした時
        Death, // デスした時

        ReceiveSpecialEffect, // SpecialEffectを受けた時
        ReceiveSpecialEffectFromSelf, // SpecialEffectを自分から受けた時
        ReceiveSpecialEffectFromEnemy, // SpecialEffectを敵から受けた時

        RemoveSpecialEffect, // SpecialEffectが消えた時

        CountSpecialEffect, // SpecialEffectのカウンターが一定になった時

        // お金を獲得した時
        // アイテムを獲得した時
        // 兵装を設定した時
        // スキルを使用した時
    }
}
