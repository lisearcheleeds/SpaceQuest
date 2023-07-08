using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;

namespace AloneSpace
{
    public partial class MessageBusDefine
    {
        public class GetFrameCacheActorRelationData : MessageBusUnicaster<Guid, ReadOnlyArray<ActorRelationData>>{}
    }
}
