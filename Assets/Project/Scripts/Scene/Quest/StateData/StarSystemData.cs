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
        public float AreaScale;
        
        StarSystemPresetVO starSystemPresetVO;

        public StarSystemData(StarSystemPresetVO starSystemPresetVO)
        {
            this.starSystemPresetVO = starSystemPresetVO;            
            AreaData = starSystemPresetVO.AreaPresetVOs
                .Select(areaPresetVO => new AreaData(areaPresetVO))
                .ToArray();

            for (var i = 0; i < AreaData.Length; i++)
            {
                for (var t = 0; t < AreaData.Length; t++)
                {
                    if (i == t)
                    {
                        continue;
                    }

                    AreaData[i].AddInteractData(new AreaInteractData(AreaData[t], AreaData[i]));
                }
            }

            AreaScale = SpaceSize.magnitude;
        }
        
        public AreaData GetAreaData(int areaId)
        {
            return AreaData.First(x => x.AreaId == areaId);
        }
    }
}
