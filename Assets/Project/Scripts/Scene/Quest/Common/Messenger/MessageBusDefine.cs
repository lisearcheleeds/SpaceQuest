using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class MessageBusDefine
    {
        public class AddPlayerQuestData : MessageBusBroadcaster<PlayerQuestData>{}
        public class AddActorData : MessageBusBroadcaster<ActorData>{}
        public class RemoveActorData : MessageBusBroadcaster<ActorData>{}
        public class AddWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}
        public class RemoveWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}
        
        public class SendThreat : MessageBusBroadcaster<IThreatData, bool>{}
        public class SendIntuition : MessageBusBroadcaster<ICollisionData, bool>{}
        public class SendCollision : MessageBusBroadcaster<ICollisionData, bool>{}
        
        public class UpdateInteractData : MessageBusBroadcaster<int, IInteractData[]>{}
        
        public class NoticeHitThreat : MessageBusBroadcaster<IThreatData, ICollisionData>{}
        public class NoticeHitCollision : MessageBusBroadcaster<ICollisionData, ICollisionData>{}
        public class NoticeDamage : MessageBusBroadcaster<ICauseDamageData, IDamageableData>{}
        public class NoticeBroken : MessageBusBroadcaster<IDamageableData>{}
        
        public class SetObserveArea : MessageBusBroadcaster<int>{}
        
        public class UserCommandOpenItemDataMenu : MessageBusBroadcaster<ItemData, Action, string, string>{}
        public class UserCommandCloseItemDataMenu : MessageBusBroadcaster{}
        public class UserCommandGlobalMapFocusCell : MessageBusBroadcaster<int, bool>{}
        public class UserCommandUpdateInventory : MessageBusBroadcaster<Guid[]>{}
        public class UserCommandRotateCamera : MessageBusBroadcaster<Vector2>{}
        public class UserCommandSetCameraAngle : MessageBusBroadcaster<Quaternion>{}
        
        public class UserCommandSetAmbientCameraPosition : MessageBusBroadcaster<Vector3>{}
        public class UserCommandGetWorldToCanvasPoint : MessageBusBroadcaster<CameraController.CameraType, Vector3, RectTransform, Action<Vector3?>>{}
        
        public class PlayerCommandSetMoveTarget : MessageBusBroadcaster<Guid, IPosition>{}
        public class PlayerCommandSetInteractOrder : MessageBusBroadcaster<ActorData, IInteractData>{}
        public class PlayerCommandSetTacticsType : MessageBusBroadcaster<Guid, TacticsType>{}
        
        public class ManagerCommandTransitionActor : MessageBusBroadcaster<ActorData, int, int>{}
        
        public class ManagerCommandStoreItem : MessageBusBroadcaster<int, InventoryData, ItemData>{}
        public class ManagerCommandTransferItem : MessageBusBroadcaster<InventoryData, InventoryData, ItemData>{}

        public class ExecuteTriggerWeapon : MessageBusBroadcaster<WeaponData, ITargetData, float>{}
    }
}
