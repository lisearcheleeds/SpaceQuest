namespace AloneSpace
{
    public partial class MessageBus
    {
        public UserMessage User { get; } = new UserMessage();

        public class UserMessage
        {
            // プレイヤー設定
            public MessageBusDefineUser.SetPlayer SetPlayer { get; } = new MessageBusDefineUser.SetPlayer();
            public MessageBusDefineUser.SetControlActor SetControlActor { get; } = new MessageBusDefineUser.SetControlActor();
            public MessageBusDefineUser.SetObserveTarget SetObserveTarget { get; } = new MessageBusDefineUser.SetObserveTarget();
            public MessageBusDefineUser.SetObserveArea SetObserveArea { get; } = new MessageBusDefineUser.SetObserveArea();
        }
    }
}
