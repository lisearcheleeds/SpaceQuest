namespace RoboQuest
{
    public class AreaAssetVO
    {
        public int AreaId { get; }
        public AmbientObjectAssetMaster.Row AmbientObjectAsset { get; }
        public PlacedObjectAssetMaster.Row PlacedObjectAsset { get; }
        public float AreaSize { get; }

        public AreaAssetVO(int areaId, int ambientObjectAssetId, int placedObjectAssetId, float areaSize)
        {
            AreaId = areaId;
            AmbientObjectAsset = AmbientObjectAssetMaster.Instance.Get(ambientObjectAssetId);
            PlacedObjectAsset = PlacedObjectAssetMaster.Instance.Get(placedObjectAssetId);
            AreaSize = areaSize;
        }
    }
}
