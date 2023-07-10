using System.Linq;

namespace AloneSpace
{
    public class ActorPresetWeaponMaster
    {
        public class Row
        {
            public int ActorPresetId { get; }
            public int WeaponIndex { get; }

            public WeaponType WeaponType { get; }
            public int WeaponSpecId { get; }

            public Row(
                int actorPresetId,
                int weaponIndex,
                WeaponType weaponType,
                int weaponSpecId)
            {
                ActorPresetId = actorPresetId;
                WeaponIndex = weaponIndex;
                WeaponType = weaponType;
                WeaponSpecId = weaponSpecId;
            }
        }

        Row[] rows;
        static ActorPresetWeaponMaster instance;

        public static ActorPresetWeaponMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorPresetWeaponMaster();
                }

                return instance;
            }
        }

        public Row[] GetRange(int actorPresetId)
        {
            return rows.Where(x => x.ActorPresetId == actorPresetId).ToArray();
        }

        ActorPresetWeaponMaster()
        {
            rows = new[]
            {
                // new Row(actorPresetId: 1, weaponIndex: 0, weaponType: WeaponType.BulletMaker, weaponSpecId: 1),
                // new Row(actorPresetId: 1, weaponIndex: 1, weaponType: WeaponType.BulletMaker, weaponSpecId: 1),
                new Row(actorPresetId: 1, weaponIndex: 0, weaponType: WeaponType.ParticleBulletMaker, weaponSpecId: 1),
                new Row(actorPresetId: 1, weaponIndex: 1, weaponType: WeaponType.ParticleBulletMaker, weaponSpecId: 1),
                new Row(actorPresetId: 1, weaponIndex: 2, weaponType: WeaponType.MissileMaker, weaponSpecId: 1),
                new Row(actorPresetId: 1, weaponIndex: 3, weaponType: WeaponType.MissileMaker, weaponSpecId: 1),

                new Row(actorPresetId: 5, weaponIndex: 0, weaponType: WeaponType.ParticleBulletMaker, weaponSpecId: 1),
                new Row(actorPresetId: 5, weaponIndex: 1, weaponType: WeaponType.ParticleBulletMaker, weaponSpecId: 1),
                new Row(actorPresetId: 5, weaponIndex: 2, weaponType: WeaponType.ParticleBulletMaker, weaponSpecId: 1),
                new Row(actorPresetId: 5, weaponIndex: 3, weaponType: WeaponType.ParticleBulletMaker, weaponSpecId: 1),
                new Row(actorPresetId: 5, weaponIndex: 4, weaponType: WeaponType.ParticleBulletMaker, weaponSpecId: 1),
                new Row(actorPresetId: 5, weaponIndex: 5, weaponType: WeaponType.ParticleBulletMaker, weaponSpecId: 1),
                new Row(actorPresetId: 5, weaponIndex: 6, weaponType: WeaponType.MissileMaker, weaponSpecId: 2),
                new Row(actorPresetId: 5, weaponIndex: 7, weaponType: WeaponType.MissileMaker, weaponSpecId: 2),
                new Row(actorPresetId: 5, weaponIndex: 8, weaponType: WeaponType.MissileMaker, weaponSpecId: 2),
                new Row(actorPresetId: 5, weaponIndex: 9, weaponType: WeaponType.MissileMaker, weaponSpecId: 2),
            };
        }
    }
}
