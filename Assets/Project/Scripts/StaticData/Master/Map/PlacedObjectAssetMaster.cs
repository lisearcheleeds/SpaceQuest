using System.Linq;

namespace AloneSpace
{
    public class PlacedObjectAssetMaster
    {
        public class Row
        {
            public int Id { get; }
            public AssetPath Path { get; }

            public Row(int id, AssetPath path)
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
                new Row(0, new AssetPath("Prefab/AreaPlacedObjects/Default")),
            };
        }
    }
}
