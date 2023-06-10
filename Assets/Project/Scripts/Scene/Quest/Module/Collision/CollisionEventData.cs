namespace AloneSpace
{
    public class CollisionEventData
    {
        public CollisionEventModule CollisionEventModule1 { get; }
        public CollisionEventModule CollisionEventModule2 { get; }

        public CollisionEventData(CollisionEventModule collisionEventModule1, CollisionEventModule collisionEventModule2)
        {
            CollisionEventModule1 = collisionEventModule1;
            CollisionEventModule2 = collisionEventModule2;
        }
    }
}