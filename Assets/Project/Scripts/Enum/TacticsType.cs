namespace AloneSpace
{
    public enum TacticsType
    {        
        Combat, // 移動しながら拾得や戦闘を行う
        Defense, // 移動せずに拾得と戦闘を行う
        Survey, // 敵が居ても拾得と移動を優先する
        Escape, // 拾得せず敵が居ても移動を優先する
    }
}