using System;
using UnityEngine;

namespace AloneSpace
{
    public static class MessageBusDefineUser
    {
        public class SetPlayer : MessageBusBroadcaster<PlayerData>{}
        public class SetControlActor : MessageBusBroadcaster<ActorData>{}
        public class SetObserveTarget : MessageBusBroadcaster<IPositionData>{}
        public class SetObserveArea : MessageBusBroadcaster<AreaData>{}
    }
}
