namespace RoboQuest.Quest.InSide
{
    public interface ICollision : IPlayer
    {
        bool IsCollidable { get; }
        CollisionShape CollisionShape { get; }
    }
}
