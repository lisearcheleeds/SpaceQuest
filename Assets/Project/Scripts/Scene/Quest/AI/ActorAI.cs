using System.Collections.Generic;
using AloneSpace;

namespace AloneSpace
{
    public static class ActorAI
    {
        static readonly Dictionary<ActorAIState, IActorAIState> AIList = new Dictionary<ActorAIState, IActorAIState>()
        {
            { ActorAIState.Check, new ActorAICheck() },
            { ActorAIState.Idle, new ActorAIIdle() },
            { ActorAIState.Fight, new ActorAIFight() },
            { ActorAIState.Interact, new ActorAIInteract() },
            { ActorAIState.Moving, new ActorAIMoving() },            
        };
        
        public static void Update(QuestData questData, ActorData actorData, float deltaTime)
        {
            var actorAICache = actorData.ActorAICache;

            if (!CheckCacheData(actorAICache))
            {
                // skip
                return;
            }

            if (actorAICache.ActorAIState != ActorAIState.None)
            {
                actorAICache.ActorAIState = AIList[actorAICache.ActorAIState].Update(questData, actorData, deltaTime);
            }

            ClearUsedCache(actorAICache);
        }

        static bool CheckCacheData(ActorAICache actorAICache)
        {
            if (actorAICache.MainTarget != null && !actorAICache.MainTarget.IsAlive)
            {
                actorAICache.MainTarget = null;
            }

            return true;
        }

        static void ClearUsedCache(ActorAICache actorAICache)
        {
            actorAICache.ThreatList.Clear();
        }
    }
}
