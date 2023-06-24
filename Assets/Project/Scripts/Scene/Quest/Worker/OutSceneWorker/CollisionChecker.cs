using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class CollisionChecker
    {
        List<CollisionEventModule> collisionList = new List<CollisionEventModule>();

        public void Initialize()
        {
            MessageBus.Instance.RegisterCollision.AddListener(RegisterCollision);
            MessageBus.Instance.UnRegisterCollision.AddListener(UnRegisterCollision);
        }

        public void Finalize()
        {
            MessageBus.Instance.RegisterCollision.RemoveListener(RegisterCollision);
            MessageBus.Instance.UnRegisterCollision.RemoveListener(UnRegisterCollision);
        }

        public void OnUpdate()
        {
            CheckCollision();
        }

        void CheckCollision()
        {
            return;
            // ↓はArea外の簡易判定にのみ使う
            /*
            for (var i = 0; i < collisionList.Count; i++)
            {
                if (!collisionList[i].CollisionData.IsCollidable)
                {
                    continue;
                }

                for (var t = i + 1; t < collisionList.Count; t++)
                {
                    if (!collisionList[t].CollisionData.IsCollidable)
                    {
                        continue;
                    }

                    if (collisionList[i].CollisionData.Player.PlayerInstanceId == collisionList[t].CollisionData.Player.PlayerInstanceId)
                    {
                        continue;
                    }

                    if (collisionList[i].CollisionData.CollisionShape.CheckHit(collisionList[t].CollisionData.CollisionShape))
                    {
                        collisionList[i].AddHit(collisionList[t]);
                        collisionList[t].AddHit(collisionList[i]);
                    }
                }
            }
            */
        }

        void RegisterCollision(CollisionEventModule entryCollision)
        {
            collisionList.Add(entryCollision);
        }

        void UnRegisterCollision(CollisionEventModule entryCollision)
        {
            collisionList.Remove(entryCollision);
        }
    }
}
