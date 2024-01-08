using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public static class MessageBusDefineData
    {
        public class CreatePlayerDataFromPresetIdAndAreaIdRandomPosition : MessageBusBroadcaster<int, Dictionary<PlayerPropertyKey, IPlayerPropertyValue>, int>{}
        public class CreatePlayerDataFromPresetIdAndAreaId : MessageBusBroadcaster<int, Dictionary<PlayerPropertyKey, IPlayerPropertyValue>, int, Vector3>{}
        public class CreatePlayerDataFromPresetId : MessageBusBroadcaster<int, Dictionary<PlayerPropertyKey, IPlayerPropertyValue>, AreaData, Vector3>{}
        public class CreatePlayerDataFromPreset : MessageBusBroadcaster<PlayerPresetVO, Dictionary<PlayerPropertyKey, IPlayerPropertyValue>, AreaData, Vector3>{}
        public class ReleasePlayerData : MessageBusBroadcaster<PlayerData>{}
        public class OnCreatePlayerData : MessageBusBroadcaster<PlayerData>{}
        public class OnReleasePlayerData : MessageBusBroadcaster<PlayerData>{}

        public class CreateActorDataFromPresetId : MessageBusBroadcaster<PlayerData, int, AreaData, Vector3>{}
        public class CreateActorDataFromPreset : MessageBusBroadcaster<PlayerData, ActorPresetVO, AreaData, Vector3>{}
        public class ReleaseActorData : MessageBusBroadcaster<ActorData>{}
        public class OnCreateActorData : MessageBusBroadcaster<ActorData>{}
        public class OnReleaseActorData : MessageBusBroadcaster<ActorData>{}

        // WeaponEffect
        public class CreateWeaponEffectData : MessageBusBroadcaster<IWeaponEffectSpecVO, IWeaponEffectCreateOptionData>{}
        public class ReleaseWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}
        public class OnCreateWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}
        public class OnReleaseWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}

        // GraphicEffect
        public class SpawnGraphicEffect : MessageBusBroadcaster<GraphicEffectSpecVO, IGraphicEffectHandler>{}

        // InteractData
        public class CreateAreaInteractData : MessageBusBroadcaster<AreaData, Vector3>{}
        public class CreateInventoryInteractData : MessageBusBroadcaster<InventoryData[], int, Vector3, Quaternion>{}
        public class CreateItemInteractData : MessageBusBroadcaster<ItemData, int, Vector3, Quaternion>{}
        public class ReleaseInteractData : MessageBusBroadcaster<IInteractData>{}
        public class OnCreateInteractData : MessageBusBroadcaster<IInteractData>{}
        public class OnReleaseInteractData : MessageBusBroadcaster<IInteractData>{}
    }
}
