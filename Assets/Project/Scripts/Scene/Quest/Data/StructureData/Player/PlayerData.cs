﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AloneSpace
{
    public class PlayerData : IReleasableData, IThinkModuleHolder
    {
        public Guid InstanceId { get; }
        public IThinkModule ThinkModule { get; private set; }

        public bool IsReleased { get; private set; }

        public PlayerStance PlayerStance { get; private set; }

        public TacticsType TacticsType { get; private set; }

        public ReadOnlyCollection<ActorData> ActorDataList { get; private set; }

        public ReadOnlyDictionary<PlayerPropertyKey, IPlayerPropertyValue> PlayerProperty { get; }

        List<ActorData> actorDataList;
        Dictionary<PlayerPropertyKey, IPlayerPropertyValue> playerProperty;

        public PlayerData(Dictionary<PlayerPropertyKey, IPlayerPropertyValue> playerProperty)
        {
            InstanceId = Guid.NewGuid();

            ThinkModule = new PlayerThinkModule(this);

            actorDataList = new List<ActorData>();
            ActorDataList = new ReadOnlyCollection<ActorData>(actorDataList);

            this.playerProperty = playerProperty ?? new Dictionary<PlayerPropertyKey, IPlayerPropertyValue>();
            PlayerProperty = new ReadOnlyDictionary<PlayerPropertyKey, IPlayerPropertyValue>(this.playerProperty);
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

        public void Release()
        {
            IsReleased = true;
        }

        public void AddActorData(ActorData actorData)
        {
            actorDataList.Add(actorData);
        }

        public void RemoveActorData(ActorData actorData)
        {
            actorDataList.Remove(actorData);
        }

        public void AddPlayerProperty(PlayerPropertyKey key, IPlayerPropertyValue value)
        {
            playerProperty.Add(key, value);
        }

        public void RemovePlayerProperty(PlayerPropertyKey key)
        {
            playerProperty.Remove(key);
        }
    }
}
