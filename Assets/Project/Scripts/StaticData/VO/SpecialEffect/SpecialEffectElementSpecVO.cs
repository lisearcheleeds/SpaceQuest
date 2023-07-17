using System;

namespace AloneSpace
{
    /// <summary>
    /// SpecialEffectSpecVO
    /// </summary>
    public class SpecialEffectElementSpecVO
    {
        public SpecialEffectElementCategory Category { get; }

        public int Id => row.Id;

        // 最大スタック数 1でスタックしない nullで無限にスタック
        public int? MaxStackCount => row.MaxStackCount;

        // 効果時間 nullで無限
        public float? EffectTime => row.EffectTime;

        // インターバル時間
        public float IntervalTime => row.IntervalTime;

        // 使用回数 nullで無限
        public int? MaxExecuteCount => row.MaxExecuteCount;

        ISpecialEffectElementSpecMasterRow row;

        public SpecialEffectElementSpecVO(SpecialEffectElementCategory category, int id)
        {
            Category = category;

            switch (category)
            {
                case SpecialEffectElementCategory.SpecialEffectTrigger:
                    row = SpecialEffectElementSpecialEffectTriggerSpecMaster.Instance.Get(id);
                    break;
                case SpecialEffectElementCategory.AddWeaponEffect:
                    row = SpecialEffectElementAddWeaponEffectSpecMaster.Instance.Get(id);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
