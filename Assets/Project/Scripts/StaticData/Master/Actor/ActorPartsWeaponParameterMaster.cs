using System.Linq;

namespace AloneSpace
{
    public class ActorPartsWeaponParameterMaster
    {
        public class Row
        {
            // ID
            public int Id { get; }

            // パラメータタイプ
            public WeaponType WeaponType { get; }

            // パラメータId
            public int ActorPartsWeaponId { get; }

            public Row(int id, WeaponType weaponType, int actorPartsWeaponId)
            {
                Id = id;
                WeaponType = weaponType;
                ActorPartsWeaponId = actorPartsWeaponId;
            }
        }

        Row[] rows;
        static ActorPartsWeaponParameterMaster instance;

        public static ActorPartsWeaponParameterMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPartsWeaponParameterMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            return rows.First(x => x.Id == id);
        }

        ActorPartsWeaponParameterMaster()
        {
            rows = new[]
            {
                new Row(1, WeaponType.Rifle, 1),
                new Row(2, WeaponType.Rifle, 2),
                new Row(3, WeaponType.MissileLauncher, 1),
                new Row(4, WeaponType.MissileLauncher, 2),
            };
        }
    }
}
