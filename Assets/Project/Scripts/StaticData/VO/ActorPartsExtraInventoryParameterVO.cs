namespace AloneSpace
{
    public class ActorPartsExtraInventoryParameterVO
    {
        public int Id => row.Id;

        // インベントリ横
        public int CapacityWidth => row.CapacityWidth;
            
        // インベントリ縦
        public int CapacityHeight => row.CapacityHeight;

        ActorPartsExtraInventoryParameterMaster.Row row;

        public ActorPartsExtraInventoryParameterVO(int id)
        {
            row = ActorPartsExtraInventoryParameterMaster.Instance.Get(id);
        }
    }
}