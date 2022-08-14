namespace RoboQuest
{
    public class ItemExclusiveAmmoVO : IItemExclusiveVO
    {
        public AmmoType AmmoType => itemAmmoRow.AmmoType;
        
        ItemExclusiveAmmoMaster.Row itemAmmoRow;

        public ItemExclusiveAmmoVO(ItemExclusiveAmmoMaster.Row itemAmmoRow)
        {
            this.itemAmmoRow = itemAmmoRow;
        }
    }
}