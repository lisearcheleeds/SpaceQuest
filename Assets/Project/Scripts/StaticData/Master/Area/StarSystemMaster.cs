using System.Linq;

namespace AloneSpace
{
    public class StarSystemMaster
    {
        public class Row
        {
            public int Id { get; }
            public int AmbientObjectAssetId { get; }
            
            public int SpaceSizeX { get; }
            public int SpaceSizeY { get; }
            public int SpaceSizeZ { get; }
            
            public int? PositionX { get; }
            public int? PositionY { get; }
            public int? PositionZ { get; }

            public Row(
                int id,
                int ambientObjectAssetId,
                int spaceSizeX,
                int spaceSizeY,
                int spaceSizeZ,
                int? positionX,
                int? positionY,
                int? positionZ)
            {
                Id = id;
                AmbientObjectAssetId = ambientObjectAssetId;
                
                SpaceSizeX = spaceSizeX;
                SpaceSizeY = spaceSizeY;
                SpaceSizeZ = spaceSizeZ;
                
                PositionX = positionX;
                PositionY = positionY;
                PositionZ = positionZ;
            }
        }
        
        static StarSystemMaster instance;
        Row[] record;

        public static StarSystemMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StarSystemMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return record.First(x => x.Id == id);
        }

        StarSystemMaster()
        {
            record = new[]
            {
                new Row(1, 0, 400, 50, 400, null, null, null),
            };
        }
    }
}
