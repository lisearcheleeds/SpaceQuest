namespace AloneSpace
{
    public interface ICollisionDataHolder : IModuleHolder
    {
        CollisionData CollisionData { get; }

        void AddHit(ICollisionDataHolder otherCollisionDataHolder);
    }
}