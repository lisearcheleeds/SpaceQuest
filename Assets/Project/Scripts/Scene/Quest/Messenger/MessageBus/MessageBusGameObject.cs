namespace AloneSpace
{
    public partial class MessageBus
    {
        // GameObject
        public MessageBusDefine.SetDirtyActorObjectList SetDirtyActorObjectList { get; } = new MessageBusDefine.SetDirtyActorObjectList();
        public MessageBusDefine.SetDirtyInteractObjectList SetDirtyInteractObjectList { get; } = new MessageBusDefine.SetDirtyInteractObjectList();
        public MessageBusDefine.SetDirtyWeaponEffectObjectList SetDirtyWeaponEffectObjectList { get; } = new MessageBusDefine.SetDirtyWeaponEffectObjectList();

        public MessageBusDefine.GetCacheAsset GetCacheAsset { get; } = new MessageBusDefine.GetCacheAsset();
        public MessageBusDefine.ReleaseCacheAssetAll ReleaseCacheAssetAll { get; } = new MessageBusDefine.ReleaseCacheAssetAll();
        public MessageBusDefine.ReleaseCacheAsset ReleaseCacheAsset { get; } = new MessageBusDefine.ReleaseCacheAsset();
    }
}
