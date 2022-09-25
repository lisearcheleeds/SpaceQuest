namespace AloneSpace
{
    public class ActorPartsExclusiveInventoryParameterVO : IActorPartsExclusiveParameterVO
    {
        public int Id => row.Id;

        public ActorPartsExclusiveType ActorPartsExclusiveType => ActorPartsExclusiveType.Inventory;
            
        // インベントリ横
        public int CapacityWidth => row.CapacityWidth;
            
        // インベントリ縦
        public int CapacityHeight => row.CapacityHeight;

        ActorPartsExclusiveInventoryParameterMaster.Row row;

        public ActorPartsExclusiveInventoryParameterVO(int id)
        {
            row = ActorPartsExclusiveInventoryParameterMaster.Instance.Get(id);
        }
    }
}