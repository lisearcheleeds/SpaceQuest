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

            areaScale = SpaceSize.magnitude;
        }

        public Vector3 GetStarSystemPosition(IPositionData positionData)
        {
            if (!positionData.AreaId.HasValue)
            {
                return positionData.Position;
            }

            return AreaData.First(x => x.AreaId == positionData.AreaId).StarSystemPosition + positionData.Position / areaScale;
        }

        public Vector3 GetOffsetStarSystemPosition(IPositionData fromPositionData, IPositionData toPositionData)
        {
            return GetStarSystemPosition(toPositionData) - GetStarSystemPosition(fromPositionData);
        }
        
        public AreaData GetNearestAreaData(IPositionData positionData)
        {
            if (positionData.AreaId.HasValue)
            {
                return AreaData.First(x => x.AreaId == positionData.AreaId);
            }

            // positionData.AreaId.HasValue = falseの時、PositionはStarSystemPositionを指す
            return AreaData.OrderBy(x => (x.StarSystemPosition - positionData.Position).sqrMagnitude).First();
        }
    }
}
