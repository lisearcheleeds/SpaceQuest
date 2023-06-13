using System;

namespace AloneSpace
{
    public partial class MessageBusDefine
    {
        public class SetDirtyActorObjectList : MessageBusBroadcaster{}
        public class SetDirtyInteractObjectList : MessageBusBroadcaster{}
        public class SetDirtyWeaponEffectObjectList : MessageBusBroadcaster{}

        public class GetCacheAsset : MessageBusBroadcaster<CacheableGameObjectPath, Action<CacheableGameObject>>{}
        public class ReleaseCacheAssetAll : MessageBusBroadcaster{}
        public class ReleaseCacheAsset : MessageBusBroadcaster<CacheableGameObject>{}
    }
}
