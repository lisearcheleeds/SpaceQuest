using System;

namespace AloneSpace
{
    public class PlayerData : IReleasableData, IThinkModuleHolder
    {
        public Guid InstanceId { get; }
        public IThinkModule ThinkModule { get; private set; }

        public bool IsReleased { get; private set; }

        public PlayerStance PlayerStance { get; private set; }

        public TacticsType TacticsType { get; private set; }

        public ActorData MainActorData { get; private set; }

        public PlayerData()
        {
            InstanceId = Guid.NewGuid();

            ThinkModule = new PlayerThinkModule(this);
        }

        public void ActivateModules()
        {
            ThinkModule.ActivateModule();
        }

        public void DeactivateModules()
        {
            ThinkModule.DeactivateModule();

            // NOTE: 別にnull入れなくても良いがIsReleased見ずにModule見ようとしたらコケてくれるので
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

        public void Release()
        {
            IsReleased = true;
        }
    }
}
