using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class CollisionUpdater : IUpdater
    {
        List<ICollisionData> collisionList = new List<ICollisionData>();

        public void Initialize()
        {
            MessageBus.Instance.SendCollision.AddListener(OnReceiveCollision);
        }

        public void Finalize()
        {
            MessageBus.Instance.SendCollision.RemoveListener(OnReceiveCollision);
        }

        public void OnLateUpdate()
        {
            var targetList = collisionList.ToArray();
            
            for (var i = 0; i < targetList.Length; i++)
            {
                if (!targetList[i].IsCollidable)
                {
                    continue;
                }

                for (var t = i + 1; t < targetList.Length; t++)
                {
                    if (!targetList[t].IsCollidable)
                    {
                        continue;
                    }
                    
                    if (targetList[i].PlayerInstanceId == targetList[t].PlayerInstanceId)
                    {
                        continue;
                    }

                    if (targetList[i].CollisionShape.CheckHit(targetList[t].CollisionShape))
                    {
                        MessageBus.Instance.NoticeHitCollision.Broadcast(targetList[i], targetList[t]);
                    }
                }
            }
        }

        void OnReceiveCollision(ICollisionData entryColiision, bool isEntry)
        {
            if (isEntry)
            {
                collisionList.Add(entryColiision);
            }
            else
            {
                collisionList.Remove(entryColiision);
            }
        }
    }
}
