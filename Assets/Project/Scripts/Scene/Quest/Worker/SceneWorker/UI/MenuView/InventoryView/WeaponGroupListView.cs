using System;
using UnityEngine;
using FancyScrollView;

namespace AloneSpace
{
    public class WeaponGroupListView : FancyScrollRect<WeaponGroupListViewCell.CellData, WeaponGroupListViewCell.CellContext>
    {
        [SerializeField] GameObject cellPrefab;
        [SerializeField] float cellSize;

        protected override float CellSize => cellSize;
        protected override GameObject CellPrefab => cellPrefab;

        public void Apply(WeaponGroupListViewCell.CellData[] cellData)
        {
            UpdateContents(cellData);
        }
    }
}
