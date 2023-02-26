using System;
using System.Collections.Generic;
using System.Linq;

namespace AloneSpace
{
    public class MessageBus : MonoSingleton<MessageBus>
    {
        // Data
        public MessageBusDefine.AddWeaponEffectData AddWeaponEffectData { get; } = new MessageBusDefine.AddWeaponEffectData();
        public MessageBusDefine.RemoveWeaponEffectData RemoveWeaponEffectData { get; } = new MessageBusDefine.RemoveWeaponEffectData();
        
        // 登録系メッセージ
        public MessageBusDefine.SendThreat SendThreat { get; } = new MessageBusDefine.SendThreat();
        public MessageBusDefine.SendIntuition SendIntuition { get; } = new MessageBusDefine.SendIntuition();
        public MessageBusDefine.SendCollision SendCollision { get; } = new MessageBusDefine.SendCollision();

        // Util
        public MessageBusDefine.UtilGetAreaData UtilGetAreaData { get; } = new MessageBusDefine.UtilGetAreaData();
        
        // プレイヤー設定
        public MessageBusDefine.ManagerCommandSetObservePlayer ManagerCommandSetObservePlayer { get; } = new MessageBusDefine.ManagerCommandSetObservePlayer();
        public MessageBusDefine.ManagerCommandLoadArea ManagerCommandLoadArea { get; } = new MessageBusDefine.ManagerCommandLoadArea();

        // イベント通知
        public MessageBusDefine.NoticeHitThreat NoticeHitThreat { get; } = new MessageBusDefine.NoticeHitThreat();
        public MessageBusDefine.NoticeHitCollision NoticeHitCollision { get; } = new MessageBusDefine.NoticeHitCollision();
        public MessageBusDefine.NoticeDamage NoticeDamage { get; } = new MessageBusDefine.NoticeDamage();
        public MessageBusDefine.NoticeBroken NoticeBroken { get; } = new MessageBusDefine.NoticeBroken();

        // UI
        public MessageBusDefine.UserInputSwitchMap UserInputSwitchMap { get; } = new MessageBusDefine.UserInputSwitchMap(); 
        public MessageBusDefine.UserInputOpenMap UserInputOpenMap { get; } = new MessageBusDefine.UserInputOpenMap();
        public MessageBusDefine.UserInputCloseMap UserInputCloseMap { get; } = new MessageBusDefine.UserInputCloseMap();
        public MessageBusDefine.UserInputSwitchInteractList UserInputSwitchInteractList { get; } = new MessageBusDefine.UserInputSwitchInteractList(); 
        public MessageBusDefine.UserInputOpenInteractList UserInputOpenInteractList { get; } = new MessageBusDefine.UserInputOpenInteractList();
        public MessageBusDefine.UserInputCloseInteractList UserInputCloseInteractList { get; } = new MessageBusDefine.UserInputCloseInteractList();
        public MessageBusDefine.UserInputSwitchInventory UserInputSwitchInventory { get; } = new MessageBusDefine.UserInputSwitchInventory(); 
        public MessageBusDefine.UserInputOpenInventory UserInputOpenInventory { get; } = new MessageBusDefine.UserInputOpenInventory();
        public MessageBusDefine.UserInputCloseInventory UserInputCloseInventory { get; } = new MessageBusDefine.UserInputCloseInventory();
        
        public MessageBusDefine.UserInputFrontBoosterPower UserInputFrontBoosterPowerRatio { get; } = new MessageBusDefine.UserInputFrontBoosterPower();
        public MessageBusDefine.UserInputBackBoosterPower UserInputBackBoosterPowerRatio { get; } = new MessageBusDefine.UserInputBackBoosterPower();
        public MessageBusDefine.UserInputRightBoosterPower UserInputRightBoosterPowerRatio { get; } = new MessageBusDefine.UserInputRightBoosterPower();
        public MessageBusDefine.UserInputLeftBoosterPower UserInputLeftBoosterPowerRatio { get; } = new MessageBusDefine.UserInputLeftBoosterPower();
        public MessageBusDefine.UserInputTopBoosterPower UserInputTopBoosterPowerRatio { get; } = new MessageBusDefine.UserInputTopBoosterPower();
        public MessageBusDefine.UserInputBottomBoosterPower UserInputBottomBoosterPowerRatio { get; } = new MessageBusDefine.UserInputBottomBoosterPower();
        public MessageBusDefine.UserInputPitchBoosterPower UserInputPitchBoosterPowerRatio { get; } = new MessageBusDefine.UserInputPitchBoosterPower();
        public MessageBusDefine.UserInputRollBoosterPower UserInputRollBoosterPowerRatio { get; } = new MessageBusDefine.UserInputRollBoosterPower();
        public MessageBusDefine.UserInputYawBoosterPower UserInputYawBoosterPowerRatio { get; } = new MessageBusDefine.UserInputYawBoosterPower();
        
