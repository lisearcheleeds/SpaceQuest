namespace AloneSpace
{
    public partial class MessageBus
    {
        // GameObject
        public MessageBusDefine.GetCacheAsset GetCacheAsset { get; } = new MessageBusDefine.GetCacheAsset();
        public MessageBusDefine.ReleaseCacheAssetAll ReleaseCacheAssetAll { get; } = new MessageBusDefine.ReleaseCacheAssetAll();
        public MessageBusDefine.ReleaseCacheAsset ReleaseCacheAsset { get; } = new MessageBusDefine.ReleaseCacheAsset();
    }
}
