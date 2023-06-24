namespace AloneSpace
{
    public partial class MessageBus
    {
        // UI
        public MessageBusDefine.UserInputSwitchMap UserInputSwitchMap { get; } = new MessageBusDefine.UserInputSwitchMap();
        public MessageBusDefine.UserInputOpenMap UserInputOpenMap { get; } = new MessageBusDefine.UserInputOpenMap();
        public MessageBusDefine.UserInputCloseMap UserInputCloseMap { get; } = new MessageBusDefine.UserInputCloseMap();
        public MessageBusDefine.UserInputSwitchInteractList UserInputSwitchInteractList { get; } = new MessageBusDefine.UserInputSwitchInteractList();
        public MessageBusDefine.UserInputOpenInteractList UserInputOpenInteractList { get; } = new MessageBusDefine.UserInputOpenInteractList();
        public MessageBusDefine.UserInputCloseInteractList UserInputCloseInteractList { get; } = new MessageBusDefine.UserInputCloseInteractList();
        public MessageBusDefine.UserInputSwitchInventory UserInputSwitchInventory { get; } = new MessageBusDefine.UserInputSwitchInventory();
        public MessageBusDefine.UserInputOpenInventory UserInputOpenInventory { get; } = new MessageBusDefine.UserInputOpenInventory();
        public MessageBusDefine.UserInputCloseInventory UserInputCloseInventory { get; } = new MessageBusDefine.UserInputCloseInventory();

        public MessageBusDefine.UserInputExecuteWeapon UserInputSetExecuteWeapon { get; } = new MessageBusDefine.UserInputExecuteWeapon();
        public MessageBusDefine.UserInputReloadWeapon UserInputReloadWeapon { get; } = new MessageBusDefine.UserInputReloadWeapon();
        public MessageBusDefine.UserInputSetCurrentWeaponGroupIndex UserInputSetCurrentWeaponGroupIndex { get; } = new MessageBusDefine.UserInputSetCurrentWeaponGroupIndex();

        public MessageBusDefine.UserInputFrontBoosterPower UserInputForwardBoosterPowerRatio { get; } = new MessageBusDefine.UserInputFrontBoosterPower();
        public MessageBusDefine.UserInputBackBoosterPower UserInputBackBoosterPowerRatio { get; } = new MessageBusDefine.UserInputBackBoosterPower();
        public MessageBusDefine.UserInputRightBoosterPower UserInputRightBoosterPowerRatio { get; } = new MessageBusDefine.UserInputRightBoosterPower();
        public MessageBusDefine.UserInputLeftBoosterPower UserInputLeftBoosterPowerRatio { get; } = new MessageBusDefine.UserInputLeftBoosterPower();
        public MessageBusDefine.UserInputTopBoosterPower UserInputTopBoosterPowerRatio { get; } = new MessageBusDefine.UserInputTopBoosterPower();
        public MessageBusDefine.UserInputBottomBoosterPower UserInputBottomBoosterPowerRatio { get; } = new MessageBusDefine.UserInputBottomBoosterPower();
        public MessageBusDefine.UserInputPitchBoosterPower UserInputPitchBoosterPowerRatio { get; } = new MessageBusDefine.UserInputPitchBoosterPower();
        public MessageBusDefine.UserInputRollBoosterPower UserInputRollBoosterPowerRatio { get; } = new MessageBusDefine.UserInputRollBoosterPower();
        public MessageBusDefine.UserInputYawBoosterPower UserInputYawBoosterPowerRatio { get; } = new MessageBusDefine.UserInputYawBoosterPower();

        public MessageBusDefine.UserCommandSetActorOperationMode UserCommandSetActorOperationMode { get; } = new MessageBusDefine.UserCommandSetActorOperationMode();

        public MessageBusDefine.UserCommandOpenItemDataMenu UserCommandOpenItemDataMenu { get; } = new MessageBusDefine.UserCommandOpenItemDataMenu();
        public MessageBusDefine.UserCommandCloseItemDataMenu UserCommandCloseItemDataMenu { get; } = new MessageBusDefine.UserCommandCloseItemDataMenu();
        public MessageBusDefine.UserCommandUpdateInventory UserCommandUpdateInventory { get; } = new MessageBusDefine.UserCommandUpdateInventory();

        public MessageBusDefine.UserCommandSetCameraTrackTarget UserCommandSetCameraTrackTarget { get; } = new MessageBusDefine.UserCommandSetCameraTrackTarget();
        public MessageBusDefine.UserCommandGetWorldToCanvasPoint UserCommandGetWorldToCanvasPoint { get; } = new MessageBusDefine.UserCommandGetWorldToCanvasPoint();

        public MessageBusDefine.UserCommandSetLookAtAngle UserCommandSetLookAtAngle { get; } = new MessageBusDefine.UserCommandSetLookAtAngle();
        public MessageBusDefine.UserCommandSetLookAtSpace UserCommandSetLookAtSpace { get; } = new MessageBusDefine.UserCommandSetLookAtSpace();
        public MessageBusDefine.UserCommandSetLookAtDistance UserCommandSetLookAtDistance { get; } = new MessageBusDefine.UserCommandSetLookAtDistance();
    }
}
