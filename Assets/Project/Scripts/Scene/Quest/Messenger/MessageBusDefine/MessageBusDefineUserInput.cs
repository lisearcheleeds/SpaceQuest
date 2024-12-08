using System;
using UnityEngine;

namespace AloneSpace
{
    public static class MessageBusDefineUserInput
    {
        public class UserInputOpenMenu : MessageBusBroadcaster{}
        public class UserInputCloseMenu : MessageBusBroadcaster{}
        public class UserInputSwitchMenuStatusView : MessageBusBroadcaster{}
        public class UserInputSwitchMenuInventoryView : MessageBusBroadcaster{}
        public class UserInputSwitchMenuPlayerView : MessageBusBroadcaster{}
        
        public class UserInputOpenSpaceMapView : MessageBusBroadcaster{}
        public class UserInputCloseSpaceMapView : MessageBusBroadcaster{}

        public class UIMenuStatusViewSelectActorData : MessageBusBroadcaster<ActorData>{}

        public class UserInputExecuteWeapon : MessageBusBroadcaster<bool>{}
        public class UserInputReloadWeapon : MessageBusBroadcaster{}
        public class UserInputSetCurrentWeaponGroupIndex : MessageBusBroadcaster<int>{}

        public class UserInputFrontBoosterPower : MessageBusBroadcaster<float>{}
        public class UserInputBackBoosterPower : MessageBusBroadcaster<float>{}
        public class UserInputRightBoosterPower : MessageBusBroadcaster<float>{}
        public class UserInputLeftBoosterPower : MessageBusBroadcaster<float>{}
        public class UserInputTopBoosterPower : MessageBusBroadcaster<float>{}
        public class UserInputBottomBoosterPower : MessageBusBroadcaster<float>{}
        public class UserInputPitchBoosterPower : MessageBusBroadcaster<float>{}
        public class UserInputRollBoosterPower : MessageBusBroadcaster<float>{}
        public class UserInputYawBoosterPower : MessageBusBroadcaster<float>{}

        public class UserInputLookAt : MessageBusBroadcaster<Vector3>{}
        public class UserInputRotateToLookAtDirection : MessageBusBroadcaster<bool>{}

        public class UserCommandSetActorOperationMode : MessageBusBroadcaster<ActorOperationMode> {}
        
        public class UserInputOpenContentQuickView : MessageBusBroadcaster<IContentQuickViewData, Func<bool>, bool>{}
        public class UserInputCloseContentQuickView : MessageBusBroadcaster{}

        public class UserCommandSetCameraTrackTarget : MessageBusBroadcaster<IPositionData>{}

        public class UserCommandSetCameraGroupType : MessageBusBroadcaster<CameraGroupType>{}
        
        public class UserCommandSetLookAtAngle : MessageBusBroadcaster<Vector3>{}
        public class UserCommandSetLookAtSpace : MessageBusBroadcaster<Quaternion>{}
        public class UserCommandSetLookAtDistance : MessageBusBroadcaster<float>{}
        public class UserCommandSetSpaceMapLookAtAngle : MessageBusBroadcaster<Vector3>{}
        public class UserCommandSetSpaceMapLookAtDistance : MessageBusBroadcaster<float>{}
    }
}
