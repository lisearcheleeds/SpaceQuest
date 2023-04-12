using AloneSpace;

namespace AloneSpace
{
    public class CollisionEffectData
    {
        public ICollisionEffectSenderModuleHolder CollisionEffectSenderModuleHolder { get; }
        public ICollisionEffectReceiverModuleHolder CollisionEffectReceiverModuleHolder { get; }

        public CollisionEffectData(ICollisionEffectSenderModuleHolder collisionEffectSenderModuleHolder, ICollisionEffectReceiverModuleHolder collisionEffectReceiverModuleHolder)
        {
            CollisionEffectSenderModuleHolder = collisionEffectSenderModuleHolder;
            CollisionEffectReceiverModuleHolder = collisionEffectReceiverModuleHolder;
        }
    }
}