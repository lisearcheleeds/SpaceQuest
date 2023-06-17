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
                new Row(10001, new CacheableGameObjectPath("Prefab/GraphicEffect/BrokenActor/Krishna")),
                new Row(10002, new CacheableGameObjectPath("Prefab/GraphicEffect/BrokenActor/Arjuna")),
                new Row(10003, new CacheableGameObjectPath("Prefab/GraphicEffect/BrokenActor/Ilis")),
                new Row(10004, new CacheableGameObjectPath("Prefab/GraphicEffect/BrokenActor/Transporter")),
                new Row(10005, new CacheableGameObjectPath("Prefab/GraphicEffect/BrokenActor/WarShip")),
            };
        }
    }
}
