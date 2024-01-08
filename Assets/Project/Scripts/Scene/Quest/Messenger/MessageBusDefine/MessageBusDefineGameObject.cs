using System;

namespace AloneSpace
{
    public class MessageBusDefineGameObject
    {
        public class GetCacheAsset : MessageBusBroadcaster<CacheableGameObjectPath, Action<CacheableGameObject>>{}
        public class ReleaseCacheAssetAll : MessageBusBroadcaster{}
        public class ReleaseCacheAsset : MessageBusBroadcaster<CacheableGameObject>{}
    }
}
