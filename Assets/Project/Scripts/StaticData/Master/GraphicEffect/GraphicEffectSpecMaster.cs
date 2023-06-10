using System.Linq;

namespace AloneSpace
{
    public class GraphicEffectSpecMaster
    {
        public class Row : IWeaponSpecMaster
        {
            // ID
            public int Id { get; }

            // Path
            public CacheableGameObjectPath Path { get; }

            public Row(
                int id,
                CacheableGameObjectPath path)
            {
                Id = id;
                Path = path;
            }
        }

        Row[] rows;
        static GraphicEffectSpecMaster instance;

        public static GraphicEffectSpecMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GraphicEffectSpecMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        GraphicEffectSpecMaster()
        {
            rows = new[]
            {
                new Row(1, new CacheableGameObjectPath("Prefab/GraphicEffect/MissileSmoke")),
                new Row(2, new CacheableGameObjectPath("Prefab/GraphicEffect/StandardExplosion")),
            };
        }
    }
}
