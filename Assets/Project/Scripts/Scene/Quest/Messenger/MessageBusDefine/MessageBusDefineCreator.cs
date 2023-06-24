using System;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public partial class MessageBusDefine
    {
        public class CreatePlayerDataFromPresetIdAndAreaIdRandomPosition : MessageBusBroadcaster<int, Dictionary<PlayerPropertyKey, IPlayerPropertyValue>, int>{}
        public class CreatePlayerDataFromPresetIdAndAreaId : MessageBusBroadcaster<int, Dictionary<PlayerPropertyKey, IPlayerPropertyValue>, int, Vector3>{}
        public class CreatePlayerDataFromPresetId : MessageBusBroadcaster<int, Dictionary<PlayerPropertyKey, IPlayerPropertyValue>, AreaData, Vector3>{}
        public class CreatePlayerDataFromPreset : MessageBusBroadcaster<PlayerPresetVO, Dictionary<PlayerPropertyKey, IPlayerPropertyValue>, AreaData, Vector3>{}
        public class ReleasePlayerData : MessageBusBroadcaster<PlayerData>{}
        public class CreatedPlayerData : MessageBusBroadcaster<PlayerData>{}
        public class ReleasedPlayerData : MessageBusBroadcaster<PlayerData>{}

        public class CreateActorDataFromPresetId : MessageBusBroadcaster<PlayerData, int, AreaData, Vector3>{}
        public class CreateActorDataFromPreset : MessageBusBroadcaster<PlayerData, ActorPresetVO, AreaData, Vector3>{}
        public class ReleaseActorData : MessageBusBroadcaster<ActorData>{}
        public class CreatedActorData : MessageBusBroadcaster<ActorData>{}
        public class ReleasedActorData : MessageBusBroadcaster<ActorData>{}

        // WeaponEffect
        public class CreateWeaponEffectData : MessageBusBroadcaster<IWeaponEffectSpecVO, IWeaponEffectCreateOptionData>{}
        public class ReleaseWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}
        public class CreatedWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}
        public class ReleasedWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}

        // GraphicEffect
        public class SpawnGraphicEffect : MessageBusBroadcaster<GraphicEffectSpecVO, IGraphicEffectHandler>{}

    }
}
