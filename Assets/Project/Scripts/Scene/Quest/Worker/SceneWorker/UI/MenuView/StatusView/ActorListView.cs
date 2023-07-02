using System;
using UnityEngine;
using FancyScrollView;

namespace AloneSpace
{
    public class ActorListView : FancyScrollRect<ActorListViewCell.CellData, ActorListViewCell.CellContext>
    {
        [SerializeField] GameObject cellPrefab;
        [SerializeField] float cellSize;

        protected override float CellSize => cellSize;
        protected override GameObject CellPrefab => cellPrefab;

        public void Apply(
            ActorListViewCell.CellData[] cellData,
            Action<ActorListViewCell.CellData> onSelect)
        {
            Context.OnSelect = onSelect;
            UpdateContents(cellData);
        }
    }
}
