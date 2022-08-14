using System.Linq;

namespace RoboQuest
{
    public class ItemExclusiveChipMaster
    {
        public class Row
        {
            public int Id { get; }

            public Row(int id)
            {
                Id = id;
            }
        }
        
        static ItemExclusiveChipMaster instance;
        Row[] record;

        public static ItemExclusiveChipMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemExclusiveChipMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return record.First(x => x.Id == id);
        }

        ItemExclusiveChipMaster()
        {
            record = new[]
            {
                new Row(0),
            };
        }
    }
}
