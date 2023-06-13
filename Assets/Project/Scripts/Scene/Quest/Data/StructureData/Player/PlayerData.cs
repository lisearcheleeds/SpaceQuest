using System;
using AloneSpace;

namespace AloneSpace
{
    public class PlayerData : IThinkModuleHolder
    {
        public Guid InstanceId { get; }
        public IThinkModule ThinkModule { get; private set; }

        public PlayerStance PlayerStance { get; private set; }

        public TacticsType TacticsType { get; private set; }

        public ActorData MainActorData { get; private set; }

        public PlayerData()
        {
            InstanceId = Guid.NewGuid();

            ActivateModules();
        }

        public void ActivateModules()
        {
            ThinkModule = new PlayerThinkModule(this);
            ThinkModule.ActivateModule();
        }

        public void DeactivateModules()
        {
            ThinkModule.DeactivateModule();
            ThinkModule = null;
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
