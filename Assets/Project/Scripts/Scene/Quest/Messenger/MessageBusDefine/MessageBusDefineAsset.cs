using System;

namespace AloneSpace
{
    public static class MessageBusDefineAsset
    {
        public class GetCacheAsset : MessageBusBroadcaster<CacheableGameObjectPath, Action<CacheableGameObject>>{}
        public class ReleaseCacheAssetAll : MessageBusBroadcaster{}
        public class ReleaseCacheAsset : MessageBusBroadcaster<CacheableGameObject>{}
    }
}
