using System;
using AloneSpace;

namespace AloneSpace
{
    public class PlayerQuestData : IThinkModuleHolder
    {
        public Guid InstanceId { get; }
        public IThinkModule ThinkModule { get; }
        
        public PlayerStance PlayerStance { get; private set; }

        public TacticsType TacticsType { get; private set; }

        public ActorData MainActorData { get; private set; }

        public PlayerQuestData()
        {
            InstanceId = Guid.NewGuid();
            ThinkModule = new PlayerThinkModule(this);
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
