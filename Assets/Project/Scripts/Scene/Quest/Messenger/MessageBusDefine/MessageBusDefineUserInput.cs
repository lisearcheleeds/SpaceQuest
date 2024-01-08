using System;
using UnityEngine;

namespace AloneSpace
{
    public class MessageBusDefineUserInput
    {
        public class UserInputOpenMenu : MessageBusBroadcaster{}
        public class UserInputCloseMenu : MessageBusBroadcaster{}
        public class UserInputSwitchMenuStatusView : MessageBusBroadcaster{}
        public class UserInputSwitchMenuInventoryView : MessageBusBroadcaster{}
        public class UserInputSwitchMenuPlayerView : MessageBusBroadcaster{}
        public class UserInputSwitchMenuAreaView : MessageBusBroadcaster{}
        public class UserInputSwitchMenuMapView : MessageBusBroadcaster{}

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
        public class UserCommandGetWorldToCanvasPoint : MessageBusUnicaster<CameraType, Vector3, RectTransform, Vector3?>{}
        public class UserCommandGetCameraRotation : MessageBusUnicaster<CameraType, Quaternion>{}
        public class UserCommandGetCameraFieldOfView : MessageBusUnicaster<CameraType, float>{}

        public class UserCommandSetLookAtAngle : MessageBusBroadcaster<Vector3>{}
        public class UserCommandSetLookAtSpace : MessageBusBroadcaster<Quaternion>{}
        public class UserCommandSetLookAtDistance : MessageBusBroadcaster<float>{}
    }
}
