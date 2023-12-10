using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace.UI
{
    public class ActorSpecialEffectList : MonoBehaviour
    {
        [SerializeField] ActorSpecialEffectListView actorSpecialEffectListView;

        ActorData actorData;
        int prevSpecialEffectCount;
        bool isDirty;

        ActorSpecialEffectListViewCell.CellData[] actorSpecialEffectListViewCellDataList;

        public void Initialize()
        {
            MessageBus.Instance.UIMenuStatusViewSelectActorData.AddListener(UIMenuStatusViewSelectActorData);
        }

        public void Finalize()
        {
            MessageBus.Instance.UIMenuStatusViewSelectActorData.RemoveListener(UIMenuStatusViewSelectActorData);
        }

        public void SetDirty()
        {
            isDirty = true;
        }

        public void OnUpdate()
        {
            var currentSpecialEffectCount = 0;
            if (actorData != null)
            {
                currentSpecialEffectCount = actorData.ActorPresetSpecialEffectSpecVOs.Length;
            }

            if (prevSpecialEffectCount != currentSpecialEffectCount)
            {
                isDirty = true;
            }

            if (isDirty)
            {
                isDirty = false;
                Refresh();
            }
        }

        void Refresh()
        {
            actorSpecialEffectListViewCellDataList = actorData?.ActorStateData.SpecialEffectDataList.Select(x => new ActorSpecialEffectListViewCell.CellData(x)).ToArray() ?? Array.Empty<ActorSpecialEffectListViewCell.CellData>();

            prevSpecialEffectCount = 0;
            actorSpecialEffectListView.Apply(actorSpecialEffectListViewCellDataList);
        }

        void UIMenuStatusViewSelectActorData(ActorData actorData)
        {
            this.actorData = actorData;
            SetDirty();
        }
    }
}
