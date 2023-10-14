using System;

namespace AloneSpace
{
    public partial class MessageBusDefine
    {
        public class GetCacheAsset : MessageBusBroadcaster<CacheableGameObjectPath, Action<CacheableGameObject>>{}
        public class ReleaseCacheAssetAll : MessageBusBroadcaster{}
        public class ReleaseCacheAsset : MessageBusBroadcaster<CacheableGameObject>{}
    }
}
