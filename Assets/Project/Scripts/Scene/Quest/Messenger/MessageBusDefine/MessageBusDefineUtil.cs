using System;
using UnityEngine;

namespace AloneSpace
{
    public static class MessageBusDefineUtil
    {
        public class GetPlayerData : MessageBusUnicaster<Guid, PlayerData>{}
        public class GetAreaData : MessageBusUnicaster<int, AreaData>{}
        public class GetAreaActorData : MessageBusUnicaster<int, ActorData[]>{}
        
        public class GetWorldToCanvasPoint : MessageBusUnicaster<CameraType, Vector3, RectTransform, Vector3?>{}
        public class GetCameraRotation : MessageBusUnicaster<CameraType, Quaternion>{}
        public class GetCameraFieldOfView : MessageBusUnicaster<CameraType, float>{}
    }
}
