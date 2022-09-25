namespace AloneSpace
{
    // プレイヤーの目的
    // エリアの目的地の設定方針とTacticsの切り替え
    // 戦闘値, 収拾値 
    public enum PlayerStance
    {
        None, // ユーザー用 None
        Scavenger, // Scavenger
        
        Fugitive, // 逃亡者
        ScavengerKiller, // Scavenger狩り
        Collector, // モノ拾い
        Hunter, // 狩人
        Stalker, // ストーカー
        
        ConquestUser, // 特定のエリアの制圧
        FacilityUser, // 特定のエリアのInteract利用
    }
}