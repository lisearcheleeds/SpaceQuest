using System;

namespace AloneSpace
{
    public static class MessageBusDefinePlayer
    {
        public class SetAreaId : MessageBusBroadcaster<Guid, int?>{}
        public class SetMoveTarget : MessageBusBroadcaster<Guid, IPositionData>{}
        public class SetTacticsType : MessageBusBroadcaster<Guid, TacticsType>{}
    }
}