using System.Linq;

namespace RoboQuest
{
    public class ItemExclusiveTacticsMaster
    {
        public class Row
        {
            public int Id { get; }

            public Row(int id)
            {
                Id = id;
            }
        }
        
        static ItemExclusiveTacticsMaster instance;
        Row[] record;

        public static ItemExclusiveTacticsMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemExclusiveTacticsMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return record.First(x => x.Id == id);
        }

        ItemExclusiveTacticsMaster()
        {
            record = new[]
            {
                new Row(0),
            };
        }
    }
}
