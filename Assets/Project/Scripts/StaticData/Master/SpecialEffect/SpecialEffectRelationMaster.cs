using System.Linq;

namespace AloneSpace
{
    public class SpecialEffectRelationMaster
    {
        public class Row
        {
            // SpecialEffectSpecId
            public int SpecialEffectSpecId { get; }

            // SpecialEffectCategory
            public SpecialEffectElementCategory Category { get; }

            // SpecialEffectElementId
            public int SpecialEffectElementSpecId { get; }

            public Row(int specialEffectSpecId, SpecialEffectElementCategory category, int specialEffectElementSpecId)
            {
                SpecialEffectSpecId = specialEffectSpecId;
                Category = category;
                SpecialEffectElementSpecId = specialEffectElementSpecId;
            }
        }

        Row[] rows;
        static SpecialEffectRelationMaster instance;

        public static SpecialEffectRelationMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpecialEffectRelationMaster();
                }

                return instance;
            }
        }

        public Row[] GetRange(int specialEffectSpecId)
        {
            return rows.Where(x => x.SpecialEffectSpecId == specialEffectSpecId).ToArray();
        }

        SpecialEffectRelationMaster()
        {
            rows = new[]
            {
                new Row(1, SpecialEffectElementCategory.SpecialEffectTrigger, 1),
                new Row(2, SpecialEffectElementCategory.AddWeaponEffect, 1),
            };
        }
    }
}
