namespace AloneSpace
{
    public partial class MessageBus
    {
        public ActorMessage Actor { get; } = new ActorMessage();

        public class ActorMessage
        {
            public MessageBusDefineActor.SetWeaponExecute SetWeaponExecute { get; } = new MessageBusDefineActor.SetWeaponExecute();
            public MessageBusDefineActor.ReloadWeapon ReloadWeapon { get; } = new MessageBusDefineActor.ReloadWeapon();
            public MessageBusDefineActor.SetCurrentWeaponGroupIndex SetCurrentWeaponGroupIndex { get; } = new MessageBusDefineActor.SetCurrentWeaponGroupIndex();

            public MessageBusDefineActor.ForwardBoosterPower ForwardBoosterPowerRatio { get; } = new MessageBusDefineActor.ForwardBoosterPower();
            public MessageBusDefineActor.BackBoosterPower BackBoosterPowerRatio { get; } = new MessageBusDefineActor.BackBoosterPower();
            public MessageBusDefineActor.RightBoosterPower RightBoosterPowerRatio { get; } = new MessageBusDefineActor.RightBoosterPower();
            public MessageBusDefineActor.LeftBoosterPower LeftBoosterPowerRatio { get; } = new MessageBusDefineActor.LeftBoosterPower();
            public MessageBusDefineActor.TopBoosterPower TopBoosterPowerRatio { get; } = new MessageBusDefineActor.TopBoosterPower();
            public MessageBusDefineActor.BottomBoosterPower BottomBoosterPowerRatio { get; } = new MessageBusDefineActor.BottomBoosterPower();
            public MessageBusDefineActor.PitchBoosterPower PitchBoosterPowerRatio { get; } = new MessageBusDefineActor.PitchBoosterPower();
            public MessageBusDefineActor.RollBoosterPower RollBoosterPowerRatio { get; } = new MessageBusDefineActor.RollBoosterPower();
            public MessageBusDefineActor.YawBoosterPower YawBoosterPowerRatio { get; } = new MessageBusDefineActor.YawBoosterPower();

            public MessageBusDefineActor.SetLookAtDirection SetLookAtDirection { get; } = new MessageBusDefineActor.SetLookAtDirection();

            public MessageBusDefineActor.SetMainTarget SetMainTarget { get; } = new MessageBusDefineActor.SetMainTarget();
        }
    }
}
