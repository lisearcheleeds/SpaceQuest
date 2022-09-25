using System.Linq;

namespace AloneSpace
{
    public class ActorPartsExclusiveParameterMaster
    {
        public class Row
        {
            // ID
            public int Id { get; }

            // パラメータタイプ
            public ActorPartsExclusiveType ActorPartsExclusiveType { get; }

            // パラメータId
            public int ActorPartsExclusiveId { get; }

            public Row(int id, ActorPartsExclusiveType actorPartsExclusiveType, int actorPartsExclusiveId)
            {
                Id = id;
                ActorPartsExclusiveType = actorPartsExclusiveType;
                ActorPartsExclusiveId = actorPartsExclusiveId;
            }
        }

        Row[] rows;
        static ActorPartsExclusiveParameterMaster instance;

        public static ActorPartsExclusiveParameterMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPartsExclusiveParameterMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ActorPartsExclusiveParameterMaster()
        {
            rows = new[]
            {
                new Row(1, ActorPartsExclusiveType.Inventory, 1),
                new Row(2, ActorPartsExclusiveType.Inventory, 2),
                new Row(3, ActorPartsExclusiveType.Sensor, 1),
                new Row(4, ActorPartsExclusiveType.Moving, 1),
            };
        }
    }
}
