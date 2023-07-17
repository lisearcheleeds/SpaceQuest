using System;
using System.Linq;

namespace AloneSpace
{
    public class ActorSpecSpecialEffectRelationMaster
    {
        public class Row
        {
            // ActorSpecId
            public int ActorSpecId { get; }

            // Order
            public int Order { get; }

            // SpecialEffectId
            public int SpecialEffectId { get; }

            public Row(int actorSpecId, int order, int specialEffectId)
            {
                ActorSpecId = actorSpecId;
                Order = order;
                SpecialEffectId = specialEffectId;
            }
        }

        Row[] rows;
        static ActorSpecSpecialEffectRelationMaster instance;

        public static ActorSpecSpecialEffectRelationMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorSpecSpecialEffectRelationMaster();
                }

                return instance;
            }
        }

        public Row[] GetRange(int actorSpecId)
        {
            return rows.Where(x => x.ActorSpecId == actorSpecId).ToArray();
        }

        ActorSpecSpecialEffectRelationMaster()
        {
            rows = Array.Empty<Row>();
        }
    }
}
