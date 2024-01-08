namespace AloneSpace
{
    public partial class MessageBus
    {
        public AssetMessage Asset { get; } = new AssetMessage();

        public class AssetMessage
        {
            // Asset
            public MessageBusDefineAsset.GetCacheAsset GetCacheAsset { get; } = new MessageBusDefineAsset.GetCacheAsset();
            public MessageBusDefineAsset.ReleaseCacheAssetAll ReleaseCacheAssetAll { get; } = new MessageBusDefineAsset.ReleaseCacheAssetAll();
            public MessageBusDefineAsset.ReleaseCacheAsset ReleaseCacheAsset { get; } = new MessageBusDefineAsset.ReleaseCacheAsset();
        }
    }
}
