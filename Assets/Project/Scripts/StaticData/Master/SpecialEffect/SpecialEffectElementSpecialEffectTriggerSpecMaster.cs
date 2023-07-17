using System;
using System.Linq;

namespace AloneSpace
{
    public class SpecialEffectElementSpecialEffectTriggerSpecMaster
    {
        public class Row : ISpecialEffectElementSpecMasterRow
        {
            // ID
            public int Id { get; }

            // 最大スタック数 0でスタックしない nullで無限にスタック
            public int? MaxStackCount { get; }

            // 効果時間 nullで無限
            public float? EffectTime { get; }

            // インターバル時間
            public float IntervalTime { get; }

            // 使用回数 nullで無限
            public int? MaxExecuteCount { get; }

            // Trigger
            public SpecialEffectElementTrigger Trigger { get; }

            // ExecuteSpecialEffectId
            public int ExecuteSpecialEffectId { get; }

            public Row(int id, int? maxStackCount, float? effectTime, float intervalTime, int? maxExecuteCount, SpecialEffectElementTrigger trigger, int executeSpecialEffectId)
            {
                Id = id;
                MaxStackCount = maxStackCount;
                EffectTime = effectTime;
                IntervalTime = intervalTime;
                MaxExecuteCount = maxExecuteCount;

                Trigger = trigger;
                ExecuteSpecialEffectId = executeSpecialEffectId;
            }
        }

        Row[] rows;
        static SpecialEffectElementSpecialEffectTriggerSpecMaster instance;

        public static SpecialEffectElementSpecialEffectTriggerSpecMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpecialEffectElementSpecialEffectTriggerSpecMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        SpecialEffectElementSpecialEffectTriggerSpecMaster()
        {
            rows = Array.Empty<Row>();

            /*
            rows = new[]
            {
                new Row(1, 0, null, 5.0f, 1, SpecialEffectElementTrigger.ReloadWeapon, 2),
            };
            */
        }
    }
}
