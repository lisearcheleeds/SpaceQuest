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

        public AreaInteractData SpawnPoint { get; }
        public List<IInteractData> InteractData { get; } = new List<IInteractData>();

        public AssetPath PlacedObjectAsset => areaPresetVO.PlacedObjectAsset;

        AreaPresetVO areaPresetVO;

        public AreaData(AreaPresetVO areaPresetVO)
        {
            this.areaPresetVO = areaPresetVO;

            SpawnPoint = new AreaInteractData(this, null);

            InteractData.AddRange(
                Enumerable
                    .Range(0, Random.Range(3, 10))
                    .Select(i =>
                    {
                        var itemData = new ItemData(new ItemVO(i), 1);
                        var position = new Vector3(Random.Range(-50.0f, 50.0f), 10, Random.Range(-50.0f, 50.0f));
                        return new ItemInteractData(itemData, areaPresetVO.AreaId, position, Quaternion.identity);
                    })
                    .ToList());
        }

        public void AddInteractData(IInteractData interactData)
        {
            InteractData.Add(interactData);
        }

        public void RemoveInteractData(IInteractData interactData)
        {
            InteractData.Remove(interactData);
        }
    }
}
