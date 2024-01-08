namespace AloneSpace
{
    public partial class MessageBus
    {
        public DataMessage Data { get; } = new DataMessage();

        public class DataMessage
        {
            // Player
            public MessageBusDefineData.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition CreatePlayerDataFromPresetIdAndAreaIdRandomPosition { get; } = new MessageBusDefineData.CreatePlayerDataFromPresetIdAndAreaIdRandomPosition();
            public MessageBusDefineData.CreatePlayerDataFromPresetIdAndAreaId CreatePlayerDataFromPresetIdAndAreaId { get; } = new MessageBusDefineData.CreatePlayerDataFromPresetIdAndAreaId();
            public MessageBusDefineData.CreatePlayerDataFromPresetId CreatePlayerDataFromPresetId { get; } = new MessageBusDefineData.CreatePlayerDataFromPresetId();
            public MessageBusDefineData.CreatePlayerDataFromPreset CreatePlayerDataFromPreset { get; } = new MessageBusDefineData.CreatePlayerDataFromPreset();
            public MessageBusDefineData.ReleasePlayerData ReleasePlayerData { get; } = new MessageBusDefineData.ReleasePlayerData();
            public MessageBusDefineData.OnCreatePlayerData OnCreatePlayerData { get; } = new MessageBusDefineData.OnCreatePlayerData();
            public MessageBusDefineData.OnReleasePlayerData OnReleasePlayerData { get; } = new MessageBusDefineData.OnReleasePlayerData();

            // Actor
            public MessageBusDefineData.CreateActorDataFromPresetId CreateActorDataFromPresetId { get; } = new MessageBusDefineData.CreateActorDataFromPresetId();
            public MessageBusDefineData.CreateActorDataFromPreset CreateActorDataFromPreset { get; } = new MessageBusDefineData.CreateActorDataFromPreset();
            public MessageBusDefineData.ReleaseActorData ReleaseActorData { get; } = new MessageBusDefineData.ReleaseActorData();
            public MessageBusDefineData.OnCreateActorData OnCreateActorData { get; } = new MessageBusDefineData.OnCreateActorData();
            public MessageBusDefineData.OnReleaseActorData OnReleaseActorData { get; } = new MessageBusDefineData.OnReleaseActorData();

            // WeaponEffect
            public MessageBusDefineData.CreateWeaponEffectData CreateWeaponEffectData { get; } = new MessageBusDefineData.CreateWeaponEffectData();
            public MessageBusDefineData.ReleaseWeaponEffectData ReleaseWeaponEffectData { get; } = new MessageBusDefineData.ReleaseWeaponEffectData();
            public MessageBusDefineData.OnCreateWeaponEffectData OnCreateWeaponEffectData { get; } = new MessageBusDefineData.OnCreateWeaponEffectData();
            public MessageBusDefineData.OnReleaseWeaponEffectData OnReleaseWeaponEffectData { get; } = new MessageBusDefineData.OnReleaseWeaponEffectData();

            // GraphicEffect
            public MessageBusDefineData.SpawnGraphicEffect SpawnGraphicEffect { get; } = new MessageBusDefineData.SpawnGraphicEffect();

            // InteractData
            public MessageBusDefineData.CreateAreaInteractData CreateAreaInteractData { get; } = new MessageBusDefineData.CreateAreaInteractData();
            public MessageBusDefineData.CreateInventoryInteractData CreateInventoryInteractData { get; } = new MessageBusDefineData.CreateInventoryInteractData();
            public MessageBusDefineData.CreateItemInteractData CreateItemInteractData { get; } = new MessageBusDefineData.CreateItemInteractData();
            public MessageBusDefineData.ReleaseInteractData ReleaseInteractData { get; } = new MessageBusDefineData.ReleaseInteractData();
            public MessageBusDefineData.OnCreateInteractData OnCreateInteractData { get; } = new MessageBusDefineData.OnCreateInteractData();
            public MessageBusDefineData.OnReleaseInteractData OnReleaseInteractData { get; } = new MessageBusDefineData.OnReleaseInteractData();
        }
    }
}
