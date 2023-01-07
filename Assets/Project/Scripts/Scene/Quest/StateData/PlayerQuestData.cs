using System;
using AloneSpace;

namespace AloneSpace
{
    public class PlayerQuestData
    {
        public Guid InstanceId { get; }
        
        public PlayerStance PlayerStance { get; private set; }

        public TacticsType TacticsType { get; private set; }

        public ActorData MainActorData { get; private set; }

        public PlayerQuestData()
        {
            InstanceId = Guid.NewGuid();
        }
        
        public void SetPlayerStance(PlayerStance playerStance)
        {
            PlayerStance = playerStance;
        }
        
        public void SetTacticsType(TacticsType tacticsType)
        {
            TacticsType = tacticsType;
        }

        public void SetMainActorData(ActorData actorData)
        {
            MainActorData = actorData;
        }
    }
}
