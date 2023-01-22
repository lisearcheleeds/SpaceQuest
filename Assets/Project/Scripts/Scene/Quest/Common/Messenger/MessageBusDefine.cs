using System;
using UnityEngine;

namespace AloneSpace
{
    public class MessageBusDefine
    {
        public class AddWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}
        public class RemoveWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}
        
        public class SendThreat : MessageBusBroadcaster<IThreatData, bool>{}
        public class SendIntuition : MessageBusBroadcaster<ICollisionData, bool>{}
        public class SendCollision : MessageBusBroadcaster<ICollisionData, bool>{}
        
        public class UtilGetAreaData : MessageBusBroadcaster<int, Action<AreaData>>{}
        
        public class ManagerCommandSetObservePlayer : MessageBusBroadcaster<Guid>{}
        public class ManagerCommandLoadArea : MessageBusBroadcaster<int?>{}
        
        public class NoticeHitThreat : MessageBusBroadcaster<IThreatData, ICollisionData>{}
        public class NoticeHitCollision : MessageBusBroadcaster<ICollisionData, ICollisionData>{}
        public class NoticeDamage : MessageBusBroadcaster<ICauseDamageData, IDamageableData>{}
        public class NoticeBroken : MessageBusBroadcaster<IDamageableData>{}
        
        public class UserInputSwitchMap : MessageBusBroadcaster{}
        public class UserInputOpenMap : MessageBusBroadcaster{}
        public class UserInputCloseMap : MessageBusBroadcaster{}
        public class UserInputSwitchInteractList : MessageBusBroadcaster{}
        public class UserInputOpenInteractList : MessageBusBroadcaster{}
        public class UserInputCloseInteractList : MessageBusBroadcaster{}
        public class UserInputSwitchInventory : MessageBusBroadcaster{}
        public class UserInputOpenInventory : MessageBusBroadcaster{}
        public class UserInputCloseInventory : MessageBusBroadcaster{}
        
        public class UserCommandOpenItemDataMenu : MessageBusBroadcaster<ItemData, Action, string, string>{}
        public class UserCommandCloseItemDataMenu : MessageBusBroadcaster{}
        public class UserCommandUpdateInventory : MessageBusBroadcaster<Guid[]>{}
        public class UserCommandSetCameraMode : MessageBusBroadcaster<CameraController.CameraMode>{}
        public class UserCommandRotateCamera : MessageBusBroadcaster<Vector2>{}
        public class UserCommandSetCameraAngle : MessageBusBroadcaster<Quaternion>{}
        
        public class UserCommandSetCameraTrackTarget : MessageBusBroadcaster<IPositionData>{}
        public class UserCommandGetWorldToCanvasPoint : MessageBusBroadcaster<CameraController.CameraType, Vector3, RectTransform, Action<Vector3?>>{}
        
        public class PlayerCommandSetAreaId : MessageBusBroadcaster<ActorData, int?>{}
        public class PlayerCommandSetMoveTarget : MessageBusBroadcaster<ActorData, IPositionData>{}
        public class PlayerCommandSetInteractOrder : MessageBusBroadcaster<ActorData, IInteractData>{}
        public class PlayerCommandSetTacticsType : MessageBusBroadcaster<Guid, TacticsType>{}
        
        public class ManagerCommandPickItem : MessageBusBroadcaster<InventoryData, ItemInteractData>{}
        public class ManagerCommandTransferItem : MessageBusBroadcaster<InventoryData, InventoryData, ItemData>{}

        public class ExecuteTriggerWeapon : MessageBusBroadcaster<WeaponData, ITargetData, float>{}
        
        public class SetDirtyActorObjectList : MessageBusBroadcaster{}
        public class SetDirtyInteractObjectList : MessageBusBroadcaster{}
    }
}
