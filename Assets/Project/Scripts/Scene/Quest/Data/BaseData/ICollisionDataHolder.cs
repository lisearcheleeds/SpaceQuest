namespace AloneSpace
{
    public interface ICollisionDataHolder
    {
        CollisionData CollisionData { get; }

        void AddHit(ICollisionDataHolder otherCollisionDataHolder);
    }
}