namespace AloneSpace
{
    public partial class MessageBus
    {
        // Player
        public MessageBusDefine.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition CreatePlayerDataFromPresetIdAndAreaIdRandomPosition { get; } = new MessageBusDefine.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition();
        public MessageBusDefine.CreatePlayerDataFromPresetIdAndAreaId CreatePlayerDataFromPresetIdAndAreaId { get; } = new MessageBusDefine.CreatePlayerDataFromPresetIdAndAreaId();
        public MessageBusDefine.CreatePlayerDataFromPresetId CreatePlayerDataFromPresetId { get; } = new MessageBusDefine.CreatePlayerDataFromPresetId();
        public MessageBusDefine.CreatePlayerDataFromPreset CreatePlayerDataFromPreset { get; } = new MessageBusDefine.CreatePlayerDataFromPreset();
        public MessageBusDefine.ReleasePlayerData ReleasePlayerData { get; } = new MessageBusDefine.ReleasePlayerData();
        public MessageBusDefine.CreatedPlayerData CreatedPlayerData { get; } = new MessageBusDefine.CreatedPlayerData();
        public MessageBusDefine.ReleasedPlayerData ReleasedPlayerData { get; } = new MessageBusDefine.ReleasedPlayerData();

        // Actor
        public MessageBusDefine.CreateActorDataFromPresetId CreateActorDataFromPresetId { get; } = new MessageBusDefine.CreateActorDataFromPresetId();
        public MessageBusDefine.CreateActorDataFromPreset CreateActorDataFromPreset { get; } = new MessageBusDefine.CreateActorDataFromPreset();
        public MessageBusDefine.ReleaseActorData ReleaseActorData { get; } = new MessageBusDefine.ReleaseActorData();
        public MessageBusDefine.CreatedActorData CreatedActorData { get; } = new MessageBusDefine.CreatedActorData();
        public MessageBusDefine.ReleasedActorData ReleasedActorData { get; } = new MessageBusDefine.ReleasedActorData();

        // WeaponEffect
        public MessageBusDefine.CreateWeaponEffectData CreateWeaponEffectData { get; } = new MessageBusDefine.CreateWeaponEffectData();
        public MessageBusDefine.ReleaseWeaponEffectData ReleaseWeaponEffectData { get; } = new MessageBusDefine.ReleaseWeaponEffectData();
        public MessageBusDefine.CreatedWeaponEffectData CreatedWeaponEffectData { get; } = new MessageBusDefine.CreatedWeaponEffectData();
        public MessageBusDefine.ReleasedWeaponEffectData ReleasedWeaponEffectData { get; } = new MessageBusDefine.ReleasedWeaponEffectData();

        // GraphicEffect
        public MessageBusDefine.SpawnGraphicEffect SpawnGraphicEffect { get; } = new MessageBusDefine.SpawnGraphicEffect();

        // InteractData
        public MessageBusDefine.CreateAreaInteractData CreateAreaInteractData { get; } = new MessageBusDefine.CreateAreaInteractData();
        public MessageBusDefine.CreateInventoryInteractData CreateInventoryInteractData { get; } = new MessageBusDefine.CreateInventoryInteractData();
        public MessageBusDefine.CreateItemInteractData CreateItemInteractData { get; } = new MessageBusDefine.CreateItemInteractData();
        public MessageBusDefine.ReleaseInteractData ReleaseInteractData { get; } = new MessageBusDefine.ReleaseInteractData();
        public MessageBusDefine.CreatedInteractData CreatedInteractData { get; } = new MessageBusDefine.CreatedInteractData();
        public MessageBusDefine.ReleasedInteractData ReleasedInteractData { get; } = new MessageBusDefine.ReleasedInteractData();
    }
}
