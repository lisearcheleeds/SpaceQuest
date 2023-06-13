using System.Linq;

namespace AloneSpace
{
    public class ActorPresetMaster
    {
        public class Row
        {
            public int Id { get; }
            public int ActorSpecId { get; }

            public Row(
                int id,
                int actorSpecId)
            {
                Id = id;
                ActorSpecId = actorSpecId;
            }
        }

        Row[] rows;
        static ActorPresetMaster instance;

        public static ActorPresetMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPresetMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ActorPresetMaster()
        {
            rows = new[]
            {
                new Row(id: 1, actorSpecId: 1),
                new Row(id: 5, actorSpecId: 5),
            };
        }
    }
}
