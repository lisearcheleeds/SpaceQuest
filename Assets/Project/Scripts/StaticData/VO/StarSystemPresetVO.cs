using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class StarSystemPresetVO
    {
        public int StarSystemId => starSystemMaster.Id;
        public IAssetPath AmbientObjectAsset { get; }
        
        public AreaPresetVO[] AreaPresetVOs { get; }
        
        public Vector3 SpaceSize { get; }
        public Vector3 Position { get; }
        
        StarSystemMaster.Row starSystemMaster;
        
        public StarSystemPresetVO(int id)
        {
            starSystemMaster = StarSystemMaster.Instance.Get(id);
            var areaMasters = AreaMaster.Instance.GetRange(id);

            AreaPresetVOs = areaMasters
                .Select(areaMaster => new AreaPresetVO(starSystemMaster, areaMaster))
                .ToArray();
            AmbientObjectAsset = AmbientObjectAssetMaster.Instance.Get(starSystemMaster.AmbientObjectAssetId);
            
            SpaceSize = new Vector3(starSystemMaster.SpaceSizeX, starSystemMaster.SpaceSizeY, starSystemMaster.SpaceSizeZ);
        }
    }
}