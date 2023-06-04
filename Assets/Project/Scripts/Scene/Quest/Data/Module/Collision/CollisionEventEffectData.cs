using AloneSpace;

namespace AloneSpace
{
    public class CollisionEventEffectData
    {
        public CollisionEventEffectSenderModule SenderModule { get; }
        public CollisionEventEffectReceiverModule ReceiverModule { get; }

        public CollisionEventEffectData(CollisionEventEffectSenderModule senderModule, CollisionEventEffectReceiverModule receiverModule)
        {
            SenderModule = senderModule;
            ReceiverModule = receiverModule;
        }
    }
}