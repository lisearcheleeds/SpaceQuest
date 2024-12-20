﻿using System.Linq;

namespace AloneSpace
{
    public class AmbientObjectAssetMaster
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

        static AmbientObjectAssetMaster instance;
        Row[] record;

        public static AmbientObjectAssetMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AmbientObjectAssetMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return record.First(x => x.Id == id);
        }

        AmbientObjectAssetMaster()
        {
            record = new[]
            {
                new Row(0, new AssetPath("Prefab/AreaAmbientObjects/Default")),
            };
        }
    }
}
