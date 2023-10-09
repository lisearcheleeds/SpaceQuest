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
        public AssetPath AmbientObjectAsset => starSystemPresetVO.AmbientObjectAsset;

        public AreaData[] AreaData { get; }

        StarSystemPresetVO starSystemPresetVO;

        public StarSystemData(StarSystemPresetVO starSystemPresetVO)
        {
            this.starSystemPresetVO = starSystemPresetVO;
            AreaData = starSystemPresetVO.AreaPresetVOs
                .Select(areaPresetVO => new AreaData(areaPresetVO))
                .ToArray();
        }

        public AreaData GetAreaData(int areaId)
        {
            return AreaData.First(x => x.AreaId == areaId);
        }
    }
}
