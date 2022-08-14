using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoboQuest.Quest
{
    public class AreaData
    {
        public int Index { get; }
        public AreaAssetVO AreaAssetVO { get; }
        public List<IInteractData> InteractData { get; }

        public AreaData(int index, AreaAssetVO areaAssetVO, (AreaDirection AreaDirection, int Index)[] adjacentIndexes)
        {
            Index = index;
            AreaAssetVO = areaAssetVO;

            InteractData = new List<IInteractData>();
            
            InteractData.AddRange(
                Enumerable
                    .Range(0, Random.Range(3, 10))
                    .Select(i =>
                    {
                        var itemData = new ItemData(new ItemVO(i), 1);
                        var position = new Vector3(Random.Range(-50.0f, 50.0f), 10, Random.Range(-50.0f, 50.0f));
                        return new ItemInteractData(itemData, index, position);
                    })
                    .ToList());

            InteractData.AddRange(
                adjacentIndexes
                    .Select(adjacentIndex =>
                    {
                        return new AreaTransitionInteractData(
                            index,
                            adjacentIndex.AreaDirection,
                            adjacentIndex.Index,
                            areaAssetVO.AreaSize);
                    })
                    .ToArray());
        }

        public void AddInteractData(IInteractData interactData)
        {
            InteractData.Add(interactData);
            MessageBus.Instance.UpdateInteractData.Broadcast(Index, InteractData.ToArray());
        }
        
        public void RemoveInteractData(IInteractData interactData)
        {
            InteractData.Remove(interactData);
            MessageBus.Instance.UpdateInteractData.Broadcast(Index, InteractData.ToArray());
        }
    }
}
