namespace AloneSpace
{
    public interface ICollisionEventEffectSenderModuleHolder : ICollisionEventModuleHolder
    {
        CollisionEventEffectSenderModule CollisionEventEffectSenderModule { get; }
    }
}