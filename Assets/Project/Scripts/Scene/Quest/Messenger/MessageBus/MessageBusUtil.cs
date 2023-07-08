namespace AloneSpace
{
    public partial class MessageBus
    {
        // Util
        public MessageBusDefine.UtilGetPlayerData UtilGetPlayerData { get; } = new MessageBusDefine.UtilGetPlayerData();
        public MessageBusDefine.UtilGetAreaData UtilGetAreaData { get; } = new MessageBusDefine.UtilGetAreaData();
        public MessageBusDefine.UtilGetAreaActorData UtilGetAreaActorData { get; } = new MessageBusDefine.UtilGetAreaActorData();
    }
}
