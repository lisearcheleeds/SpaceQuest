using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace.InSide
{
    public class ActorAIHandler
    {
        public IActor Actor { get; }
        public ActorData ActorData { get; }
        public ICollision ActorCollision { get; }
        
        public ActorPathFinder ActorPathFinder { get; set; }
        
        // 現在の移動速度
        public Vector3 Velocity { get; set; }
        
        // 操作系
        // 通常移動
        public Vector3 RequestMove { get; set; }
        public float Throttle { get; set; }
        // リソースを使った移動
        public Vector3 RequestQuickMove { get; set; }
        // 通常回転
        public Vector3 RequestRotate { get; set; }
        // リソースを使った回転
        public Vector3 RequestQuickRotate { get; set; }

        public ITarget[] Targets { get; set; } = new ITarget[0];
        public ITarget MainTarget { get; set; }
        
        public List<IThreat> HitThreatList { get; set; } = new List<IThreat>();
        
        public IInteractionObject[] InteractionObjects { get; set; }

        public ActorAIHandler(IActor actor, ActorData actorData, ICollision actorCollision)
        {
            Actor = actor;
            ActorData = actorData;
            ActorCollision = actorCollision;
        }
    }
}