using System;
using UnityEngine;

namespace AloneSpace
{
    public class MessageBusDefine
    {
        public class AddWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}
        public class RemoveWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}

        public class RegisterCollision : MessageBusBroadcaster<CollisionEventModule>{}
        public class UnRegisterCollision : MessageBusBroadcaster<CollisionEventModule>{}

        public class RegisterMovingModule : MessageBusBroadcaster<MovingModule>{}
        public class UnRegisterMovingModule : MessageBusBroadcaster<MovingModule>{}
        public class RegisterOrderModule : MessageBusBroadcaster<IOrderModule>{}
        public class UnRegisterOrderModule : MessageBusBroadcaster<IOrderModule>{}
        public class RegisterThinkModule : MessageBusBroadcaster<IThinkModule>{}
        public class UnRegisterThinkModule : MessageBusBroadcaster<IThinkModule>{}
        public class RegisterCollisionEventModule : MessageBusBroadcaster<CollisionEventModule>{}
        public class UnRegisterCollisionEventModule : MessageBusBroadcaster<CollisionEventModule>{}
        public class RegisterCollisionEffectReceiverModuleModule : MessageBusBroadcaster<CollisionEventEffectReceiverModule>{}
        public class UnRegisterCollisionEffectReceiverModuleModule : MessageBusBroadcaster<CollisionEventEffectReceiverModule>{}
        public class RegisterCollisionEffectSenderModuleModule : MessageBusBroadcaster<CollisionEventEffectSenderModule>{}
        public class UnRegisterCollisionEffectSenderModuleModule : MessageBusBroadcaster<CollisionEventEffectSenderModule>{}

        public class UtilGetPlayerQuestData : MessageBusUnicaster<Guid, PlayerQuestData>{}
        public class UtilGetAreaData : MessageBusUnicaster<int, AreaData>{}
        public class UtilGetAreaActorData : MessageBusUnicaster<int, ActorData[]>{}

        public class SetOrderUserPlayer : MessageBusBroadcaster<Guid>{}
        public class SetUserPlayer : MessageBusBroadcaster<PlayerQuestData>{}
        public class SetOrderUserArea : MessageBusBroadcaster<int?>{}
        public class SetUserArea : MessageBusBroadcaster<AreaData>{}

        public class NoticeCollisionEventData : MessageBusBroadcaster<CollisionEventData>{}
        public class NoticeCollisionEventEffectData : MessageBusBroadcaster<CollisionEventEffectData>{}

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

        public class UserInputSwitchActorMode : MessageBusBroadcaster {}
        public class UserInputSetActorCombatMode : MessageBusBroadcaster <ActorCombatMode>{}

        public class UserCommandSetCameraTrackTarget : MessageBusBroadcaster<IPositionData>{}
        public class UserCommandGetWorldToCanvasPoint : MessageBusUnicaster<CameraController.CameraType, Vector3, RectTransform, Vector3?>{}

        public class UserCommandSetLookAtAngle : MessageBusBroadcaster<Vector3>{}
        public class UserCommandSetLookAtSpace : MessageBusBroadcaster<Quaternion>{}
        public class UserCommandSetLookAtDistance : MessageBusBroadcaster<float>{}

        public class PlayerCommandSetAreaId : MessageBusBroadcaster<ActorData, int?>{}
        public class PlayerCommandSetMoveTarget : MessageBusBroadcaster<ActorData, IPositionData>{}
        public class PlayerCommandSetInteractOrder : MessageBusBroadcaster<ActorData, IInteractData>{}
        public class PlayerCommandSetTacticsType : MessageBusBroadcaster<Guid, TacticsType>{}

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

        public class ActorCommandSetActorMode : MessageBusBroadcaster<Guid, ActorMode>{}
        public class ActorCommandSetActorCombatMode : MessageBusBroadcaster<Guid, ActorCombatMode>{}

        public class ActorCommandSetMainTarget : MessageBusBroadcaster<Guid, IPositionData>{}
        public class ActorCommandSetAroundTargets : MessageBusBroadcaster<Guid, IPositionData[]>{}

        public class ManagerCommandPickItem : MessageBusBroadcaster<InventoryData, ItemInteractData>{}
        public class ManagerCommandTransferItem : MessageBusBroadcaster<InventoryData, InventoryData, ItemData>{}

        public class CreateWeaponEffectData : MessageBusBroadcaster<IWeaponEffectSpecVO, WeaponData, IPositionData, Quaternion, IPositionData>{}
        public class ReleaseWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}

        public class SpawnGraphicEffect : MessageBusBroadcaster<GraphicEffectSpecVO, IGraphicEffectHandler>{}

        public class SetDirtyActorObjectList : MessageBusBroadcaster{}
        public class SetDirtyInteractObjectList : MessageBusBroadcaster{}
        public class SetDirtyWeaponEffectObjectList : MessageBusBroadcaster{}

        public class GetCacheAsset : MessageBusBroadcaster<CacheableGameObjectPath, Action<CacheableGameObject>>{}
        public class ReleaseCacheAssetAll : MessageBusBroadcaster{}
        public class ReleaseCacheAsset : MessageBusBroadcaster<CacheableGameObject>{}
    }
}
