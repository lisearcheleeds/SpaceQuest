using System;
using AloneSpace;
using VariableInventorySystem;

namespace AloneSpace
{
    public class ItemData : StandardGridCellData
    {
        public int Id => ItemVO.Id;
        public Guid InstanceId { get; }

        public ItemVO ItemVO { get; }

        public bool HasAmount => ItemVO.MaxAmount.HasValue;
        public int? Amount { get; set; }

        public Texture2DPathVO ImageAsset => ItemVO.ImageAsset;

        public override bool IsRotate { get; set; }
        public override int WidthCount => ItemVO.Width;
        public override int HeightCount => ItemVO.Height;
        public override string ImagePath => ItemVO.ImageAsset.Path;

        public ItemData(ItemVO itemVO, int? amount)
        {
            ItemVO = itemVO;
            Amount = amount;

            InstanceId = Guid.NewGuid();
        }
    }
}
