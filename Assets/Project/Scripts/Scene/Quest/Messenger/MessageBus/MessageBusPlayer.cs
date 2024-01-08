namespace AloneSpace
{
    public partial class MessageBus
    {
        public PlayerMessage Player { get; } = new PlayerMessage();

        public class PlayerMessage
        {
            // Playerによるゲームコマンド
            public MessageBusDefinePlayer.SetAreaId SetAreaId { get; } = new MessageBusDefinePlayer.SetAreaId();
            public MessageBusDefinePlayer.SetMoveTarget SetMoveTarget { get; } = new MessageBusDefinePlayer.SetMoveTarget();
            public MessageBusDefinePlayer.SetTacticsType SetTacticsType { get; } = new MessageBusDefinePlayer.SetTacticsType();
        }
    }
}