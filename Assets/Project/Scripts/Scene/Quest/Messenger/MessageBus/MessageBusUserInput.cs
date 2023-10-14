namespace AloneSpace
{
    public partial class MessageBus
    {
        // UI
        public MessageBusDefine.UserInputOpenMenu UserInputOpenMenu { get; } = new MessageBusDefine.UserInputOpenMenu();
        public MessageBusDefine.UserInputCloseMenu UserInputCloseMenu { get; } = new MessageBusDefine.UserInputCloseMenu();
        public MessageBusDefine.UserInputSwitchMenuStatusView UserInputSwitchMenuStatusView { get; } = new MessageBusDefine.UserInputSwitchMenuStatusView();
        public MessageBusDefine.UserInputSwitchMenuInventoryView UserInputSwitchMenuInventoryView { get; } = new MessageBusDefine.UserInputSwitchMenuInventoryView();
        public MessageBusDefine.UserInputSwitchMenuPlayerView UserInputSwitchMenuPlayerView { get; } = new MessageBusDefine.UserInputSwitchMenuPlayerView();
        public MessageBusDefine.UserInputSwitchMenuAreaView UserInputSwitchMenuAreaView { get; } = new MessageBusDefine.UserInputSwitchMenuAreaView();
        public MessageBusDefine.UserInputSwitchMenuMapView UserInputSwitchMenuMapView { get; } = new MessageBusDefine.UserInputSwitchMenuMapView();

        public MessageBusDefine.UIMenuStatusViewSelectActorData UIMenuStatusViewSelectActorData { get; } = new MessageBusDefine.UIMenuStatusViewSelectActorData();

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

        public MessageBusDefine.UserCommandSetCameraTrackTarget UserCommandSetCameraTrackTarget { get; } = new MessageBusDefine.UserCommandSetCameraTrackTarget();
        public MessageBusDefine.UserCommandGetWorldToCanvasPoint UserCommandGetWorldToCanvasPoint { get; } = new MessageBusDefine.UserCommandGetWorldToCanvasPoint();

        public MessageBusDefine.UserCommandSetLookAtAngle UserCommandSetLookAtAngle { get; } = new MessageBusDefine.UserCommandSetLookAtAngle();
        public MessageBusDefine.UserCommandSetLookAtSpace UserCommandSetLookAtSpace { get; } = new MessageBusDefine.UserCommandSetLookAtSpace();
        public MessageBusDefine.UserCommandSetLookAtDistance UserCommandSetLookAtDistance { get; } = new MessageBusDefine.UserCommandSetLookAtDistance();
    }
}
