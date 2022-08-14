using System.Linq;

namespace RoboQuest
{
    public class PlacedObjectAssetMaster
    {
        public class Row : IAssetPath
        {
            public int Id { get; }
            public string Path { get; }

            public Row(int id, string path)
            {
                Id = id;
                Path = path;
            }
        }
        
        static PlacedObjectAssetMaster instance;
        Row[] record;

        public static PlacedObjectAssetMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlacedObjectAssetMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return record.First(x => x.Id == id);
        }

        PlacedObjectAssetMaster()
        {
            record = new[]
            {
                new Row(0, "AreaAmbientObjects/Default"),
            };
        }
    }
}
