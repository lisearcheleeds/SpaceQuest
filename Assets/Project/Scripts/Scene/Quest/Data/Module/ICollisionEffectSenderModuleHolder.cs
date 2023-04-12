namespace AloneSpace
{
    public interface ICollisionEffectSenderModuleHolder : ICollisionDataHolder
    {
        CollisionEffectSenderModule CollisionEffectSenderModule { get; }
    }
}