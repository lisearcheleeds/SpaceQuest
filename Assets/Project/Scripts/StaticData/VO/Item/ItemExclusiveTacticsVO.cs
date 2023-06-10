namespace AloneSpace
{
    public class ItemExclusiveTacticsVO : IItemExclusiveVO
    {
        ItemExclusiveTacticsMaster.Row itemTacticsRow;

        public ItemExclusiveTacticsVO(ItemExclusiveTacticsMaster.Row itemTacticsRow)
        {
            this.itemTacticsRow = itemTacticsRow;
        }
    }
}