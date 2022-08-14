using System.Linq;

namespace RoboQuest
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
                new Row(0, "Rifle Ammo(degraded)", Rarity.Common, 1, 1, 120, new Texture2DPathVO(0, "Image/rifle_ammo_degraded")),
                new Row(1, "Rifle Ammo", Rarity.Common, 1, 1, 120, new Texture2DPathVO(0, "Image/rifle_ammo_normal")),
                new Row(2, "Rifle Ammo(light)", Rarity.Rare, 1, 1, 120, new Texture2DPathVO(0, "Image/rifle_ammo_light")),
                new Row(3, "Rifle Ammo(impact)", Rarity.Rare, 1, 1, 120, new Texture2DPathVO(0, "Image/rifle_ammo_impact")),
                new Row(4, "Rifle Ammo(ap)", Rarity.Rare, 1, 1, 120, new Texture2DPathVO(0, "Image/rifle_ammo_armor_piercing")),
                new Row(5, "Missile Ammo(degraded)", Rarity.Common, 1, 1, 24, new Texture2DPathVO(0, "Image/missile_ammo_degraded")),
                new Row(6, "Missile Ammo", Rarity.Common, 1, 1, 24, new Texture2DPathVO(0, "Image/missile_ammo_normal")),
                new Row(7, "Missile Ammo(homing)", Rarity.Rare, 1, 1, 24, new Texture2DPathVO(0, "Image/missile_ammo_homing")),
                new Row(8, "Missile Ammo(explosive)", Rarity.Rare, 1, 1, 24, new Texture2DPathVO(0, "Image/missile_ammo_explosive")),
                new Row(9, "Chip", Rarity.Unique, 1, 1, null, new Texture2DPathVO(0, "Image/chip")),
            };
        }
    }
}
