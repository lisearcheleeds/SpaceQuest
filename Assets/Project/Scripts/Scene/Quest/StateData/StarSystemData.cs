using System;
using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    /// <summary>
    /// 星系
    /// </summary>
    public class StarSystemData
    {
        public Vector3 SpaceSize => starSystemPresetVO.SpaceSize;
        public IAssetPath AmbientObjectAsset => starSystemPresetVO.AmbientObjectAsset;

        public AreaData[] AreaData { get; }

        // SpaceSize.magnitudeは適当なスケール
        float areaScale;
        
        StarSystemPresetVO starSystemPresetVO;

        public StarSystemData(StarSystemPresetVO starSystemPresetVO)
        {
            this.starSystemPresetVO = starSystemPresetVO;            
            AreaData = starSystemPresetVO.AreaPresetVOs
                .Select(areaPresetVO => new AreaData(areaPresetVO))
                .ToArray();

            areaScale = SpaceSize.magnitude;
        }
        
        public Vector3 GetOffsetPosition(IPosition toPosition, IPosition fromPosition)
        {
            var areaOffsetPosition = AreaData.First(x => x.AreaId == toPosition.AreaId).Position - AreaData.First(x => x.AreaId == fromPosition.AreaId).Position;
            return areaOffsetPosition * areaScale + toPosition.Position - fromPosition.Position;
        }
    }
}
