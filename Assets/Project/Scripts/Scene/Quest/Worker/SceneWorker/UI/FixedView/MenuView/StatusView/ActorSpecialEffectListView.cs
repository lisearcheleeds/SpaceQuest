using System;
using UnityEngine;
using FancyScrollView;

namespace AloneSpace.UI
{
    public class ActorSpecialEffectListView : FancyScrollRect<ActorSpecialEffectListViewCell.CellData, ActorSpecialEffectListViewCell.CellContext>
    {
        [SerializeField] GameObject cellPrefab;
        [SerializeField] float cellSize;

        protected override float CellSize => cellSize;
        protected override GameObject CellPrefab => cellPrefab;

        public void Apply(ActorSpecialEffectListViewCell.CellData[] cellData)
        {
            UpdateContents(cellData);
        }
    }
}
