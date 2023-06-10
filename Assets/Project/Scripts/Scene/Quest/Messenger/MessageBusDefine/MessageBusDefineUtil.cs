using System;

namespace AloneSpace
{
    public partial class MessageBusDefine
    {
        public class UtilGetPlayerQuestData : MessageBusUnicaster<Guid, PlayerQuestData>{}
        public class UtilGetAreaData : MessageBusUnicaster<int, AreaData>{}
        public class UtilGetAreaActorData : MessageBusUnicaster<int, ActorData[]>{}
    }
}