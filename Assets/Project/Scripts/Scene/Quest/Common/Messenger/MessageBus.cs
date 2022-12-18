using System;
using System.Collections.Generic;
using System.Linq;

namespace AloneSpace
{
    public class MessageBus : MonoSingleton<MessageBus>
    {
        public MessageBusDefine.AddPlayerQuestData AddPlayerQuestData { get; } = new MessageBusDefine.AddPlayerQuestData();
        public MessageBusDefine.AddActorData AddActorData { get; } = new MessageBusDefine.AddActorData();
        public MessageBusDefine.RemoveActorData RemoveActorData { get; } = new MessageBusDefine.RemoveActorData();
        public MessageBusDefine.AddWeaponEffectData AddWeaponEffectData { get; } = new MessageBusDefine.AddWeaponEffectData();
        public MessageBusDefine.RemoveWeaponEffectData RemoveWeaponEffectData { get; } = new MessageBusDefine.RemoveWeaponEffectData();
        
        // 登録系メッセージ
        public MessageBusDefine.SendThreat SendThreat { get; } = new MessageBusDefine.SendThreat();
        public MessageBusDefine.SendIntuition SendIntuition { get; } = new MessageBusDefine.SendIntuition();
        public MessageBusDefine.SendCollision SendCollision { get; } = new MessageBusDefine.SendCollision();

        // QuestDataの変更通知
        public MessageBusDefine.UpdateInteractData UpdateInteractData { get; } = new MessageBusDefine.UpdateInteractData();
        
        // イベント通知
        public MessageBusDefine.NoticeHitThreat NoticeHitThreat { get; } = new MessageBusDefine.NoticeHitThreat();
        public MessageBusDefine.NoticeHitCollision NoticeHitCollision { get; } = new MessageBusDefine.NoticeHitCollision();
        public MessageBusDefine.NoticeDamage NoticeDamage { get; } = new MessageBusDefine.NoticeDamage();
        public MessageBusDefine.NoticeBroken NoticeBroken { get; } = new MessageBusDefine.NoticeBroken();

        // 間接的なユーザー操作メッセージ
        public MessageBusDefine.SetObserveArea SetObserveArea { get; } = new MessageBusDefine.SetObserveArea();

        // Userによるゲームコマンド（UIなど）
        public MessageBusDefine.UserCommandOpenItemDataMenu UserCommandOpenItemDataMenu { get; } = new MessageBusDefine.UserCommandOpenItemDataMenu();
        public MessageBusDefine.UserCommandCloseItemDataMenu UserCommandCloseItemDataMenu { get; } = new MessageBusDefine.UserCommandCloseItemDataMenu();
        public MessageBusDefine.UserCommandGlobalMapFocusCell UserCommandGlobalMapFocusCell { get; } = new MessageBusDefine.UserCommandGlobalMapFocusCell();
        public MessageBusDefine.UserCommandUpdateInventory UserCommandUpdateInventory { get; } = new MessageBusDefine.UserCommandUpdateInventory();
        public MessageBusDefine.UserCommandRotateCamera UserCommandRotateCamera { get; } = new MessageBusDefine.UserCommandRotateCamera();
        public MessageBusDefine.UserCommandSetCameraAngle UserCommandSetCameraAngle { get; } = new MessageBusDefine.UserCommandSetCameraAngle();

        public MessageBusDefine.UserCommandSetAmbientCameraPosition UserCommandSetAmbientCameraPosition { get; } = new MessageBusDefine.UserCommandSetAmbientCameraPosition();
        public MessageBusDefine.UserCommandGetWorldToCanvasPoint UserCommandGetWorldToCanvasPoint { get; } = new MessageBusDefine.UserCommandGetWorldToCanvasPoint();
        
        // Playerによるゲームコマンド
        public MessageBusDefine.PlayerCommandSetMoveTarget PlayerCommandSetMoveTarget { get; } = new MessageBusDefine.PlayerCommandSetMoveTarget();
        public MessageBusDefine.PlayerCommandSetInteractOrder PlayerCommandSetInteractOrder { get; } = new MessageBusDefine.PlayerCommandSetInteractOrder();
        public MessageBusDefine.PlayerCommandSetTacticsType PlayerCommandSetTacticsType { get; } = new MessageBusDefine.PlayerCommandSetTacticsType();
        
        // Managerによるゲームコマンド Actor周り
        // ActorのArea移動
        public MessageBusDefine.ManagerCommandTransitionActor ManagerCommandTransitionActor { get; } = new MessageBusDefine.ManagerCommandTransitionActor();
        
        // ActorのItem収集
        public MessageBusDefine.ManagerCommandStoreItem ManagerCommandStoreItem { get; } = new MessageBusDefine.ManagerCommandStoreItem();
        public MessageBusDefine.ManagerCommandTransferItem ManagerCommandTransferItem { get; } = new MessageBusDefine.ManagerCommandTransferItem();
        
        // Weapon
        public MessageBusDefine.ExecuteTriggerWeapon ExecuteTriggerWeapon { get; } = new MessageBusDefine.ExecuteTriggerWeapon();
    }
}
