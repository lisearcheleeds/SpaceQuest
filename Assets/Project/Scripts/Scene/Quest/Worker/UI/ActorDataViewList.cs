using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using AloneSpace.InSide;

namespace AloneSpace
{
    public class ActorDataViewList : MonoBehaviour
    {
        [SerializeField] ActorDataView actorDataViewPrefab;
        [SerializeField] RectTransform actorStatusViewParent;
        
        List<ActorDataView> actorStatusViews = new List<ActorDataView>();
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.SubscribeUpdateActorList.AddListener(SubscribeUpdateActorList);
            MessageBus.Instance.UserCommandSetObservePlayer.AddListener(UserCommandSetObservePlayer);
            MessageBus.Instance.UserCommandSetObserveActor.AddListener(UserCommandSetObserveActor);
        }

        void SubscribeUpdateActorList(Actor[] actors)
        {
            UpdateActorList();
        }
        
        void UserCommandSetObservePlayer(Guid playerInstanceId)
        {
            UpdateActorList();
        }
        
        void UserCommandSetObserveActor(Guid actorInstanceId)
        {
            UpdateActorList();
        }

        void UpdateActorList()
        {
            var observePlayerActors = questData.ObservePlayerActors;

            foreach (var actorView in actorStatusViews)
            {
                actorView.gameObject.SetActive(false);
            }
            
            for (var i = 0; i < observePlayerActors.Length; i++)
            {
                // captcha
                var index = i;
                
                if (actorStatusViews.Count <= i)
                {
                    actorStatusViews.Add(Instantiate(actorDataViewPrefab, actorStatusViewParent, false));
                }

                actorStatusViews[i].gameObject.SetActive(true);
                actorStatusViews[i].IsSelect = questData.ObserveActor != null && actorStatusViews[i].ActorData?.InstanceId == questData.ObserveActor.InstanceId;
                actorStatusViews[i].Apply(observePlayerActors[i], () => OnClickActorStatusView(actorStatusViews[index]));
            }

            if (observePlayerActors.Length == 0)
            {
                // 選択されていない状態もしくは選択していたものが居なければ設定し直す
                OnClickActorStatusView(actorStatusViews.FirstOrDefault(x => x.gameObject.activeSelf));
            }
        }
        
        void OnClickActorStatusView(ActorDataView selectActorDataView)
        {
            MessageBus.Instance.UserCommandSetObserveActor.Broadcast(selectActorDataView.ActorData.InstanceId);
        }
    }
}