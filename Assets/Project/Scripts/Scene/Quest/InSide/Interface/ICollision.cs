namespace AloneSpace.InSide
{
    public interface ICollision : IPlayer
    {
        bool IsCollidable { get; }
        CollisionShape CollisionShape { get; }
    }
}
