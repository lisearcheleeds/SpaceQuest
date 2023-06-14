using System;
using UnityEngine;

namespace AloneSpace
{
    public partial class MessageBusDefine
    {
        public class ActorCommandSetWeaponExecute : MessageBusBroadcaster<Guid, bool>{}
        public class ActorCommandReloadWeapon : MessageBusBroadcaster<Guid>{}
        public class ActorCommandSetCurrentWeaponGroupIndex : MessageBusBroadcaster<Guid, int>{}

        public class ActorCommandForwardBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class ActorCommandBackBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class ActorCommandRightBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class ActorCommandLeftBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class ActorCommandTopBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class ActorCommandBottomBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class ActorCommandPitchBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class ActorCommandRollBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class ActorCommandYawBoosterPower : MessageBusBroadcaster<Guid, float>{}

        public class ActorCommandSetLookAtDirection : MessageBusBroadcaster<Guid, Vector3>{}

        public class ActorCommandSetMainTarget : MessageBusBroadcaster<Guid, IPositionData>{}
        public class ActorCommandSetAroundTargets : MessageBusBroadcaster<Guid, IPositionData[]>{}
    }
}
