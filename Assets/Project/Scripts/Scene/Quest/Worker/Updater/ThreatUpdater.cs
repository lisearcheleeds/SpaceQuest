using System.Collections.Generic;

namespace AloneSpace
{
    public class ThreatUpdater : IUpdater
    {
        List<IThreatData> threatList = new List<IThreatData>();
        List<ICollisionData> intuitionCollisionList = new List<ICollisionData>();
        
        public void Initialize()
        {
            MessageBus.Instance.SendThreat.AddListener(SendThreat);
            MessageBus.Instance.SendIntuition.AddListener(SendIntuition);
        }

        public void Finalize()
        {
            MessageBus.Instance.SendThreat.RemoveListener(SendThreat);
            MessageBus.Instance.SendIntuition.RemoveListener(SendIntuition);
        }

        public void OnLateUpdate()
        {
            foreach (var threat in threatList.ToArray())
            {
                foreach (var intuitionCollision in intuitionCollisionList.ToArray())
                {
                    if (!intuitionCollision.IsCollidable)
                    {
                        continue;
                    }
                    
                    if (threat.WeaponData.PlayerInstanceId == intuitionCollision.PlayerInstanceId)
                    {
                        continue;
                    }
                    
                    if (threat.HitCollidePrediction.CheckHit(intuitionCollision.CollisionShape))
                    {
                        MessageBus.Instance.NoticeHitThreat.Broadcast(threat, intuitionCollision);
                    }
                }
            }
        }

        void SendThreat(IThreatData threatData, bool isEntry)
        {
            if (isEntry)
            {
                threatList.Add(threatData);
            }
            else
            {
                threatList.Remove(threatData);
            }
        }

        void SendIntuition(ICollisionData intuitionCollisionData, bool isEntry)
        {
            if (isEntry)
            {
                intuitionCollisionList.Add(intuitionCollisionData);
            }
            else
            {
                intuitionCollisionList.Remove(intuitionCollisionData);
            }
        }
    }
}
