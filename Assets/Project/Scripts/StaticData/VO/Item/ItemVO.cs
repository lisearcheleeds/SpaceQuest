using System.Linq;
using VariableInventorySystem;

namespace AloneSpace
{
    public class ItemVO
    {
        public int Id => itemMaster.Id;

        public string Text => itemMaster.Text;
        public Rarity Rarity => itemMaster.Rarity;
        public int Width => itemMaster.Width;
        public int Height => itemMaster.Height;
        public int? MaxAmount => itemMaster.MaxAmount;
        public Texture2DPathVO ImageAsset => itemMaster.ImageAsset;

        public ItemType[] ItemTypes { get; }
        public IItemExclusiveVO[] ItemExclusiveVOs { get; }

        ItemMaster.Row itemMaster;

        public ItemVO(int itemId)
        {
            itemMaster = ItemMaster.Instance.Get(itemId);

            var exclusiveItemMasters = ItemExclusiveMaster.Instance.GetRange(itemId);
            ItemTypes = exclusiveItemMasters.Select(x => x.ItemType).ToArray();
            ItemExclusiveVOs = exclusiveItemMasters.Select(master =>
            {
                switch (master.ItemType)
                {
                    case ItemType.ActorParts:
                        return new ItemExclusiveActorPartsVO(ItemExclusiveActorPartsMaster.Instance.Get(master.ItemExclusiveId));
                    case ItemType.Chip:
                        return new ItemExclusiveChipVO(ItemExclusiveChipMaster.Instance.Get(master.ItemExclusiveId));
                    case ItemType.Tactics:
                        return  new ItemExclusiveTacticsVO(ItemExclusiveTacticsMaster.Instance.Get(master.ItemExclusiveId));
                    default:
                        return (IItemExclusiveVO) null;
                }
            }).Where(vo => vo != null).ToArray();
        }
    }
}
