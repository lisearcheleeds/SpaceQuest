﻿using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    /// <summary>
    /// ActorのStateデータ
    /// ActorDataとの棲み分け基準はゲームを再起動した時にリセットされるかどうか
    /// </summary>
    public class ActorStateData
    {
        public ActorAIState ActorAIState { get; set; }
        public bool EnableThink { get; set; }

        public bool IsWarping { get; set; }

        public List<InteractOrderState> InteractOrderStateList { get; set; } = new List<InteractOrderState>();

        public IPositionData MainTarget { get; set; }

        public float ForwardBoosterPowerRatio { get; set; }
        public float BackBoosterPowerRatio { get; set; }
        public float RightBoosterPowerRatio { get; set; }
        public float LeftBoosterPowerRatio { get; set; }
        public float TopBoosterPowerRatio { get; set; }
        public float BottomBoosterPowerRatio { get; set; }

        public float PitchBoosterPowerRatio { get; set; }
        public float RollBoosterPowerRatio { get; set; }
        public float YawBoosterPowerRatio { get; set; }

        public Vector3 LookAtDirection { get; set; } = Vector3.forward;
        public IPositionData MoveTarget { get; set; }

        public int CurrentWeaponGroupIndex { get; set; }

        public List<DamageEventData> CurrentDamageEventDataList { get; set; } = new List<DamageEventData>();
        public List<DamageEventHistoryData> DamageEventHistoryDataList { get; set; } = new List<DamageEventHistoryData>();

        public float EnduranceValue { get; set; }
        public float EnduranceValueMax { get; set; }

        public float ShieldValue { get; set; }
        public float ShieldValueMax { get; set; }

        public List<SpecialEffectData> SpecialEffectDataList = new List<SpecialEffectData>();
    }
}
