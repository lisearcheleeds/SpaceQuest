namespace RoboQuest
{
    public class AreaAssetVO
    {
        public int AreaId { get; }
        public IAssetPath PlacedObjectAsset { get; }
        public float AreaSize { get; }

        public AreaAssetVO(int areaId, int placedObjectAssetId, float areaSize)
        {
            AreaId = areaId;
            PlacedObjectAsset = PlacedObjectAssetMaster.Instance.Get(placedObjectAssetId);
            AreaSize = areaSize;
        }
    }
}
