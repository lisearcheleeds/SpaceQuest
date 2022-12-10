using System.Linq;

namespace AloneSpace
{
    public class ItemMaster
    {
        public class Row
        {
            public int Id { get; }
            public string Text { get; }
            public Rarity Rarity { get; }
            public int Width { get; }
            public int Height { get; }
            public int? MaxAmount { get; }
            public Texture2DPathVO ImageAsset { get; }

            public Row(int id, string text, Rarity rarity, int width, int height, int? maxAmount, Texture2DPathVO imageAsset)
            {
                Id = id;
                Text = text;
                Rarity = rarity;
                Width = width;
                Height = height;
                MaxAmount = maxAmount;
                ImageAsset = imageAsset;
            }
        }
        
        static ItemMaster instance;
        Row[] record;

        public static ItemMaster Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemMaster();
                }

                return instance;
            }
        }

        public Row Get(int id)
        {
            // FIXME
            id %= 10;
            return record.First(x => x.Id == id);
        }

        ItemMaster()
        {
            record = new[]
            {
                new Row(0, "Chip1", Rarity.Common, 1, 1, null, new Texture2DPathVO(0, "Image/chip")),
                new Row(1, "Chip2", Rarity.Rare, 1, 1, null, new Texture2DPathVO(0, "Image/chip")),
                new Row(2, "Chip3", Rarity.Common, 1, 1, null, new Texture2DPathVO(0, "Image/chip")),
                new Row(3, "Chip4", Rarity.Common, 1, 1, null, new Texture2DPathVO(0, "Image/chip")),
                new Row(4, "Chip5", Rarity.Common, 1, 1, null, new Texture2DPathVO(0, "Image/chip")),
                new Row(5, "Chip6", Rarity.Common, 1, 1, null, new Texture2DPathVO(0, "Image/chip")),
                new Row(6, "Chip7", Rarity.Rare, 1, 1, null, new Texture2DPathVO(0, "Image/chip")),
                new Row(7, "Chip8", Rarity.Legend, 1, 1, null, new Texture2DPathVO(0, "Image/chip")),
                new Row(8, "Chip9", Rarity.SuperRare, 1, 1, null, new Texture2DPathVO(0, "Image/chip")),
                new Row(9, "Chip10", Rarity.Unique, 1, 1, null, new Texture2DPathVO(0, "Image/chip")),
            };
        }
    }
}
