using System.Linq;

namespace AloneSpace
{
    public class SpecialEffectSpecMaster
    {
        public class Row
        {
            // ID
            public int Id { get; }

            // Name
            public string Name { get; }

            public Row(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        Row[] rows;
        static SpecialEffectSpecMaster instance;

        public static SpecialEffectSpecMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SpecialEffectSpecMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        SpecialEffectSpecMaster()
        {
            rows = new[]
            {
                new Row(1, "SpecialEffect1"),
                new Row(2, "SpecialEffect2"),
            };
        }
    }
}
