using System;
using FancyScrollView;
using UnityEngine;

namespace AloneSpace
{
    public class InteractionListView : FancyScrollRect<InteractionListViewCell.CellData, InteractionListViewCell.CellContext>
    {
        [SerializeField] GameObject cellPrefab;
        [SerializeField] float cellSize;

        protected override float CellSize => cellSize;
        protected override GameObject CellPrefab => cellPrefab;

        public void Apply(
            InteractionListViewCell.CellData[] cellData,
            Action<InteractionListViewCell.CellData> onSelect,
            Action<InteractionListViewCell.CellData> onConfirm)
        {
            Context.OnSelect = onSelect;
            Context.OnConfirm = onConfirm;
            UpdateContents(cellData);
        }
    }
}
