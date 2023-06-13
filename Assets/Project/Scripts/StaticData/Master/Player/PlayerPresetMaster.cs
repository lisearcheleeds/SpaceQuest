using System.Linq;

namespace AloneSpace
{
    public class PlayerPresetMaster
    {
        public class Row
        {
            public int Id { get; }
            public int Index { get; }
            public int ActorPresetId { get; }

            public Row(
                int id,
                int index,
                int actorPresetId)
            {
                Id = id;
                Index = index;
                ActorPresetId = actorPresetId;
            }
        }

        Row[] rows;
        static PlayerPresetMaster instance;

        public static PlayerPresetMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerPresetMaster();
                }

                return instance;
            }
        }

        public Row[] GetRange(int id)
        {
            return rows.Where(x => x.Id == id).ToArray();
        }

        PlayerPresetMaster()
        {
            rows = new[]
            {
                new Row(id: 1, index: 0, actorPresetId: 1),
                new Row(id: 5, index: 0, actorPresetId: 5),
            };
        }
    }
}
