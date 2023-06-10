namespace AloneSpace
{
    public partial class MessageBus
    {
        // Util
        public MessageBusDefine.UtilGetPlayerQuestData UtilGetPlayerQuestData { get; } = new MessageBusDefine.UtilGetPlayerQuestData();
        public MessageBusDefine.UtilGetAreaData UtilGetAreaData { get; } = new MessageBusDefine.UtilGetAreaData();
        public MessageBusDefine.UtilGetAreaActorData UtilGetAreaActorData { get; } = new MessageBusDefine.UtilGetAreaActorData();
    }
}