using System;

namespace AloneSpace
{
    public partial class MessageBusDefine
    {
        public class UtilGetPlayerData : MessageBusUnicaster<Guid, PlayerData>{}
        public class UtilGetAreaData : MessageBusUnicaster<int, AreaData>{}
        public class UtilGetAreaActorData : MessageBusUnicaster<int, ActorData[]>{}
    }
}
