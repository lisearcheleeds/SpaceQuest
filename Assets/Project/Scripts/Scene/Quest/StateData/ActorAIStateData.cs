using System;
using System.Collections.Generic;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    /// <summary>
    /// AI用の一時データ
    /// </summary>
    public class ActorAIStateData
    {
        public ActorAIState ActorAIState { get; set; }

        public IInteractData InteractOrder { get; set; }
        public float CurrentInteractingTime { get; set; }

        public List<IThreatData> ThreatList { get; } = new List<IThreatData>();
        public ITargetData[] AroundTargets { get; private set; } = Array.Empty<ITargetData>();

        public ITargetData MainTarget { get; set; }

        public float ForwardBoosterPowerRatio { get; set; }
        public float BackBoosterPowerRatio { get; set; }
        public float RightBoosterPowerRatio { get; set; }
        public float LeftBoosterPowerRatio { get; set; }
        public float TopBoosterPowerRatio { get; set; }
        public float BottomBoosterPowerRatio { get; set; }
        
        public float PitchBoosterPowerRatio { get; set; }
        public float RollBoosterPowerRatio { get; set; }
        public float YawBoosterPowerRatio { get; set; }

        public bool IsRotateLookAt { get; set; }
    }
}