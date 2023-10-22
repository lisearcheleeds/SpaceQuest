using UnityEngine;

namespace VariableInventorySystem
{
    public class InventoryData
    {
        public ICellData[] CellData { get; }

        public int CapacityWidth { get; }
        public int CapacityHeight { get; }

        bool[] mask;

        public InventoryData(int capacityWidth, int capacityHeight)
            : this(new ICellData[capacityWidth * capacityHeight], capacityWidth, capacityHeight)
        {
        }

        public InventoryData(ICellData[] cellData, int capacityWidth, int capacityHeight)
        {
            Debug.Assert(cellData.Length == capacityWidth * capacityHeight);

            CellData = cellData;
            CapacityWidth = capacityWidth;
            CapacityHeight = capacityHeight;

            UpdateMask();
        }

        public virtual int? GetId(ICellData cellData)
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

        public virtual int? GetInsertableId(ICellData cellData)
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

        public virtual void InsertInventoryItem(int id, ICellData cellData)
        {
            CellData[id] = cellData;
            UpdateMask();
        }

        public virtual void RemoveInventoryItem(int id)
        {
            CellData[id] = null;
            UpdateMask();
        }

        public virtual bool CheckInsert(int id, ICellData cellData)
        {
            if (id < 0)
            {
                return false;
            }

            var (widthCount, heightCount) = GridLayoutHelper.GetRotateDataSize(cellData);

            // check width
            if ((id % CapacityWidth) + (widthCount - 1) >= CapacityWidth)
            {
                return false;
            }

            // check height
            if (id + ((heightCount - 1) * CapacityWidth) >= CellData.Length)
            {
                return false;
            }

            for (var i = 0; i < widthCount; i++)
            {
                for (var t = 0; t < heightCount; t++)
                {
                    if (mask[id + i + (t * CapacityWidth)])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected virtual void UpdateMask()
        {
            mask = new bool[CapacityWidth * CapacityHeight];

            for (var i = 0; i < CellData.Length; i++)
            {
                if (CellData[i] == null || mask[i])
                {
                    continue;
                }

                var (widthCount, heightCount) = GridLayoutHelper.GetRotateDataSize(CellData[i]);
                for (var w = 0; w < widthCount; w++)
                {
                    for (var h = 0; h < heightCount; h++)
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
    }
}
