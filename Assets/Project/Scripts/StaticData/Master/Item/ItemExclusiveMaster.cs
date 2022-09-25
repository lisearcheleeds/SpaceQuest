using System.Linq;

namespace AloneSpace
{
    public class ItemExclusiveMaster
    {
        public class Row
        {
            // ID
            public int ItemId { get; }

            // タイプ
            public ItemType ItemType { get; }

            // 拡張Id
            public int ItemExclusiveId { get; }

            public Row(int itemId, ItemType itemType, int itemExclusiveId)
            {
                ItemId = itemId;
                ItemType = itemType;
                ItemExclusiveId = itemExclusiveId;
            }
        }

        Row[] rows;
        static ItemExclusiveMaster instance;

        public static ItemExclusiveMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemExclusiveMaster();
                }

                return instance;
            }
        }

        public Row[] GetRange(int id)
        {
            return rows.Where(x => x.ItemId == id).ToArray();
        }

        ItemExclusiveMaster()
        {
            rows = new[]
            {
                new Row(0, ItemType.Ammo, 0),
                new Row(1, ItemType.Ammo, 1),
                new Row(2, ItemType.Ammo, 2),
                new Row(3, ItemType.Ammo, 3),
                new Row(4, ItemType.Ammo, 4),
                new Row(5, ItemType.Ammo, 5),
                new Row(6, ItemType.Ammo, 6),
                new Row(7, ItemType.Ammo, 7),
                new Row(8, ItemType.Ammo, 8),
                new Row(9, ItemType.Chip, 0),
            };
        }
    }
}