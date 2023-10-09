using UnityEngine;

namespace AloneSpace
{
    public class AreaPresetVO
    {
        public int AreaId => areaMaster.AreaId;
        public AssetPath PlacedObjectAsset { get; }

        public Vector3 SpaceSize { get; }
        public Vector3 Position { get; }
        public Vector3 SpawnPosition { get; }

        AreaMaster.Row areaMaster;

        public AreaPresetVO(StarSystemMaster.Row starSystemMaster, AreaMaster.Row areaMaster)
        {
            this.areaMaster = areaMaster;
            PlacedObjectAsset = PlacedObjectAssetMaster.Instance.Get(areaMaster.PlacedObjectAssetId).Path;
            SpaceSize = new Vector3(areaMaster.SpaceSizeX, areaMaster.SpaceSizeY, areaMaster.SpaceSizeZ);

            if (areaMaster.PositionX.HasValue && areaMaster.PositionY.HasValue && areaMaster.PositionZ.HasValue)
            {
                Position = new Vector3(areaMaster.PositionX.Value, areaMaster.PositionY.Value, areaMaster.PositionZ.Value);
            }
            else
            {
                Position = new Vector3(
                    Random.Range(-starSystemMaster.SpaceSizeX * 0.5f, starSystemMaster.SpaceSizeX * 0.5f),
                    Random.Range(-starSystemMaster.SpaceSizeY * 0.5f, starSystemMaster.SpaceSizeY * 0.5f),
                    Random.Range(-starSystemMaster.SpaceSizeZ * 0.5f, starSystemMaster.SpaceSizeZ * 0.5f));
            }
        }
    }
}
