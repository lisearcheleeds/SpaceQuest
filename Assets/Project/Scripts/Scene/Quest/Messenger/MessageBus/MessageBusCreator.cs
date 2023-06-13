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
        public MessageBusDefine.AddedPlayerData AddedPlayerData { get; } = new MessageBusDefine.AddedPlayerData();
        public MessageBusDefine.RemovedPlayerData RemovedPlayerData { get; } = new MessageBusDefine.RemovedPlayerData();

        // Actor
        public MessageBusDefine.CreateActorDataFromPresetId CreateActorDataFromPresetId { get; } = new MessageBusDefine.CreateActorDataFromPresetId();
        public MessageBusDefine.CreateActorDataFromPreset CreateActorDataFromPreset { get; } = new MessageBusDefine.CreateActorDataFromPreset();
        public MessageBusDefine.ReleaseActorData ReleaseActorData { get; } = new MessageBusDefine.ReleaseActorData();
        public MessageBusDefine.AddedActorData AddedActorData { get; } = new MessageBusDefine.AddedActorData();
        public MessageBusDefine.RemovedActorData RemovedActorData { get; } = new MessageBusDefine.RemovedActorData();

        // WeaponEffect
        public MessageBusDefine.CreateWeaponEffectData CreateWeaponEffectData { get; } = new MessageBusDefine.CreateWeaponEffectData();
        public MessageBusDefine.ReleaseWeaponEffectData ReleaseWeaponEffectData { get; } = new MessageBusDefine.ReleaseWeaponEffectData();
        public MessageBusDefine.AddedWeaponEffectData AddedWeaponEffectData { get; } = new MessageBusDefine.AddedWeaponEffectData();
        public MessageBusDefine.RemovedWeaponEffectData RemovedWeaponEffectData { get; } = new MessageBusDefine.RemovedWeaponEffectData();

        // GraphicEffect
        public MessageBusDefine.SpawnGraphicEffect SpawnGraphicEffect { get; } = new MessageBusDefine.SpawnGraphicEffect();
    }
}
