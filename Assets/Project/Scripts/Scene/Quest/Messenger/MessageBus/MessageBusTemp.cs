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

            // イベント通知
            public MessageBusDefineTemp.NoticeCollisionEventData NoticeCollisionEventData { get; } = new MessageBusDefineTemp.NoticeCollisionEventData();
            public MessageBusDefineTemp.NoticeCollisionEventEffectData NoticeCollisionEventEffectData { get; } = new MessageBusDefineTemp.NoticeCollisionEventEffectData();
            public MessageBusDefineTemp.NoticeDamageEventData NoticeDamageEventData { get; } = new MessageBusDefineTemp.NoticeDamageEventData();
            public MessageBusDefineTemp.NoticeBrokenActorEventData NoticeBrokenActorEventData { get; } = new MessageBusDefineTemp.NoticeBrokenActorEventData();
        }
    }
}
