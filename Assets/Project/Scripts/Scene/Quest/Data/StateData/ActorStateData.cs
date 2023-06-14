using System;
using System.Collections.Generic;
using AloneSpace;
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
        public bool IsUserControl { get; set; }

        public bool IsWarping { get; set; }

        public IInteractData InteractOrder { get; set; }
        public float CurrentInteractingTime { get; set; }

        public IPositionData MainTarget { get; set; }
        public IPositionData[] AroundTargets { get; set; } = Array.Empty<IPositionData>();

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

        public List<DamageEventData> CurrentDamageEventData { get; set; } = new List<DamageEventData>();
        public List<DamageEventData> HistoryDamageEventData { get; set; } = new List<DamageEventData>();

        public float EnduranceValue { get; set; }
    }
}
