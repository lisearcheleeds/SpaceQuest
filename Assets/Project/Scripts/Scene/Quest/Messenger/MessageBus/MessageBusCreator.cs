namespace AloneSpace
{
    public partial class MessageBus
    {
        public CreatorMessage Creator { get; } = new CreatorMessage();

        public class CreatorMessage
        {
            // Player
            public MessageBusDefineCreator.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition CreatePlayerDataFromPresetIdAndAreaIdRandomPosition { get; } = new MessageBusDefineCreator.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition();
            public MessageBusDefineCreator.CreatePlayerDataFromPresetIdAndAreaId CreatePlayerDataFromPresetIdAndAreaId { get; } = new MessageBusDefineCreator.CreatePlayerDataFromPresetIdAndAreaId();
            public MessageBusDefineCreator.CreatePlayerDataFromPresetId CreatePlayerDataFromPresetId { get; } = new MessageBusDefineCreator.CreatePlayerDataFromPresetId();
            public MessageBusDefineCreator.CreatePlayerDataFromPreset CreatePlayerDataFromPreset { get; } = new MessageBusDefineCreator.CreatePlayerDataFromPreset();
            public MessageBusDefineCreator.ReleasePlayerData ReleasePlayerData { get; } = new MessageBusDefineCreator.ReleasePlayerData();
            public MessageBusDefineCreator.OnCreatePlayerData OnCreatePlayerData { get; } = new MessageBusDefineCreator.OnCreatePlayerData();
            public MessageBusDefineCreator.OnReleasePlayerData OnReleasePlayerData { get; } = new MessageBusDefineCreator.OnReleasePlayerData();

            // Actor
            public MessageBusDefineCreator.CreateActorDataFromPresetId CreateActorDataFromPresetId { get; } = new MessageBusDefineCreator.CreateActorDataFromPresetId();
            public MessageBusDefineCreator.CreateActorDataFromPreset CreateActorDataFromPreset { get; } = new MessageBusDefineCreator.CreateActorDataFromPreset();
            public MessageBusDefineCreator.ReleaseActorData ReleaseActorData { get; } = new MessageBusDefineCreator.ReleaseActorData();
            public MessageBusDefineCreator.OnCreateActorData OnCreateActorData { get; } = new MessageBusDefineCreator.OnCreateActorData();
            public MessageBusDefineCreator.OnReleaseActorData OnReleaseActorData { get; } = new MessageBusDefineCreator.OnReleaseActorData();

            // WeaponEffect
            public MessageBusDefineCreator.CreateWeaponEffectData CreateWeaponEffectData { get; } = new MessageBusDefineCreator.CreateWeaponEffectData();
            public MessageBusDefineCreator.ReleaseWeaponEffectData ReleaseWeaponEffectData { get; } = new MessageBusDefineCreator.ReleaseWeaponEffectData();
            public MessageBusDefineCreator.OnCreateWeaponEffectData OnCreateWeaponEffectData { get; } = new MessageBusDefineCreator.OnCreateWeaponEffectData();
            public MessageBusDefineCreator.OnReleaseWeaponEffectData OnReleaseWeaponEffectData { get; } = new MessageBusDefineCreator.OnReleaseWeaponEffectData();

            // GraphicEffect
            public MessageBusDefineCreator.SpawnGraphicEffect SpawnGraphicEffect { get; } = new MessageBusDefineCreator.SpawnGraphicEffect();

            // InteractData
            public MessageBusDefineCreator.CreateAreaInteractData CreateAreaInteractData { get; } = new MessageBusDefineCreator.CreateAreaInteractData();
            public MessageBusDefineCreator.CreateInventoryInteractData CreateInventoryInteractData { get; } = new MessageBusDefineCreator.CreateInventoryInteractData();
            public MessageBusDefineCreator.CreateItemInteractData CreateItemInteractData { get; } = new MessageBusDefineCreator.CreateItemInteractData();
            public MessageBusDefineCreator.ReleaseInteractData ReleaseInteractData { get; } = new MessageBusDefineCreator.ReleaseInteractData();
            public MessageBusDefineCreator.OnCreateInteractData OnCreateInteractData { get; } = new MessageBusDefineCreator.OnCreateInteractData();
            public MessageBusDefineCreator.OnReleaseInteractData OnReleaseInteractData { get; } = new MessageBusDefineCreator.OnReleaseInteractData();
        }
    }
}
