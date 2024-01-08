using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class UserController
    {
        UserData userData;

        public void Initialize(UserData userData)
        {
            this.userData = userData;
        }

        public void Finalize()
        {
        }

        public void OnUpdate()
        {
            if (userData.ControlActorData == null)
            {
                return;
            }

            // 向いてる方向に一番近いターゲットをメインに
            var aroundActorRelationDataList = MessageBus.Instance.FrameCache.GetActorRelationData.Unicast(userData.ControlActorData.InstanceId);
            if (aroundActorRelationDataList.Count != 0)
            {
                var nextMainTarget = aroundActorRelationDataList
                    .OrderByDescending(aroundActorRelationData => Vector3.Dot(userData.ControlActorData.ActorStateData.LookAtDirection, aroundActorRelationData.RelativePosition.normalized))
                    .First();

                if (userData.ControlActorData.ActorStateData.MainTarget?.InstanceId != nextMainTarget.OtherActorData.InstanceId)
                {
                    MessageBus.Instance.Actor.SetMainTarget.Broadcast(userData.ControlActorData.InstanceId, nextMainTarget.OtherActorData);
                }
            }
        }
    }
}
