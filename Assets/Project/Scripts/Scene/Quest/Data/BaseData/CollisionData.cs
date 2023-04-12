using System;
using System.Collections.Generic;

namespace AloneSpace
{
    public class CollisionData
    {
        public IPlayer Player { get; }
        
        public bool IsCollidable { get; set; }
        public CollisionShape CollisionShape { get; }

        public bool IsCollided => collideList.Count != 0;
        
        List<ICollisionDataHolder> collideList = new List<ICollisionDataHolder>();

        public CollisionData(IPlayer player, CollisionShape collisionShape)
        {
            Player = player;
            CollisionShape = collisionShape;
        }

        public void OnModuleUpdate(float deltaTime)
        {
            collideList.Clear();
        }

        public void OnCollision(ICollisionDataHolder otherCollisionData)
        {
            collideList.Add(otherCollisionData);
        }
    }
}