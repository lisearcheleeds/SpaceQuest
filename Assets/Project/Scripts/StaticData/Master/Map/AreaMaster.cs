using System.Linq;

namespace AloneSpace
{
    public class AreaMaster
    {
        public class Row
        {
            public int StarSystemId { get; }
            
            public int AreaId { get; }
            public int PlacedObjectAssetId { get; }
            
            public int SpaceSizeX { get; }
            public int SpaceSizeY { get; }
            public int SpaceSizeZ { get; }
            
            public int? PositionX { get; }
            public int? PositionY { get; }
            public int? PositionZ { get; }
            
            public Row(
                int starSystemId,
                int areaId,
                int placedObjectAssetId,
                int spaceSizeX,
                int spaceSizeY,
                int spaceSizeZ,
                int? positionX,
                int? positionY,
                int? positionZ)
            {
                StarSystemId = starSystemId;
                AreaId = areaId;
                PlacedObjectAssetId = placedObjectAssetId;

                SpaceSizeX = spaceSizeX;
                SpaceSizeY = spaceSizeY;
                SpaceSizeZ = spaceSizeZ;
                
                PositionX = positionX;
                PositionY = positionY;
                PositionZ = positionZ;
            }
        }
        
        static AreaMaster instance;
        Row[] record;

        public static AreaMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AreaMaster();
                }

                return instance;
            }
        }

        public Row[] GetRange(int starSystemId)
        {
            return record.Where(x => x.StarSystemId == starSystemId).ToArray();
        }

        AreaMaster()
        {
            record = new[]
            {
                new Row(1, 1, 0, 7, 2, 14, null, null, null),
                new Row(1, 2, 0, 7, 2, 14, null, null, null),
                new Row(1, 3, 0, 7, 2, 14, null, null, null),
                new Row(1, 4, 0, 7, 2, 14, null, null, null),
            };
        }
    }
}
