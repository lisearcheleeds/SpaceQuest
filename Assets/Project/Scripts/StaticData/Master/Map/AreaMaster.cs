﻿using System.Linq;

namespace AloneSpace
{
    public class AreaMaster
    {
        public class Row
        {
            public int MapId { get; }
            public int AreaId { get; }
            public int PlacedObjectAssetId { get; }
            public float AreaSize { get; }

            public Row(int mapId, int areaId, int placedObjectAssetId, float areaSize)
            {
                MapId = mapId;
                AreaId = areaId;
                PlacedObjectAssetId = placedObjectAssetId;
                AreaSize = areaSize;
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

        public Row Get(int mapId, int areaId)
        {
            // FIXME
            areaId = 1;
            
            return record.First(x => x.MapId == mapId && x.AreaId == areaId);
        }

        AreaMaster()
        {
            record = new[]
            {
                new Row(1, 1, 0, 100.0f),
            };
        }
    }
}
