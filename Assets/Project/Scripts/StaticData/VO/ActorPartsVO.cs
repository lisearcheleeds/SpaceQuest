namespace AloneSpace
{
    public class ActorPartsVO : IAssetPath
    {
        // ID
        public int Id => row.Id;

        // 基本パラメータID
        public int ParameterId => row.ParameterId;

        public string Path => row.Path;
        
        // 拡張パラメータID
        public int[] ExclusiveParameterIds => row.ExclusiveParameterIds;
        
        // 武器パラメータID
        public int[] WeaponParameterIds => row.WeaponParameterIds;

        ActorPartsMaster.Row row;
        
        public ActorPartsVO(ActorPartsMaster.Row row)
        {
            this.row = row;
        }
    }
}