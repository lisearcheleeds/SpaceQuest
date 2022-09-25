namespace AloneSpace
{
    public interface ICollisionData : IPlayer
    {
        bool IsCollidable { get; }
        CollisionShape CollisionShape { get; }
        void OnCollision(ICollisionData otherCollisionData);
    }
}
