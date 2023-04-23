using System.Linq;

namespace AloneSpace
{
    public class WeaponEffectObjectPathMaster
    {
        public class Row : ICacheableGameObjectPath
        {
            public int Id { get; }
            public string Path { get; }

            public Row(int id, string path)
            {
                Id = id;
                Path = path;
            }
        }
        
        static WeaponEffectObjectPathMaster instance;
        Row[] record;

        public static WeaponEffectObjectPathMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WeaponEffectObjectPathMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return record.First(x => x.Id == id);
        }

        WeaponEffectObjectPathMaster()
        {
            record = new[]
            {
                new Row(1, "WeaponEffect/Projectile/vulcan"),
                new Row(2, "WeaponEffect/Projectile/missile"),
            };
        }
    }
}