using System;
using UnityEngine;

namespace AloneSpace
{
    public partial class MessageBusDefine
    {
        public class CreatePlayerDataFromPresetIdAndAreaIdRandomPosition : MessageBusBroadcaster<int, int>{}
        public class CreatePlayerDataFromPresetIdAndAreaId : MessageBusBroadcaster<int, int, Vector3>{}
        public class CreatePlayerDataFromPresetId : MessageBusBroadcaster<int, AreaData, Vector3>{}
        public class CreatePlayerDataFromPreset : MessageBusBroadcaster<PlayerPresetVO, AreaData, Vector3>{}
        public class ReleasePlayerData : MessageBusBroadcaster<PlayerQuestData>{}
        public class AddedPlayerData : MessageBusBroadcaster<PlayerQuestData>{}
        public class RemovedPlayerData : MessageBusBroadcaster<PlayerQuestData>{}

        public class CreateActorDataFromPresetId : MessageBusBroadcaster<PlayerQuestData, int, AreaData, Vector3>{}
        public class CreateActorDataFromPreset : MessageBusBroadcaster<PlayerQuestData, ActorPresetVO, AreaData, Vector3>{}
        public class ReleaseActorData : MessageBusBroadcaster<ActorData>{}
        public class AddedActorData : MessageBusBroadcaster<ActorData>{}
        public class RemovedActorData : MessageBusBroadcaster<ActorData>{}

        // WeaponEffect
        public class CreateWeaponEffectData : MessageBusBroadcaster<IWeaponEffectSpecVO, WeaponData, IPositionData, Quaternion, IPositionData>{}
        public class ReleaseWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}
        public class AddedWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}
        public class RemovedWeaponEffectData : MessageBusBroadcaster<WeaponEffectData>{}

        // GraphicEffect
        public class SpawnGraphicEffect : MessageBusBroadcaster<GraphicEffectSpecVO, IGraphicEffectHandler>{}

    }
}
