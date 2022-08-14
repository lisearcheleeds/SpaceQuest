using System.Linq;

namespace RoboQuest
{
    public class ItemExclusiveAmmoMaster
    {
        public class Row
        {
            public int Id { get; }
            public AmmoType AmmoType { get; }

            public Row(int id, AmmoType ammoType)
            {
                Id = id;
                AmmoType = ammoType;
            }
        }
        
        static ItemExclusiveAmmoMaster instance;
        Row[] record;

        public static ItemExclusiveAmmoMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemExclusiveAmmoMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return record.First(x => x.Id == id);
        }

        ItemExclusiveAmmoMaster()
        {
            record = new[]
            {
                new Row(0, AmmoType.Rifle),
                new Row(1, AmmoType.Rifle),
                new Row(2, AmmoType.Rifle),
                new Row(3, AmmoType.Rifle),
                new Row(4, AmmoType.Rifle),
                new Row(5, AmmoType.Missile),
                new Row(6, AmmoType.Missile),
                new Row(7, AmmoType.Missile),
                new Row(8, AmmoType.Missile),
            };
        }
    }
}
