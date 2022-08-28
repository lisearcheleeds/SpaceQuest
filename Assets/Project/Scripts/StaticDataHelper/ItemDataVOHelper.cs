using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using Random = UnityEngine.Random;

namespace RoboQuest
{
    public static class ItemDataVOHelper
    {
        public static InventoryData[] GetActorDropInventoryData(ActorData actorData)
        {
            var dropData = new InventoryData(4, 8);
            var items = Enumerable
                .Range(1, Random.Range(2, 4))
                .Select(x => new ItemVO(Random.Range(3, 10)))
                .Select(x => new ItemData(x, x.ItemTypes.Any(y => y == ItemType.Ammo) ? Random.Range(10, 50) : (int?)null))
                .ToArray();

            foreach (var item in items)
            {
                var insertableId = dropData.VariableInventoryViewData.GetInsertableId(item);
                if (insertableId.HasValue)
                {
                    dropData.VariableInventoryViewData.InsertInventoryItem(insertableId.Value, item);
                }
            }
                
            var res = new List<InventoryData>();
            res.Add(dropData);
            res.AddRange(actorData.InventoryDataList);
            return res.ToArray();
        }
    }
}