using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    /// <summary>
    /// 宙域
    /// </summary>
    public class AreaData
    {
        public int AreaId => areaPresetVO.AreaId;
        public Vector3 SpaceSize => areaPresetVO.SpaceSize;
        public Vector3 StarSystemPosition => areaPresetVO.Position;
        public Vector3 SpawnPosition => areaPresetVO.SpawnPosition;

        public AssetPath PlacedObjectAsset => areaPresetVO.PlacedObjectAsset;

        AreaPresetVO areaPresetVO;

        public AreaData(AreaPresetVO areaPresetVO)
        {
            this.areaPresetVO = areaPresetVO;
        }
    }
}
