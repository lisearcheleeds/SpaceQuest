namespace AloneSpace
{
    public enum KeyBindKey
    {
        // 移動1
        Forward,
        Backward,
        Right,
        Left,
        Up,
        Down,

        // 移動2
        Deceleration,

        // 回転
        PitchPlus,
        PitchMinus,
        RollPlus,
        RollMinus,
        YawPlus,
        YawMinus,

        // 武器
        Trigger,
        Reload,
        WeaponGroup1,
        WeaponGroup2,
        WeaponGroup3,
        WeaponGroup4,
        WeaponGroup5,

        // UI
        Map,
        InteractList,
        Inventory,

        // System,
        MouseModeSwitch,

        ActorOperationModeSwitchObserve,
        ActorOperationModeSwitchCockpit,
        ActorOperationModeSwitchSpotter,
        ActorOperationModeSwitchSpotFreeCamera,
    }
}
