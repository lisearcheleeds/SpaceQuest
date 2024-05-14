namespace AloneSpace
{
    public partial class MessageBus
    {
        public UserInputMessage UserInput { get; } = new UserInputMessage();

        public class UserInputMessage
        {
            // UI
            public MessageBusDefineUserInput.UserInputOpenMenu UserInputOpenMenu { get; } = new MessageBusDefineUserInput.UserInputOpenMenu();
            public MessageBusDefineUserInput.UserInputCloseMenu UserInputCloseMenu { get; } = new MessageBusDefineUserInput.UserInputCloseMenu();
            public MessageBusDefineUserInput.UserInputSwitchMenuStatusView UserInputSwitchMenuStatusView { get; } = new MessageBusDefineUserInput.UserInputSwitchMenuStatusView();
            public MessageBusDefineUserInput.UserInputSwitchMenuInventoryView UserInputSwitchMenuInventoryView { get; } = new MessageBusDefineUserInput.UserInputSwitchMenuInventoryView();
            public MessageBusDefineUserInput.UserInputSwitchMenuPlayerView UserInputSwitchMenuPlayerView { get; } = new MessageBusDefineUserInput.UserInputSwitchMenuPlayerView();
            public MessageBusDefineUserInput.UserInputOpenSpaceMapView UserInputOpenSpaceMapView { get; } = new MessageBusDefineUserInput.UserInputOpenSpaceMapView();
            public MessageBusDefineUserInput.UserInputCloseSpaceMapView UserInputCloseSpaceMapView { get; } = new MessageBusDefineUserInput.UserInputCloseSpaceMapView();

            public MessageBusDefineUserInput.UIMenuStatusViewSelectActorData UIMenuStatusViewSelectActorData { get; } = new MessageBusDefineUserInput.UIMenuStatusViewSelectActorData();

            public MessageBusDefineUserInput.UserInputExecuteWeapon UserInputSetExecuteWeapon { get; } = new MessageBusDefineUserInput.UserInputExecuteWeapon();
            public MessageBusDefineUserInput.UserInputReloadWeapon UserInputReloadWeapon { get; } = new MessageBusDefineUserInput.UserInputReloadWeapon();
            public MessageBusDefineUserInput.UserInputSetCurrentWeaponGroupIndex UserInputSetCurrentWeaponGroupIndex { get; } = new MessageBusDefineUserInput.UserInputSetCurrentWeaponGroupIndex();

            public MessageBusDefineUserInput.UserInputFrontBoosterPower UserInputForwardBoosterPowerRatio { get; } = new MessageBusDefineUserInput.UserInputFrontBoosterPower();
            public MessageBusDefineUserInput.UserInputBackBoosterPower UserInputBackBoosterPowerRatio { get; } = new MessageBusDefineUserInput.UserInputBackBoosterPower();
            public MessageBusDefineUserInput.UserInputRightBoosterPower UserInputRightBoosterPowerRatio { get; } = new MessageBusDefineUserInput.UserInputRightBoosterPower();
            public MessageBusDefineUserInput.UserInputLeftBoosterPower UserInputLeftBoosterPowerRatio { get; } = new MessageBusDefineUserInput.UserInputLeftBoosterPower();
            public MessageBusDefineUserInput.UserInputTopBoosterPower UserInputTopBoosterPowerRatio { get; } = new MessageBusDefineUserInput.UserInputTopBoosterPower();
            public MessageBusDefineUserInput.UserInputBottomBoosterPower UserInputBottomBoosterPowerRatio { get; } = new MessageBusDefineUserInput.UserInputBottomBoosterPower();
            public MessageBusDefineUserInput.UserInputPitchBoosterPower UserInputPitchBoosterPowerRatio { get; } = new MessageBusDefineUserInput.UserInputPitchBoosterPower();
            public MessageBusDefineUserInput.UserInputRollBoosterPower UserInputRollBoosterPowerRatio { get; } = new MessageBusDefineUserInput.UserInputRollBoosterPower();
            public MessageBusDefineUserInput.UserInputYawBoosterPower UserInputYawBoosterPowerRatio { get; } = new MessageBusDefineUserInput.UserInputYawBoosterPower();

            public MessageBusDefineUserInput.UserCommandSetActorOperationMode UserCommandSetActorOperationMode { get; } = new MessageBusDefineUserInput.UserCommandSetActorOperationMode();

            public MessageBusDefineUserInput.UserInputOpenContentQuickView UserInputOpenContentQuickView { get; } = new MessageBusDefineUserInput.UserInputOpenContentQuickView();
            public MessageBusDefineUserInput.UserInputCloseContentQuickView UserInputCloseContentQuickView { get; } = new MessageBusDefineUserInput.UserInputCloseContentQuickView();

            public MessageBusDefineUserInput.UserCommandSetCameraTrackTarget UserCommandSetCameraTrackTarget { get; } = new MessageBusDefineUserInput.UserCommandSetCameraTrackTarget();

            public MessageBusDefineUserInput.UserCommandSetLookAtAngle UserCommandSetLookAtAngle { get; } = new MessageBusDefineUserInput.UserCommandSetLookAtAngle();
            public MessageBusDefineUserInput.UserCommandSetLookAtSpace UserCommandSetLookAtSpace { get; } = new MessageBusDefineUserInput.UserCommandSetLookAtSpace();
            public MessageBusDefineUserInput.UserCommandSetLookAtDistance UserCommandSetLookAtDistance { get; } = new MessageBusDefineUserInput.UserCommandSetLookAtDistance();
            public MessageBusDefineUserInput.UserCommandSetSpaceMapLookAtAngle UserCommandSetSpaceMapLookAtAngle { get; } = new MessageBusDefineUserInput.UserCommandSetSpaceMapLookAtAngle();
            public MessageBusDefineUserInput.UserCommandSetSpaceMapLookAtDistance UserCommandSetSpaceMapLookAtDistance { get; } = new MessageBusDefineUserInput.UserCommandSetSpaceMapLookAtDistance();
        }
    }
}
