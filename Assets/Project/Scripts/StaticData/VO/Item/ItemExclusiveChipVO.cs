namespace AloneSpace
{
    public class ItemExclusiveChipVO : IItemExclusiveVO
    {
        ItemExclusiveChipMaster.Row itemChipRow;

        public ItemExclusiveChipVO(ItemExclusiveChipMaster.Row itemChipRow)
        {
            this.itemChipRow = itemChipRow;
        }
    }
}