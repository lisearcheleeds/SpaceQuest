using System.Linq;

namespace RoboQuest
{
    public class MapMaster
    {
        public class Row
        {
            public int Id { get; }
            public int MapSizeX { get; }
            public int MapSizeY { get; }

            public Row(int id, int mapSizeX, int mapSizeY)
            {
                Id = id;
                MapSizeX = mapSizeX;
                MapSizeY = mapSizeY;
            }
        }
        
        static MapMaster instance;
        Row[] record;

        public static MapMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MapMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return record.First(x => x.Id == id);
        }

        MapMaster()
        {
            record = new[]
            {
                new Row(1, 7, 14),
            };
        }
    }
}
