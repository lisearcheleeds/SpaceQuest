using UnityEngine;

namespace AloneSpace
{
    public class AreaPresetVO
    {
        public int AreaId => areaMaster.AreaId;
        public IAssetPath PlacedObjectAsset { get; }
        
        public Vector3 SpaceSize { get; }
        public Vector3 Position { get; }

        AreaMaster.Row areaMaster;
        
        public AreaPresetVO(AreaMaster.Row areaMaster)
        {
            this.areaMaster = areaMaster;
            PlacedObjectAsset = PlacedObjectAssetMaster.Instance.Get(areaMaster.PlacedObjectAssetId);
            SpaceSize = new Vector3(areaMaster.SpaceSizeX, areaMaster.SpaceSizeY, areaMaster.SpaceSizeZ);

            if (areaMaster.PositionX.HasValue && areaMaster.PositionY.HasValue && areaMaster.PositionZ.HasValue)
            {
                Position = new Vector3(areaMaster.PositionX.Value, areaMaster.PositionY.Value, areaMaster.PositionZ.Value);
            }
            else
            {
                Position = new Vector3(
                    Random.Range(-SpaceSize.x, SpaceSize.x), 
                    Random.Range(-SpaceSize.y, SpaceSize.y),
                    Random.Range(-SpaceSize.z, SpaceSize.z));
            }
        }
    }
}