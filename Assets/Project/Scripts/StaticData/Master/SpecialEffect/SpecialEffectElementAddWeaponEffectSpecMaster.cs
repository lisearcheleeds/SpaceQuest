using System.Linq;

namespace AloneSpace
{
    public class SpecialEffectElementAddWeaponEffectSpecMaster
    {
        public class Row : ISpecialEffectElementSpecMasterRow
        {
            // ID
            public int Id { get; }

            public Row(int id)
            {
                Id = id;
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
            rows = new[]
            {
                new Row(1),
            };
        }
    }
}
