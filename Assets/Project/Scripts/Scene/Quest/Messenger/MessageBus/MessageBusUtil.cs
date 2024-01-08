namespace AloneSpace
{
    public partial class MessageBus
    {
        public UtilMessage Util { get; } = new UtilMessage();

        public class UtilMessage
        {
            public MessageBusDefineUtil.GetPlayerData GetPlayerData { get; } = new MessageBusDefineUtil.GetPlayerData();
            public MessageBusDefineUtil.GetAreaData GetAreaData { get; } = new MessageBusDefineUtil.GetAreaData();
            public MessageBusDefineUtil.GetAreaActorData GetAreaActorData { get; } = new MessageBusDefineUtil.GetAreaActorData();
            
            public MessageBusDefineUtil.GetWorldToCanvasPoint GetWorldToCanvasPoint { get; } = new MessageBusDefineUtil.GetWorldToCanvasPoint();
            public MessageBusDefineUtil.GetCameraRotation GetCameraRotation { get; } = new MessageBusDefineUtil.GetCameraRotation();
            public MessageBusDefineUtil.GetCameraFieldOfView GetCameraFieldOfView { get; } = new MessageBusDefineUtil.GetCameraFieldOfView();
        }
    }
}
