using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class AreaData : IPosition
    {
        public int AreaIndex { get; }
        public Vector3 Position { get; } = Vector3.zero;
        public AreaAssetVO AreaAssetVO { get; }
        public List<IInteractData> InteractData { get; }
        public (AreaDirection AreaDirection, int Index)[] AdjacentAreaIndexes { get; }

        public AreaData(int areaIndex, AreaAssetVO areaAssetVO, (AreaDirection AreaDirection, int Index)[] adjacentAreaIndexes)
        {
            AreaIndex = areaIndex;
            AreaAssetVO = areaAssetVO;
            InteractData = new List<IInteractData>();
            
            InteractData.AddRange(
                Enumerable
                    .Range(0, Random.Range(3, 10))
                    .Select(i =>
                    {
                        var itemData = new ItemData(new ItemVO(i), 1);
                        var position = new Vector3(Random.Range(-50.0f, 50.0f), 10, Random.Range(-50.0f, 50.0f));
                        return new ItemInteractData(itemData, areaIndex, position);
                    })
                    .ToList());
            
            AdjacentAreaIndexes = adjacentAreaIndexes;
        }

        public void AddInteractData(IInteractData interactData)
        {
            InteractData.Add(interactData);
            MessageBus.Instance.UpdateInteractData.Broadcast(AreaIndex, InteractData.ToArray());
        }
        
        public void RemoveInteractData(IInteractData interactData)
        {
            InteractData.Remove(interactData);
            MessageBus.Instance.UpdateInteractData.Broadcast(AreaIndex, InteractData.ToArray());
        }
    }
}
