using System;
using RoboQuest;
using VariableInventorySystem;

namespace AloneSpace
{
    public class ItemData : IVariableInventoryCellData
    {
        public int Id => ItemVO.Id;
        public Guid InstanceId { get; }
        
        public bool IsRotate { get; set; }
        public ItemVO ItemVO { get; }

        public bool HasAmount => ItemVO.MaxAmount.HasValue;
        public int? Amount { get; set; }

        int IVariableInventoryCellData.Width => ItemVO.Width;
        int IVariableInventoryCellData.Height => ItemVO.Height;
        IVariableInventoryAsset IVariableInventoryCellData.ImageAsset => ItemVO.ImageAsset;

        public ItemData(ItemVO itemVO, int? amount)
        {
            ItemVO = itemVO;
            Amount = amount;
            
            InstanceId = Guid.NewGuid();
        }
    }
}