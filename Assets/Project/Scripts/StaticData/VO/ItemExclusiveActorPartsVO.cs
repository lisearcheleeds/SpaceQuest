namespace AloneSpace
{
    public class ItemExclusiveActorPartsVO : IItemExclusiveVO
    {
        ItemExclusiveActorPartsMaster.Row itemActorPartsRow;

        public ItemExclusiveActorPartsVO(ItemExclusiveActorPartsMaster.Row itemActorPartsRow)
        {
            this.itemActorPartsRow = itemActorPartsRow;
        }
    }
}