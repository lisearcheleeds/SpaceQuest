using System;
using UnityEngine;

namespace AloneSpace
{
    public static class MessageBusDefineActor
    {
        public class SetWeaponExecute : MessageBusBroadcaster<Guid, bool>{}
        public class ReloadWeapon : MessageBusBroadcaster<Guid>{}
        public class SetCurrentWeaponGroupIndex : MessageBusBroadcaster<Guid, int>{}

        public class ForwardBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class BackBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class RightBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class LeftBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class TopBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class BottomBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class PitchBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class RollBoosterPower : MessageBusBroadcaster<Guid, float>{}
        public class YawBoosterPower : MessageBusBroadcaster<Guid, float>{}

        public class SetLookAtDirection : MessageBusBroadcaster<Guid, Vector3>{}

        public class SetMainTarget : MessageBusBroadcaster<Guid, IPositionData>{}
    }
}
