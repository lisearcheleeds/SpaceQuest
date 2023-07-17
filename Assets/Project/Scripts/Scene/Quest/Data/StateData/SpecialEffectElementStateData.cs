namespace AloneSpace
{
    public class SpecialEffectElementStateData
    {
        // Stack数
        public int CurrentStackCount { get; set; }

        // インターバル時間
        public float IntervalRemainingTime { get; set; }

        // 有効時間 0になるとRemove
        public float? RemainingTime { get; set; }

        // 残り回数 0になるとRemove
        public int? RemainingExectureCount { get; set; }
    }
}
