using UnityEngine;

namespace VariableInventorySystem
{
    public class VariableInventoryViewData
    {
        public IVariableInventoryCellData[] CellData { get; }

        public int CapacityWidth { get; }
        public int CapacityHeight { get; }
        
        bool[] mask;

        public VariableInventoryViewData(int capacityWidth, int capacityHeight)
            : this(new IVariableInventoryCellData[capacityWidth * capacityHeight], capacityWidth, capacityHeight)
        {
        }

        public VariableInventoryViewData(IVariableInventoryCellData[] cellData, int capacityWidth, int capacityHeight)
        {
            Debug.Assert(cellData.Length == capacityWidth * capacityHeight);

            CellData = cellData;
            CapacityWidth = capacityWidth;
            CapacityHeight = capacityHeight;

            UpdateMask();
        }

        public virtual int? GetId(IVariableInventoryCellData cellData)
        {
            for (var i = 0; i < CellData.Length; i++)
            {
                if (CellData[i] == cellData)
                {
                    return i;
                }
            }

            return null;
        }

        public virtual int? GetInsertableId(IVariableInventoryCellData cellData)
        {
            for (var i = 0; i < mask.Length; i++)
            {
                if (!mask[i] && CheckInsert(i, cellData))
                {
                    return i;
                }
            }

            return null;
        }

        public virtual void InsertInventoryItem(int id, IVariableInventoryCellData cellData)
        {
            CellData[id] = cellData;
            UpdateMask();
        }
        
        public virtual void RemoveInventoryItem(int id)
        {
            CellData[id] = null;
            UpdateMask();
        }

        public virtual bool CheckInsert(int id, IVariableInventoryCellData cellData)
        {
            if (id < 0)
            {
                return false;
            }

            var (width, height) = GetRotateSize(cellData);

            // check width
            if ((id % CapacityWidth) + (width - 1) >= CapacityWidth)
            {
                return false;
            }

            // check height
            if (id + ((height - 1) * CapacityWidth) >= CellData.Length)
            {
                return false;
            }

            for (var i = 0; i < width; i++)
            {
                for (var t = 0; t < height; t++)
                {
                    if (mask[id + i + (t * CapacityWidth)])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected void UpdateMask()
        {
            mask = new bool[CapacityWidth * CapacityHeight];

            for (var i = 0; i < CellData.Length; i++)
            {
                if (CellData[i] == null || mask[i])
                {
                    continue;
                }

                var width = CellData[i].IsRotate ? CellData[i].Height : CellData[i].Width;
                var height = CellData[i].IsRotate ? CellData[i].Width : CellData[i].Height;

                for (var w = 0; w < width; w++)
                {
                    for (var h = 0; h < height; h++)
                    {
                        var checkIndex = i + w + (h * CapacityWidth);
                        if (checkIndex < mask.Length)
                        {
                            mask[checkIndex] = true;
                        }
                    }
                }
            }
        }

        protected (int, int) GetRotateSize(IVariableInventoryCellData cell)
        {
            if (cell == null)
            {
                return (1, 1);
            }

            return (cell.IsRotate ? cell.Height : cell.Width, cell.IsRotate ? cell.Width : cell.Height);
        }
    }
}