namespace AloneSpace
{
    public static class ItemDataHelper
    {
        public static InventoryData[] GetActorDropInventoryData(ActorData actorData)
        {
            return actorData.InventoryDataList;
        }
    }
}