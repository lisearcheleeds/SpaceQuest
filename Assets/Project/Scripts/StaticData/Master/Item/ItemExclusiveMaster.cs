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
                new Row(0, ItemType.Chip, 0),
            };
        }
    }
}