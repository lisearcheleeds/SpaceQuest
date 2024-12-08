namespace AloneSpace
{
    public enum ActorOperationMode
    {
        // 三人称視点（フリー視点）
        ObserverMode,
        // 戦闘機モード W/Sで加減速 A/Dでヨー マウスでロールピッチ
        FighterMode,
        // 攻撃機モード WASDで移動 マウスでピッチヨー
        AttackerMode,
        // ロックオンモード W/Sでピッチ A/Dでロール 
        LockOnMode,
    }
}
