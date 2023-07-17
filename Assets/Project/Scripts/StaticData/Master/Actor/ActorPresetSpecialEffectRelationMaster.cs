using System;
using System.Linq;

namespace AloneSpace
{
    public class ActorPresetSpecialEffectRelationMaster
    {
        public class Row
        {
            // ActorPresetId
            public int ActorPresetId { get; }

            // Order
            public int Order { get; }

            // SpecialEffectId
            public int SpecialEffectId { get; }

            public Row(int actorPresetId, int order, int specialEffectId)
            {
                ActorPresetId = actorPresetId;
                Order = order;
                SpecialEffectId = specialEffectId;
            }
        }

        Row[] rows;
        static ActorPresetSpecialEffectRelationMaster instance;

        public static ActorPresetSpecialEffectRelationMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPresetSpecialEffectRelationMaster();
                }

                return instance;
            }
        }

        public Row[] GetRange(int actorPresetId)
        {
            return rows.Where(x => x.ActorPresetId == actorPresetId).ToArray();
        }

        ActorPresetSpecialEffectRelationMaster()
        {
            rows = Array.Empty<Row>();

            /*
            rows = new[]
            {
                new Row(1, 0, 1),
            };
            */
        }
    }
}
