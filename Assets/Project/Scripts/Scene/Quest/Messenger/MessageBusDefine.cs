using System;
using AloneSpace.InSide;
using RoboQuest;
using UnityEngine;

namespace AloneSpace
{
    public class MessageBusDefine
    {
        public class SendTarget : MessageBusBroadcaster<ITarget, bool>{}
        public class SendThreat : MessageBusBroadcaster<IThreat, bool>{}
        public class SendIntuition : MessageBusBroadcaster<ICollision, bool>{}
        public class SendCollision : MessageBusBroadcaster<ICollision, bool>{}
        public class SendInteractionObject : MessageBusBroadcaster<IInteractionObject, bool>{}
        
        public class UpdateInteractData : MessageBusBroadcaster<int, IInteractData[]>{}
        
        public class SubscribeUpdateAll : MessageBusBroadcaster{}
        public class SubscribeUpdateTargetList : MessageBusBroadcaster<ITarget[]>{}
        public class SubscribeUpdateInteractionObjectList : MessageBusBroadcaster<IInteractionObject[]>{}
        public class SubscribeUpdateActorList : MessageBusBroadcaster<Actor[]>{}
        
        public class NoticeHitThreat : MessageBusBroadcaster<IThreat, ICollision>{}
        public class NoticeHitCollision : MessageBusBroadcaster<ICollision, ICollision>{}
        public class NoticeDamage : MessageBusBroadcaster<WeaponData, ItemVO, IDamageableData>{}
        public class NoticeBroken : MessageBusBroadcaster<IDamageableData>{}
        
        public class SetObserveArea : MessageBusBroadcaster<int>{}
        
        public class UserCommandChangeCameraMode : MessageBusBroadcaster<CameraMode>{}
        public class UserCommandSetCameraFocusObject : MessageBusBroadcaster<Transform>{}
        public class UserCommandSetObservePlayer : MessageBusBroadcaster<Guid>{}
        public class UserCommandSetObserveActor : MessageBusBroadcaster<Guid>{}
        public class UserCommandOpenItemDataMenu : MessageBusBroadcaster<ItemData, Action, string, string>{}
        public class UserCommandCloseItemDataMenu : MessageBusBroadcaster{}
        public class UserCommandGlobalMapFocusCell : MessageBusBroadcaster<int, bool>{}
        public class UserCommandUpdateInventory : MessageBusBroadcaster<Guid[]>{}
        public class UserCommandRotateCamera : MessageBusBroadcaster<Vector2>{}
        public class UserCommandSetCameraAngle : MessageBusBroadcaster<Quaternion>{}
        
        public class PlayerCommandSetDestinateAreaIndex : MessageBusBroadcaster<Guid, int?>{}
        public class PlayerCommandAddInteractItemOrder : MessageBusBroadcaster<ActorData, ItemObject>{}
        public class PlayerCommandRemoveInteractItemOrder : MessageBusBroadcaster<ActorData, ItemObject>{}
        public class PlayerCommandSetTacticsType : MessageBusBroadcaster<Guid, TacticsType>{}
        
        public class ManagerCommandTransitionActor : MessageBusBroadcaster<ActorData, int, int>{}
        
        public class ManagerCommandStoreItem : MessageBusBroadcaster<int, InventoryData, ItemData>{}
        public class ManagerCommandTransferItem : MessageBusBroadcaster<InventoryData, InventoryData, ItemData>{}

        public class ExecuteTriggerWeapon : MessageBusBroadcaster<WeaponData, ItemVO, ITargetData, float>{}
    }
}
