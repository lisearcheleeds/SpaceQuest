using System;
using AloneSpace;
using VariableInventorySystem;

namespace AloneSpace
{
    public class ItemData : ICellData, IContentQuickViewData
    {
        public int Id => ItemVO.Id;
        public Guid InstanceId { get; }

        public ItemVO ItemVO { get; }

        public bool HasAmount => ItemVO.MaxAmount.HasValue;
        public int? Amount { get; set; }

        public bool IsRotate { get; set; }
        public int GridCellDataSizeWidth => ItemVO.Width;
        public int GridCellDataSizeHeight => ItemVO.Height;

        public ItemData(ItemVO itemVO, int? amount)
        {
            ItemVO = itemVO;
            Amount = amount;

            InstanceId = Guid.NewGuid();
        }
    }
}
