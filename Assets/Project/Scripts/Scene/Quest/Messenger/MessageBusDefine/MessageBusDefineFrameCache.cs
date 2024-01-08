using System;
using UnityEngine.InputSystem.Utilities;

namespace AloneSpace
{
    public class MessageBusDefineFrameCache
    {
        public class GetActorRelationData : MessageBusUnicaster<Guid, ReadOnlyArray<ActorRelationData>>{}
    }
}
