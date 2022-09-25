using System.Linq;

namespace AloneSpace
{
    public class ItemExclusiveActorPartsMaster
    {
        public class Row
        {
            public int Id { get; }

            public Row(int id)
            {
                Id = id;
            }
        }
        
        static ItemExclusiveActorPartsMaster instance;
        Row[] record;

        public static ItemExclusiveActorPartsMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemExclusiveActorPartsMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return record.First(x => x.Id == id);
        }

        ItemExclusiveActorPartsMaster()
        {
            record = new[]
            {
                new Row(0),
            };
        }
    }
}
