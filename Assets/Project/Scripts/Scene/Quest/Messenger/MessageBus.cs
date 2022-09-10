using System;
using System.Collections.Generic;
using System.Linq;

namespace AloneSpace
{
    public class MessageBus : MonoSingleton<MessageBus>
    {
        // 登録系メッセージ
        public MessageBusDefine.SendTarget SendTarget { get; } = new MessageBusDefine.SendTarget();
        public MessageBusDefine.SendThreat SendThreat { get; } = new MessageBusDefine.SendThreat();
        public MessageBusDefine.SendIntuition SendIntuition { get; } = new MessageBusDefine.SendIntuition();
        public MessageBusDefine.SendCollision SendCollision { get; } = new MessageBusDefine.SendCollision();
        public MessageBusDefine.SendInteractionObject SendInteractionObject { get; } = new MessageBusDefine.SendInteractionObject();

        // QuestDataの変更通知
        public MessageBusDefine.UpdateInteractData UpdateInteractData { get; } = new MessageBusDefine.UpdateInteractData();
        
        // リストに更新があった時の通知メッセージ
        public MessageBusDefine.SubscribeUpdateAll SubscribeUpdateAll { get; } = new MessageBusDefine.SubscribeUpdateAll();
        public MessageBusDefine.SubscribeUpdateTargetList SubscribeUpdateTargetList { get; } = new MessageBusDefine.SubscribeUpdateTargetList();
        public MessageBusDefine.SubscribeUpdateInteractionObjectList SubscribeUpdateInteractionObjectList { get; } = new MessageBusDefine.SubscribeUpdateInteractionObjectList();
        public MessageBusDefine.SubscribeUpdateActorList SubscribeUpdateActorList { get; } = new MessageBusDefine.SubscribeUpdateActorList();
        
        // イベント通知
        public MessageBusDefine.NoticeHitThreat NoticeHitThreat { get; } = new MessageBusDefine.NoticeHitThreat();
        public MessageBusDefine.NoticeHitCollision NoticeHitCollision { get; } = new MessageBusDefine.NoticeHitCollision();
        public MessageBusDefine.NoticeDamage NoticeDamage { get; } = new MessageBusDefine.NoticeDamage();
        public MessageBusDefine.NoticeBroken NoticeBroken { get; } = new MessageBusDefine.NoticeBroken();

        // 間接的なユーザー操作メッセージ
        public MessageBusDefine.SetObserveArea SetObserveArea { get; } = new MessageBusDefine.SetObserveArea();

        // Userによるゲームコマンド（UIなど）
        public MessageBusDefine.UserCommandSetObservePlayer UserCommandSetObservePlayer { get; } = new MessageBusDefine.UserCommandSetObservePlayer();
        public MessageBusDefine.UserCommandSetObserveActor UserCommandSetObserveActor { get; } = new MessageBusDefine.UserCommandSetObserveActor();
        public MessageBusDefine.UserCommandOpenItemDataMenu UserCommandOpenItemDataMenu { get; } = new MessageBusDefine.UserCommandOpenItemDataMenu();
        public MessageBusDefine.UserCommandCloseItemDataMenu UserCommandCloseItemDataMenu { get; } = new MessageBusDefine.UserCommandCloseItemDataMenu();
        public MessageBusDefine.UserCommandChangeCameraMode UserCommandSetCameraMode { get; } = new MessageBusDefine.UserCommandChangeCameraMode();
        public MessageBusDefine.UserCommandSetCameraFocusObject UserCommandSetCameraFocusObject { get; } = new MessageBusDefine.UserCommandSetCameraFocusObject();
        public MessageBusDefine.UserCommandGlobalMapFocusCell UserCommandGlobalMapFocusCell { get; } = new MessageBusDefine.UserCommandGlobalMapFocusCell();
        public MessageBusDefine.UserCommandUpdateInventory UserCommandUpdateInventory { get; } = new MessageBusDefine.UserCommandUpdateInventory();
        public MessageBusDefine.UserCommandRotateCamera UserCommandRotateCamera { get; } = new MessageBusDefine.UserCommandRotateCamera();
        public MessageBusDefine.UserCommandSetCameraAngle UserCommandSetCameraAngle { get; } = new MessageBusDefine.UserCommandSetCameraAngle();
        
        // Playerによるゲームコマンド
        public MessageBusDefine.PlayerCommandSetDestinateAreaIndex PlayerCommandSetDestinateAreaIndex { get; } = new MessageBusDefine.PlayerCommandSetDestinateAreaIndex();
        public MessageBusDefine.PlayerCommandAddInteractItemOrder PlayerCommandAddInteractItemOrder { get; } = new MessageBusDefine.PlayerCommandAddInteractItemOrder();
        public MessageBusDefine.PlayerCommandRemoveInteractItemOrder PlayerCommandRemoveInteractItemOrder { get; } = new MessageBusDefine.PlayerCommandRemoveInteractItemOrder();
        public MessageBusDefine.PlayerCommandSetTacticsType PlayerCommandSetTacticsType { get; } = new MessageBusDefine.PlayerCommandSetTacticsType();
        
        // Managerによるゲームコマンド Actor周り
        // ActorのArea移動
        public MessageBusDefine.ManagerCommandTransitionActor ManagerCommandTransitionActor { get; } = new MessageBusDefine.ManagerCommandTransitionActor();
        
        // ActorのItem収集
        public MessageBusDefine.ManagerCommandStoreItem ManagerCommandStoreItem { get; } = new MessageBusDefine.ManagerCommandStoreItem();
        public MessageBusDefine.ManagerCommandTransferItem ManagerCommandTransferItem { get; } = new MessageBusDefine.ManagerCommandTransferItem();
        
        // Actorによるゲームコマンド Actor内で閉じてる
        public MessageBusDefine.ExecuteTriggerWeapon ExecuteTriggerWeapon { get; } = new MessageBusDefine.ExecuteTriggerWeapon();
    }
}
