namespace AloneSpace
{
    public partial class MessageBus
    {
        public MessageBusDefine.ActorCommandSetWeaponExecute ActorCommandSetWeaponExecute { get; } = new MessageBusDefine.ActorCommandSetWeaponExecute();
        public MessageBusDefine.ActorCommandReloadWeapon ActorCommandReloadWeapon { get; } = new MessageBusDefine.ActorCommandReloadWeapon();
        public MessageBusDefine.ActorCommandSetCurrentWeaponGroupIndex ActorCommandSetCurrentWeaponGroupIndex { get; } = new MessageBusDefine.ActorCommandSetCurrentWeaponGroupIndex();

        public MessageBusDefine.ActorCommandForwardBoosterPower ActorCommandForwardBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandForwardBoosterPower();
        public MessageBusDefine.ActorCommandBackBoosterPower ActorCommandBackBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandBackBoosterPower();
        public MessageBusDefine.ActorCommandRightBoosterPower ActorCommandRightBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandRightBoosterPower();
        public MessageBusDefine.ActorCommandLeftBoosterPower ActorCommandLeftBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandLeftBoosterPower();
        public MessageBusDefine.ActorCommandTopBoosterPower ActorCommandTopBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandTopBoosterPower();
        public MessageBusDefine.ActorCommandBottomBoosterPower ActorCommandBottomBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandBottomBoosterPower();
        public MessageBusDefine.ActorCommandPitchBoosterPower ActorCommandPitchBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandPitchBoosterPower();
        public MessageBusDefine.ActorCommandRollBoosterPower ActorCommandRollBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandRollBoosterPower();
        public MessageBusDefine.ActorCommandYawBoosterPower ActorCommandYawBoosterPowerRatio { get; } = new MessageBusDefine.ActorCommandYawBoosterPower();

        public MessageBusDefine.ActorCommandSetLookAtDirection ActorCommandSetLookAtDirection { get; } = new MessageBusDefine.ActorCommandSetLookAtDirection();

        public MessageBusDefine.ActorCommandSetActorMode ActorCommandSetActorMode { get; } = new MessageBusDefine.ActorCommandSetActorMode();
        public MessageBusDefine.ActorCommandSetActorCombatMode ActorCommandSetActorCombatMode { get; } = new MessageBusDefine.ActorCommandSetActorCombatMode();

        public MessageBusDefine.ActorCommandSetMainTarget ActorCommandSetMainTarget { get; } = new MessageBusDefine.ActorCommandSetMainTarget();
        public MessageBusDefine.ActorCommandSetAroundTargets ActorCommandSetAroundTargets { get; } = new MessageBusDefine.ActorCommandSetAroundTargets();
    }
}
