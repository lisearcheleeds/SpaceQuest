using System;
using AloneSpace;

namespace AloneSpace
{
    public class PlayerQuestData
    {
        public Guid InstanceId { get; }
        
        public PlayerStance PlayerStance { get; }

        public TacticsType TacticsType { get; private set; }

        public int? ExitAreaIndex { get; }
        
        public IPosition MoveTarget { get; private set; }

        public ActorData MainActorData { get; private set; }

        public PlayerQuestData(int? exitAreaIndex, PlayerStance playerStance)
        {
            InstanceId = Guid.NewGuid();

            PlayerStance = playerStance;
            ExitAreaIndex = exitAreaIndex;
        }
        
        public void SetTacticsType(TacticsType tacticsType)
        {
            TacticsType = tacticsType;
        }

        public void SetMoveTarget(IPosition moveTarget)
        {
            MoveTarget = moveTarget;
        }

        public void SetMainActorData(ActorData actorData)
        {
            MainActorData = actorData;
        }
    }
}
