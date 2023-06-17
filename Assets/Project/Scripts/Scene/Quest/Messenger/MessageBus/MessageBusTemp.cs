namespace AloneSpace
{
    public partial class MessageBus
    {
        // 登録系メッセージ
        public MessageBusDefine.RegisterCollision RegisterCollision { get; } = new MessageBusDefine.RegisterCollision();
        public MessageBusDefine.UnRegisterCollision UnRegisterCollision { get; } = new MessageBusDefine.UnRegisterCollision();

        // プレイヤー設定
        public MessageBusDefine.SetUserPlayer SetUserPlayer { get; } = new MessageBusDefine.SetUserPlayer();
        public MessageBusDefine.SetUserControlActor SetUserControlActor { get; } = new MessageBusDefine.SetUserControlActor();
        public MessageBusDefine.SetUserObserveTarget SetUserObserveTarget { get; } = new MessageBusDefine.SetUserObserveTarget();

        public MessageBusDefine.SetUserObserveArea SetUserObserveArea { get; } = new MessageBusDefine.SetUserObserveArea();

        // イベント通知
        public MessageBusDefine.NoticeCollisionEventData NoticeCollisionEventData { get; } = new MessageBusDefine.NoticeCollisionEventData();
        public MessageBusDefine.NoticeCollisionEventEffectData NoticeCollisionEventEffectData { get; } = new MessageBusDefine.NoticeCollisionEventEffectData();
        public MessageBusDefine.NoticeDamageEventData NoticeDamageEventData { get; } = new MessageBusDefine.NoticeDamageEventData();
        public MessageBusDefine.NoticeBrokenActorEventData NoticeBrokenActorEventData { get; } = new MessageBusDefine.NoticeBrokenActorEventData();

        // Playerによるゲームコマンド
        public MessageBusDefine.PlayerCommandSetAreaId PlayerCommandSetAreaId { get; } = new MessageBusDefine.PlayerCommandSetAreaId();
        public MessageBusDefine.PlayerCommandSetMoveTarget PlayerCommandSetMoveTarget { get; } = new MessageBusDefine.PlayerCommandSetMoveTarget();
        public MessageBusDefine.PlayerCommandSetInteractOrder PlayerCommandSetInteractOrder { get; } = new MessageBusDefine.PlayerCommandSetInteractOrder();
        public MessageBusDefine.PlayerCommandSetTacticsType PlayerCommandSetTacticsType { get; } = new MessageBusDefine.PlayerCommandSetTacticsType();

        // ActorのItem収集
        public MessageBusDefine.ManagerCommandPickItem ManagerCommandPickItem { get; } = new MessageBusDefine.ManagerCommandPickItem();
        public MessageBusDefine.ManagerCommandTransferItem ManagerCommandTransferItem { get; } = new MessageBusDefine.ManagerCommandTransferItem();
    }
}
