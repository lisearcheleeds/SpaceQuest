namespace AloneSpace
{
    public interface ICollisionEventEffectReceiverModuleHolder : ICollisionEventModuleHolder
    {
        CollisionEventEffectReceiverModule CollisionEventEffectReceiverModule { get; }
    }
}