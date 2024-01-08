using System;

namespace AloneSpace
{
    public class MessageBusDefineUtil
    {
        public class GetPlayerData : MessageBusUnicaster<Guid, PlayerData>{}
        public class GetAreaData : MessageBusUnicaster<int, AreaData>{}
        public class GetAreaActorData : MessageBusUnicaster<int, ActorData[]>{}
    }
}
