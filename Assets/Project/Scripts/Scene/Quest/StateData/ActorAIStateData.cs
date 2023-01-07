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

        public IPosition MoveTarget { get; set; }

        public IInteractData InteractOrder { get; set; }
        public float CurrentInteractingTime { get; set; }

        public List<IThreatData> ThreatList { get; } = new List<IThreatData>();
        public ITargetData[] AroundTargets { get; private set; } = Array.Empty<ITargetData>();

        public ITargetData MainTarget { get; set; }
    }
}