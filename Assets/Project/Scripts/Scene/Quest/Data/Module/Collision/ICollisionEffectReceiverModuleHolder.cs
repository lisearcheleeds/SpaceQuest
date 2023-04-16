namespace AloneSpace
{
    public interface ICollisionEffectReceiverModuleHolder : ICollisionDataHolder
    {
        CollisionEffectReceiverModule CollisionEffectReceiverModule { get; }
    }
}