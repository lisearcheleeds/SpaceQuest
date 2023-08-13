using System.Linq;

namespace AloneSpace
{
    public class ActorWeaponSlotLayoutMaster
    {
        public class Row
        {
            public int ActorSpecId { get; }
            public int WeaponSlotIndex { get; }
            public float PositionX { get; }
            public float PositionY { get; }

            public Row(
                int actorSpecId,
                int weaponSlotIndex,
                float positionX,
                float positionY)
            {
                ActorSpecId = actorSpecId;
                WeaponSlotIndex = weaponSlotIndex;
                PositionX = positionX;
                PositionY = positionY;
            }
        }

        Row[] rows;
        static ActorWeaponSlotLayoutMaster instance;

        public static ActorWeaponSlotLayoutMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ActorWeaponSlotLayoutMaster();
                }

                return instance;
            }
        }

        public Row[] GetRange(int actorSpecId)
        {
            return rows.Where(x => x.ActorSpecId == actorSpecId).ToArray();
        }

        ActorWeaponSlotLayoutMaster()
        {
            rows = new[]
            {
                new Row(actorSpecId: 1, weaponSlotIndex: 0, positionX: 0.5f, positionY: 0.5f),
                new Row(actorSpecId: 1, weaponSlotIndex: 1, positionX: 0.5f, positionY: 0.5f),
                new Row(actorSpecId: 1, weaponSlotIndex: 2, positionX: -0.5f, positionY: 0.5f),
                new Row(actorSpecId: 1, weaponSlotIndex: 3, positionX: -0.5f, positionY: 0.5f),
                new Row(actorSpecId: 1, weaponSlotIndex: 4, positionX: 0, positionY: 0.15f),
                new Row(actorSpecId: 1, weaponSlotIndex: 5, positionX: 0, positionY: 0.15f),

                new Row(actorSpecId: 2, weaponSlotIndex: 0, positionX: 0, positionY: 0),
                new Row(actorSpecId: 2, weaponSlotIndex: 1, positionX: 0, positionY: 0),
                new Row(actorSpecId: 2, weaponSlotIndex: 2, positionX: 0, positionY: 0),
                new Row(actorSpecId: 2, weaponSlotIndex: 3, positionX: 0, positionY: 0),
                new Row(actorSpecId: 2, weaponSlotIndex: 4, positionX: 0, positionY: 0),

                new Row(actorSpecId: 3, weaponSlotIndex: 0, positionX: 0, positionY: 0),

                new Row(actorSpecId: 5, weaponSlotIndex: 0, positionX: 0, positionY: 0),
                new Row(actorSpecId: 5, weaponSlotIndex: 1, positionX: 0, positionY: 0),
                new Row(actorSpecId: 5, weaponSlotIndex: 2, positionX: 0, positionY: 0),
                new Row(actorSpecId: 5, weaponSlotIndex: 3, positionX: 0, positionY: 0),
                new Row(actorSpecId: 5, weaponSlotIndex: 4, positionX: 0, positionY: 0),
                new Row(actorSpecId: 5, weaponSlotIndex: 5, positionX: 0, positionY: 0),
                new Row(actorSpecId: 5, weaponSlotIndex: 6, positionX: 0, positionY: 0),
                new Row(actorSpecId: 5, weaponSlotIndex: 7, positionX: 0, positionY: 0),
                new Row(actorSpecId: 5, weaponSlotIndex: 8, positionX: 0, positionY: 0),
                new Row(actorSpecId: 5, weaponSlotIndex: 9, positionX: 0, positionY: 0),
            };
        }
    }
}
