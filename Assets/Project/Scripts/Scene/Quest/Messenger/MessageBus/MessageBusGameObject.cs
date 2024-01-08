namespace AloneSpace
{
    public partial class MessageBus
    {
        public GameObjectMessage GameObject { get; } = new GameObjectMessage();

        public class GameObjectMessage
        {
        }
        
        // GameObject
        public MessageBusDefineGameObject.GetCacheAsset GetCacheAsset { get; } = new MessageBusDefineGameObject.GetCacheAsset();
        public MessageBusDefineGameObject.ReleaseCacheAssetAll ReleaseCacheAssetAll { get; } = new MessageBusDefineGameObject.ReleaseCacheAssetAll();
        public MessageBusDefineGameObject.ReleaseCacheAsset ReleaseCacheAsset { get; } = new MessageBusDefineGameObject.ReleaseCacheAsset();
    }
}
