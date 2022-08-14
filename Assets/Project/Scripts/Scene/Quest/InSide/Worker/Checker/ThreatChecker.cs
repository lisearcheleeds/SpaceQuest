using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoboQuest.Quest.InSide
{
    public class ThreatChecker
    {
        List<IThreat> threatList = new List<IThreat>();
        List<ICollision> intuitionCollisionList = new List<ICollision>();
        
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

        public void LateUpdate()
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

        void SendThreat(IThreat threat, bool isEntry)
        {
            if (isEntry)
            {
                threatList.Add(threat);
            }
            else
            {
                threatList.Remove(threat);
            }
        }

        void SendIntuition(ICollision intuitionCollision, bool isEntry)
        {
            if (isEntry)
            {
                intuitionCollisionList.Add(intuitionCollision);
            }
            else
            {
                intuitionCollisionList.Remove(intuitionCollision);
            }
        }
    }
}
