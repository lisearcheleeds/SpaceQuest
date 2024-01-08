namespace AloneSpace
{
    public partial class MessageBus
    {
        public FrameCacheMessage FrameCache { get; } = new FrameCacheMessage();

        public class FrameCacheMessage
        {
            public MessageBusDefineFrameCache.GetActorRelationData GetActorRelationData { get; } = new MessageBusDefineFrameCache.GetActorRelationData();
        }
    }
}
