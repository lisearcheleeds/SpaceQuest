using System.Linq;

namespace AloneSpace
{
    public class SpecialEffectElementSpecialEffectTriggerSpecMaster
    {
        public class Row : ISpecialEffectElementSpecMasterRow
        {
            // ID
            public int Id { get; }

            // Trigger
            public SpecialEffectElementTrigger Trigger { get; }

            // ExecuteSpecialEffectId
            public int ExecuteSpecialEffectId { get; }

            public Row(int id, SpecialEffectElementTrigger trigger, int executeSpecialEffectId)
            {
                Id = id;
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
            rows = new[]
            {
                new Row(1, SpecialEffectElementTrigger.ReloadWeapon, 2),
            };
        }
    }
}
