namespace AloneSpace
{
    public partial class MessageBus
    {
        public TempMessage Temp { get; } = new TempMessage();

        public class TempMessage
        {
            // 登録系メッセージ
            public MessageBusDefineTemp.RegisterCollision RegisterCollision { get; } = new MessageBusDefineTemp.RegisterCollision();
            public MessageBusDefineTemp.UnRegisterCollision UnRegisterCollision { get; } = new MessageBusDefineTemp.UnRegisterCollision();

            // プレイヤー設定
            public MessageBusDefineTemp.SetUserPlayer SetUserPlayer { get; } = new MessageBusDefineTemp.SetUserPlayer();
            public MessageBusDefineTemp.SetUserControlActor SetUserControlActor { get; } = new MessageBusDefineTemp.SetUserControlActor();
            public MessageBusDefineTemp.SetUserObserveTarget SetUserObserveTarget { get; } = new MessageBusDefineTemp.SetUserObserveTarget();
            public MessageBusDefineTemp.SetUserObserveArea SetUserObserveArea { get; } = new MessageBusDefineTemp.SetUserObserveArea();

            // イベント通知
            public MessageBusDefineTemp.NoticeCollisionEventData NoticeCollisionEventData { get; } = new MessageBusDefineTemp.NoticeCollisionEventData();
            public MessageBusDefineTemp.NoticeCollisionEventEffectData NoticeCollisionEventEffectData { get; } = new MessageBusDefineTemp.NoticeCollisionEventEffectData();
            public MessageBusDefineTemp.NoticeDamageEventData NoticeDamageEventData { get; } = new MessageBusDefineTemp.NoticeDamageEventData();
            public MessageBusDefineTemp.NoticeBrokenActorEventData NoticeBrokenActorEventData { get; } = new MessageBusDefineTemp.NoticeBrokenActorEventData();

            // Playerによるゲームコマンド
            public MessageBusDefineTemp.PlayerCommandSetAreaId PlayerCommandSetAreaId { get; } = new MessageBusDefineTemp.PlayerCommandSetAreaId();
            public MessageBusDefineTemp.PlayerCommandSetMoveTarget PlayerCommandSetMoveTarget { get; } = new MessageBusDefineTemp.PlayerCommandSetMoveTarget();
            public MessageBusDefineTemp.PlayerCommandSetTacticsType PlayerCommandSetTacticsType { get; } = new MessageBusDefineTemp.PlayerCommandSetTacticsType();
            public MessageBusDefineTemp.PlayerCommandAddInteractOrder PlayerCommandAddInteractOrder { get; } = new MessageBusDefineTemp.PlayerCommandAddInteractOrder();
            public MessageBusDefineTemp.PlayerCommandRemoveInteractOrder PlayerCommandRemoveInteractOrder { get; } = new MessageBusDefineTemp.PlayerCommandRemoveInteractOrder();
            
            public MessageBusDefineTemp.OnAddInteractOrder OnAddInteractOrder { get; } = new MessageBusDefineTemp.OnAddInteractOrder();
            public MessageBusDefineTemp.OnRemoveInteractOrder OnRemoveInteractOrder { get; } = new MessageBusDefineTemp.OnRemoveInteractOrder();
            
            // ActorのItem収集
            public MessageBusDefineTemp.ManagerCommandPickItem ManagerCommandPickItem { get; } = new MessageBusDefineTemp.ManagerCommandPickItem();
            public MessageBusDefineTemp.ManagerCommandOnPickItem ManagerCommandOnPickItem { get; } = new MessageBusDefineTemp.ManagerCommandOnPickItem();
            public MessageBusDefineTemp.ManagerCommandDropItem ManagerCommandDropItem { get; } = new MessageBusDefineTemp.ManagerCommandDropItem();
            public MessageBusDefineTemp.ManagerCommandOnDropItem ManagerCommandOnDropItem { get; } = new MessageBusDefineTemp.ManagerCommandOnDropItem();
            public MessageBusDefineTemp.ManagerCommandTransferItem ManagerCommandTransferItem { get; } = new MessageBusDefineTemp.ManagerCommandTransferItem();
        }
    }
}
