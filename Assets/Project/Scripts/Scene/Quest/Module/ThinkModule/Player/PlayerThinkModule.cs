using System;
using System.Linq;
using AloneSpace;

namespace AloneSpace
{
    public class PlayerThinkModule : IThinkModule
    {
        PlayerData playerData;

        public Guid InstanceId => playerData.InstanceId;

        public PlayerThinkModule(PlayerData playerData)
        {
            this.playerData = playerData;
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
            if (playerData.PlayerStance == PlayerStance.Scavenger)
            {
                return;
            }

            if (!playerData.ActorDataList.Any())
            {
                return;
            }


            switch (playerData.PlayerStance)
            {
                case PlayerStance.None:
                case PlayerStance.Scavenger:
                    return;
                case PlayerStance.Fugitive:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerData.InstanceId, TacticsType.Escape);
                    return;
                case PlayerStance.ScavengerKiller:
                    /*
                    if (isExistScavenger && !isExistOtherPlayer)
                    {
                        MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerData.InstanceId, TacticsType.Combat);
                    }
                    else
                    {
                        MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerData.InstanceId, TacticsType.Survey);
                    }
                    */
                    return;
                case PlayerStance.Collector:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerData.InstanceId, TacticsType.Survey);
                    return;
                case PlayerStance.Hunter:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerData.InstanceId, TacticsType.Combat);
                    return;
                case PlayerStance.Stalker:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerData.InstanceId, TacticsType.Combat);
                    return;
                case PlayerStance.ConquestUser:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerData.InstanceId, TacticsType.Combat);
                    return;
                case PlayerStance.FacilityUser:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerData.InstanceId, TacticsType.Combat);
                    return;
            }
        }

        void UpdateInteract(QuestData questData, PlayerData playerData)
        {
            if (playerData.PlayerStance == PlayerStance.Scavenger)
            {
                // FIXME: ScavでもInteract出来るようにする
                return;
            }

            // var areaData = questData.StarSystemData.AreaData[playerQuestData.MainActorData.AreaId];
        }
    }
}
