using System;
using System.Linq;
using AloneSpace;

namespace AloneSpace
{
    public class PlayerThinkModule : IThinkModule
    {
        PlayerQuestData playerQuestData;

        public Guid InstanceId => playerQuestData.InstanceId;

        public PlayerThinkModule(PlayerQuestData playerQuestData)
        {
            this.playerQuestData = playerQuestData;
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
            if (playerQuestData.PlayerStance == PlayerStance.Scavenger)
            {
                return;
            }

            if (!playerQuestData.MainActorData?.AreaId.HasValue ?? true)
            {
                return;
            }

            var areaActorData = MessageBus.Instance.UtilGetAreaActorData.Unicast(playerQuestData.MainActorData.AreaId.Value);

            var isExistScavenger = areaActorData.Any(x => MessageBus.Instance.UtilGetPlayerQuestData.Unicast(x.PlayerInstanceId).PlayerStance == PlayerStance.Scavenger);
            var isExistOtherPlayer = areaActorData.Any(x =>
            {
                var playerData = MessageBus.Instance.UtilGetPlayerQuestData.Unicast(x.PlayerInstanceId);
                return playerData.PlayerStance != PlayerStance.Scavenger && playerData.InstanceId != playerQuestData.InstanceId;
            });;

            switch (playerQuestData.PlayerStance)
            {
                case PlayerStance.None:
                case PlayerStance.Scavenger:
                    return;
                case PlayerStance.Fugitive:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Escape);
                    return;
                case PlayerStance.ScavengerKiller:
                    if (isExistScavenger && !isExistOtherPlayer)
                    {
                        MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Combat);
                    }
                    else
                    {
                        MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Survey);
                    }
                    return;
                case PlayerStance.Collector:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Survey);
                    return;
                case PlayerStance.Hunter:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Combat);
                    return;
                case PlayerStance.Stalker:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Combat);
                    return;
                case PlayerStance.ConquestUser:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Combat);
                    return;
                case PlayerStance.FacilityUser:
                    MessageBus.Instance.PlayerCommandSetTacticsType.Broadcast(playerQuestData.InstanceId, TacticsType.Combat);
                    return;
            }
        }

        void UpdateInteract(QuestData questData, PlayerQuestData playerQuestData)
        {
            if (playerQuestData.PlayerStance == PlayerStance.Scavenger)
            {
                // FIXME: ScavでもInteract出来るようにする
                return;
            }

            // var areaData = questData.StarSystemData.AreaData[playerQuestData.MainActorData.AreaId];
        }
    }
}
