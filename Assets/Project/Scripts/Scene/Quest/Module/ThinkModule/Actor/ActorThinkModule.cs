using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class ActorThinkModule : IThinkModule
    {
        static readonly Dictionary<ActorAIState, IActorAIState> AIList = new Dictionary<ActorAIState, IActorAIState>()
        {
            { ActorAIState.None, null },
            { ActorAIState.Check, new ActorAICheck() },
            { ActorAIState.Sleep, new ActorAISleep() },
            { ActorAIState.Fight, new ActorAIFight() },
            { ActorAIState.Moving, new ActorAIMoving() },
        };

        ActorData actorData;

        public Guid InstanceId => actorData.InstanceId;

        public ActorThinkModule(ActorData actorData)
        {
            this.actorData = actorData;
            actorData.ActorStateData.ActorAIState = ActorAIState.Check;
        }

        public void ActivateModule()
        {
            MessageBus.Instance.RegisterThinkModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.UnRegisterThinkModule.Broadcast(this);
        }

        public void OnUpdateModule(float deltaTime)
        {
            if (actorData.ActorStateData.EnableThink)
            {
                // skip
                return;
            }

            var nextState = AIList[actorData.ActorStateData.ActorAIState]?.Update(actorData, deltaTime);
            if (nextState != null)
            {
                actorData.ActorStateData.ActorAIState = nextState.Value;
            }
        }
    }
}