        public MessageBusDefine.UserInputSwitchActorMode UserInputSwitchActorMode { get; } = new MessageBusDefine.UserInputSwitchActorMode();
        public MessageBusDefine.UserInputSetActorCombatMode UserInputSetActorCombatMode { get; } = new MessageBusDefine.UserInputSetActorCombatMode();
        
        // ゲームコマンド
        public MessageBusDefine.UserCommandOpenItemDataMenu UserCommandOpenItemDataMenu { get; } = new MessageBusDefine.UserCommandOpenItemDataMenu();
        public MessageBusDefine.UserCommandCloseItemDataMenu UserCommandCloseItemDataMenu { get; } = new MessageBusDefine.UserCommandCloseItemDataMenu();
        public MessageBusDefine.UserCommandUpdateInventory UserCommandUpdateInventory { get; } = new MessageBusDefine.UserCommandUpdateInventory();
        public MessageBusDefine.UserCommandSetCameraMode UserCommandSetCameraMode { get; } = new MessageBusDefine.UserCommandSetCameraMode();
        public MessageBusDefine.UserCommandRotateCamera UserCommandRotateCamera { get; } = new MessageBusDefine.UserCommandRotateCamera();
        public MessageBusDefine.UserCommandSetCameraAngle UserCommandSetCameraAngle { get; } = new MessageBusDefine.UserCommandSetCameraAngle();

        public MessageBusDefine.UserCommandSetCameraTrackTarget UserCommandSetCameraTrackTarget { get; } = new MessageBusDefine.UserCommandSetCameraTrackTarget();
        public MessageBusDefine.UserCommandGetWorldToCanvasPoint UserCommandGetWorldToCanvasPoint { get; } = new MessageBusDefine.UserCommandGetWorldToCanvasPoint();
        
        // Playerによるゲームコマンド
        public MessageBusDefine.PlayerCommandSetAreaId PlayerCommandSetAreaId { get; } = new MessageBusDefine.PlayerCommandSetAreaId();
        public MessageBusDefine.PlayerCommandSetMoveTarget PlayerCommandSetMoveTarget { get; } = new MessageBusDefine.PlayerCommandSetMoveTarget();
        public MessageBusDefine.PlayerCommandSetInteractOrder PlayerCommandSetInteractOrder { get; } = new MessageBusDefine.PlayerCommandSetInteractOrder();
        public MessageBusDefine.PlayerCommandSetTacticsType PlayerCommandSetTacticsType { get; } = new MessageBusDefine.PlayerCommandSetTacticsType();
        
        public MessageBusDefine.ActorCommandFrontBoosterPower ActorCommandFrontBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandFrontBoosterPower();
        public MessageBusDefine.ActorCommandBackBoosterPower ActorCommandBackBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandBackBoosterPower();
        public MessageBusDefine.ActorCommandRightBoosterPower ActorCommandRightBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandRightBoosterPower();
        public MessageBusDefine.ActorCommandLeftBoosterPower ActorCommandLeftBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandLeftBoosterPower();
        public MessageBusDefine.ActorCommandTopBoosterPower ActorCommandTopBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandTopBoosterPower();
        public MessageBusDefine.ActorCommandBottomBoosterPower ActorCommandBottomBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandBottomBoosterPower();
        public MessageBusDefine.ActorCommandPitchBoosterPower ActorCommandPitchBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandPitchBoosterPower();
        public MessageBusDefine.ActorCommandRollBoosterPower ActorCommandRollBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandRollBoosterPower();
        public MessageBusDefine.ActorCommandYawBoosterPower ActorCommandYawBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandYawBoosterPower();
        
        public MessageBusDefine.ActorCommandSetActorMode ActorCommandSetActorMode { get; } = new MessageBusDefine.ActorCommandSetActorMode();
        public MessageBusDefine.ActorCommandSetActorCombatMode ActorCommandSetActorCombatMode { get; } = new MessageBusDefine.ActorCommandSetActorCombatMode();
        
        // ActorのItem収集
        public MessageBusDefine.ManagerCommandPickItem ManagerCommandPickItem { get; } = new MessageBusDefine.ManagerCommandPickItem();
        public MessageBusDefine.ManagerCommandTransferItem ManagerCommandTransferItem { get; } = new MessageBusDefine.ManagerCommandTransferItem();
        
        // Weapon
        public MessageBusDefine.ExecuteTriggerWeapon ExecuteTriggerWeapon { get; } = new MessageBusDefine.ExecuteTriggerWeapon();
        
        public MessageBusDefine.SetDirtyActorObjectList SetDirtyActorObjectList { get; } = new MessageBusDefine.SetDirtyActorObjectList();
        public MessageBusDefine.SetDirtyInteractObjectList SetDirtyInteractObjectList { get; } = new MessageBusDefine.SetDirtyInteractObjectList();
    }
}
