using AloneSpace;

namespace AloneSpace
{
    public static class ConstantAssetPath
    {
        public static CacheableGameObjectPath ActorPathVO = new CacheableGameObjectPath("Prefab/Actor/Actor");

        public static CacheableGameObjectPath ItemObjectPathVO = new CacheableGameObjectPath("Prefab/AreaObjects/ItemObject");
        public static CacheableGameObjectPath BrokenActorObjectPathVO = new CacheableGameObjectPath("Prefab/AreaObjects/BrokenActorObject");
        public static CacheableGameObjectPath InventoryObjectPathVO = new CacheableGameObjectPath("Prefab/AreaObjects/InventoryObject");
    }
}