using System.Linq;

namespace AloneSpace
{
    public class PlayerPresetMaster
    {
        public class Row
        {
            public int Id { get; }
            public string Name { get; }
            public int Index { get; }
            public int ActorPresetId { get; }

            public Row(
                int id,
                string name,
                int index,
                int actorPresetId)
            {
                Id = id;
                Name = name;
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
                new Row(id: 1, "Player1", index: 0, actorPresetId: 1),
                new Row(id: 5, "Player2", index: 0, actorPresetId: 5),
            };
        }
    }
}
