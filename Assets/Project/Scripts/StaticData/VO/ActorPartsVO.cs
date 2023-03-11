namespace AloneSpace
{
    public class ActorPartsVO : IAssetPath
    {
        // ID
        public int Id => row.Id;
        public string Path => row.Path;
        public ActorPartsParameterVO ActorPartsParameterVO { get; }
        public ActorPartsExtraInventoryParameterVO ActorPartsExtraInventoryParameterVO { get; }
        public ActorPartsExtraSensorParameterVO ActorPartsExtraSensorParameterVO { get; }
        public ActorPartsExtraBoosterParameterVO ActorPartsExtraBoosterParameterVO { get; }
        public IActorPartsWeaponParameterVO ActorPartsWeaponParameterVO { get; }

        ActorPartsMaster.Row row;
        
        public ActorPartsVO(ActorPartsMaster.Row row)
        {
            this.row = row;

            ActorPartsParameterVO = new ActorPartsParameterVO(ActorPartsParameterMaster.Instance.Get(row.ParameterId));

            ActorPartsExtraInventoryParameterVO = row.InventoryParameterId.HasValue ? new ActorPartsExtraInventoryParameterVO(row.InventoryParameterId.Value) : null;
            ActorPartsExtraSensorParameterVO = row.SensorParameterId.HasValue ? new ActorPartsExtraSensorParameterVO(row.SensorParameterId.Value) : null;
            ActorPartsExtraBoosterParameterVO = row.BoosterParameterId.HasValue ? new ActorPartsExtraBoosterParameterVO(row.BoosterParameterId.Value) : null;

            if (row.WeaponParameterId.HasValue)
            {
                var master = ActorPartsWeaponParameterMaster.Instance.Get(row.WeaponParameterId.Value);
                ActorPartsWeaponParameterVO = master.WeaponType switch
                {
                    WeaponType.Rifle => new ActorPartsWeaponRifleParameterVO(master.ActorPartsWeaponId),
                    WeaponType.MissileLauncher => new ActorPartsWeaponMissileLauncherParameterVO(master.ActorPartsWeaponId),
                    _ => null,
                };
            }
        }
    }
}