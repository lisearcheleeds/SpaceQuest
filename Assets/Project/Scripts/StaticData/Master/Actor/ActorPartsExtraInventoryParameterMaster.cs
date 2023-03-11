using System.Linq;

namespace AloneSpace
{
    public class ActorPartsExtraInventoryParameterMaster
    {
        public class Row
        {
            // ID
            public int Id { get; }
            
            // インベント横
            public int CapacityWidth { get; }
            
            // インベント縦
            public int CapacityHeight { get; }

            public Row(
                int id,
                int capacityWidth,
                int capacityHeight)
            {
                Id = id;
                CapacityWidth = capacityWidth;
                CapacityHeight = capacityHeight;
            }
        }

        Row[] rows;
        static ActorPartsExtraInventoryParameterMaster instance;

        public static ActorPartsExtraInventoryParameterMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPartsExtraInventoryParameterMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ActorPartsExtraInventoryParameterMaster()
        {
            rows = new[]
            {
                new Row(1, 8, 5),
                new Row(2, 4, 3)
            };
        }
    }
}
