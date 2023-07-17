namespace AloneSpace
{
    public interface ISpecialEffectElementSpecMasterRow
    {
        // ID
        int Id { get; }

        // 最大スタック数 0でスタックしない nullで無限にスタック
        int? MaxStackCount { get; }

        // 効果時間 nullで無限
        float? EffectTime { get; }

        // インターバル時間
        float IntervalTime { get; }

        // 使用回数 nullで無限
        int? MaxExecuteCount { get; }
    }
}
