namespace AloneSpace
{
    public enum KeyBindKey
    {
        // FighterModeの入力キー
        FighterModeForward,
        FighterModeBackward,
        FighterModeYawPlus,
        FighterModeYawMinus,
        
        // AttackerModeの入力キー
        AttackerModeVerticalPlus,
        AttackerModeVerticalMinus,
        AttackerModeHorizontalPlus,
        AttackerModeHorizontalMinus,
        
        // AimModeの入力キー
        AimModeForward,
        AimModeBackward,
        AimModeRight,
        AimModeLeft,
        AimModeUp,
        AimModeDown,

        // 武器
        Trigger,
        Reload,
        WeaponGroup1,
        WeaponGroup2,
        WeaponGroup3,
        WeaponGroup4,
        WeaponGroup5,

        // UI
        Menu,
        MenuStatusView,
        MenuInventoryView,
        MenuPlayerView,
        SpaceMapView,

        // System,
        MouseModeSwitch,

        ActorOperationModeSwitchObserverMode,
        ActorOperationModeSwitchFighterMode,
        ActorOperationModeSwitchAttackerMode,
        
        LockOn,
        FreeCamera,

        Escape,
    }
}
