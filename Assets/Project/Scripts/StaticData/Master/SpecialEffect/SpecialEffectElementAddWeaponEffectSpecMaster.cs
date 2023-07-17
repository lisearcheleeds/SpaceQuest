using System;
using System.Linq;

namespace AloneSpace
{
    public class SpecialEffectElementAddWeaponEffectSpecMaster
    {
        public class Row : ISpecialEffectElementSpecMasterRow
        {
            // ID
            public int Id { get; }

            // 最大スタック数 1でスタックしない nullで無限にスタック
            public int? MaxStackCount { get; }

            // 効果時間 nullで無限
            public float? EffectTime { get; }

            // インターバル時間
            public float IntervalTime { get; }

            // 使用回数 nullで無限
            public int? MaxExecuteCount { get; }

            public Row(int id, int? maxStackCount, float? effectTime, float intervalTime, int? maxExecuteCount)
            {
                Id = id;
                MaxStackCount = maxStackCount;
                EffectTime = effectTime;
                IntervalTime = intervalTime;
                MaxExecuteCount = maxExecuteCount;
            }
        }

        Row[] rows;
        static SpecialEffectElementAddWeaponEffectSpecMaster instance;

        public static SpecialEffectElementAddWeaponEffectSpecMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpecialEffectElementAddWeaponEffectSpecMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        SpecialEffectElementAddWeaponEffectSpecMaster()
        {
            rows = Array.Empty<Row>();

            /*
            rows = new[]
            {
                new Row(1, null, 5.0f, 0, 1),
            };
            */
        }
    }
}
