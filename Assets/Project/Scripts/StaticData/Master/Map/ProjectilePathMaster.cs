using System.Linq;
using AloneSpace;

namespace RoboQuest
{
    public class ProjectilePathMaster
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
        
        static ProjectilePathMaster instance;
        Row[] record;

        public static ProjectilePathMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProjectilePathMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return record.First(x => x.Id == id);
        }

        ProjectilePathMaster()
        {
            record = new[]
            {
                new Row(1, "Projectile/vulcan"),
                new Row(2, "Projectile/missile"),
            };
        }
    }
}