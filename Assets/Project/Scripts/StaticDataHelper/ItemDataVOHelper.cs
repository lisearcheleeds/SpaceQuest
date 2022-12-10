using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using Random = UnityEngine.Random;

namespace AloneSpace
{
    public static class ItemDataVOHelper
    {
        public static InventoryData[] GetActorDropInventoryData(ActorData actorData)
        {
            return actorData.InventoryDataList;
        }
    }
}