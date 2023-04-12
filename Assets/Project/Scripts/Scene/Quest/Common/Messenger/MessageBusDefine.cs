using System;
using UnityEngine;

namespace AloneSpace
{
    public class MessageBusDefine
    {
        public class AddWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}
        public class RemoveWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}
        
        public class SendCollision : MessageBusBroadcaster<ICollisionDataHolder, bool>{}
        
        public class UtilGetPlayerQuestData : MessageBusUnicaster<Guid, PlayerQuestData>{}
        public class UtilGetAreaData : MessageBusUnicaster<int, AreaData>{}
        public class UtilGetAreaActorData : MessageBusUnicaster<int, ActorData[]>{}
        
        public class ManagerCommandSetObservePlayer : MessageBusBroadcaster<Guid>{}
        public class ManagerCommandLoadArea : MessageBusBroadcaster<int?>{}
        
        public class NoticeCollisionEffectData : MessageBusBroadcaster<CollisionEffectData>{}
        
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
        
        public class UserInputSwitchActorMode : MessageBusBroadcaster {}
        public class UserInputSetActorCombatMode : MessageBusBroadcaster <ActorCombatMode>{}
        
        public class UserCommandSetCameraTrackTarget : MessageBusBroadcaster<IPositionData>{}
        public class UserCommandGetWorldToCanvasPoint : MessageBusUnicaster<CameraController.CameraType, Vector3, RectTransform, Vector3?>{}
        
        public class UserCommandSetLookAtAngle : MessageBusBroadcaster<Vector3>{}
        public class UserCommandSetLookAtSpace : MessageBusBroadcaster<Quaternion>{}
        
        public class PlayerCommandSetAreaId : MessageBusBroadcaster<ActorData, int?>{}
        public class PlayerCommandSetMoveTarget : MessageBusBroadcaster<ActorData, IPositionData>{}
        public class PlayerCommandSetInteractOrder : MessageBusBroadcaster<ActorData, IInteractData>{}
        public class PlayerCommandSetTacticsType : MessageBusBroadcaster<Guid, TacticsType>{}
        
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
        
        public class ActorCommandSetActorMode : MessageBusBroadcaster<Guid, ActorMode>{}
        public class ActorCommandSetActorCombatMode : MessageBusBroadcaster<Guid, ActorCombatMode>{}
        
        public class ManagerCommandPickItem : MessageBusBroadcaster<InventoryData, ItemInteractData>{}
        public class ManagerCommandTransferItem : MessageBusBroadcaster<InventoryData, InventoryData, ItemData>{}

        public class ExecuteTriggerWeapon : MessageBusBroadcaster<WeaponData, IPositionData, float>{}
        
        public class SetDirtyActorObjectList : MessageBusBroadcaster{}
        public class SetDirtyInteractObjectList : MessageBusBroadcaster{}
    }
}
