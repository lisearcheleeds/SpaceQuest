using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RoboQuest.Quest
{
    public class MapData
    {
        public int MapSizeX => mapPresetVO.MapSizeX;
        public int MapSizeY => mapPresetVO.MapSizeY;
        public int MapSize => mapPresetVO.MapSize;

        public AreaData[] AreaData { get; }
        
        MapPresetVO mapPresetVO;

        public MapData(MapPresetVO mapPresetVO)
        {
            this.mapPresetVO = mapPresetVO;            
            AreaData = Enumerable.Range(0, MapSize)
                .Select(i => new AreaData(i, mapPresetVO.AreaAssetVOs[i], GetAdjacentIndexes(i)))
                .ToArray();
        }

        public AreaDirection? GetAreaDirection(int currentIndex, int nextIndex)
        {
            var row = currentIndex / MapSizeX;
            var isEvenNumberRow = row % 2 == 0;

            if (GetNextIndex(currentIndex, AreaDirection.Top, isEvenNumberRow) == nextIndex) return AreaDirection.Top;
            if (GetNextIndex(currentIndex, AreaDirection.TopLeft, isEvenNumberRow) == nextIndex) return AreaDirection.TopLeft;
            if (GetNextIndex(currentIndex, AreaDirection.TopRight, isEvenNumberRow) == nextIndex) return AreaDirection.TopRight;
            if (GetNextIndex(currentIndex, AreaDirection.BottomLeft, isEvenNumberRow) == nextIndex) return AreaDirection.BottomLeft;
            if (GetNextIndex(currentIndex, AreaDirection.BottomRight, isEvenNumberRow) == nextIndex) return AreaDirection.BottomRight;
            if (GetNextIndex(currentIndex, AreaDirection.Bottom, isEvenNumberRow) == nextIndex) return AreaDirection.Bottom;
            return null;
        }

        public RouteAreaData[] GetRouteAreaData(int fromAreaIndex, int? toAreaIndex)
        {
            var routeAreaData = new List<RouteAreaData>();
            var currentAreaIndex = fromAreaIndex;

            while (toAreaIndex.HasValue)
            {
                var (routeCell, next) = GetRouteCell(currentAreaIndex, toAreaIndex.Value);
                routeAreaData.Add(routeCell);

                if (!next.HasValue)
                {
                    break;
                }

                currentAreaIndex = next.Value;
            }

            return routeAreaData.ToArray();
        }

        (AreaDirection, int)[] GetAdjacentIndexes(int index)
        {
            return new[]
                {
                    (AreaDirection.Top, GetAdjacentIndex(index, AreaDirection.Top)),
                    (AreaDirection.TopLeft, GetAdjacentIndex(index, AreaDirection.TopLeft)),
                    (AreaDirection.TopRight, GetAdjacentIndex(index, AreaDirection.TopRight)),
                    (AreaDirection.BottomLeft, GetAdjacentIndex(index, AreaDirection.BottomLeft)),
                    (AreaDirection.BottomRight, GetAdjacentIndex(index, AreaDirection.BottomRight)),
                    (AreaDirection.Bottom, GetAdjacentIndex(index, AreaDirection.Bottom)),
                }
                .Where(x => x.Item2.HasValue)
                .Select(x => (x.Item1, x.Item2.Value))
                .ToArray();
        }

        int? GetAdjacentIndex(int index, AreaDirection areaDirection)
        {
            var row = index / MapSizeX;
            var isEvenNumberRow = row % 2 == 0;
            var adjacentIndex = (int?)GetNextIndex(index, areaDirection, isEvenNumberRow);

            // マップの外周判定
            // indexから各Directionの行数を予想して一致していなかったらマップ端として判定する
            var expectedRow = row;
            switch (areaDirection)
            {
                case AreaDirection.Top:
                    expectedRow += 2;
                    break;

                case AreaDirection.TopLeft:
                case AreaDirection.TopRight:
                    expectedRow++;
                    break;

                case AreaDirection.Bottom:
                    expectedRow -= 2;
                    break;

                case AreaDirection.BottomLeft:
                case AreaDirection.BottomRight:
                    expectedRow--;
                    break;
            }

            if (adjacentIndex < 0)
            {
                adjacentIndex = null;
            }

            return adjacentIndex / MapSizeX == expectedRow ? adjacentIndex : null;
        }

        (RouteAreaData RouteAreaData, int? NextIndex) GetRouteCell(int currentAreaIndex, int toAreaIndex)
        {
            var diffX = toAreaIndex % MapSizeX - currentAreaIndex % MapSizeX;
            var diffY = toAreaIndex / MapSizeX - currentAreaIndex / MapSizeX;

            // 偶数行かどうか
            var isEvenNumberRow = (currentAreaIndex / MapSizeX) % 2 == 0;

            // 基本縦にはまっすぐいけるので縦のラインを合わせるように動く
            if (diffX == 0)
            {
                if (diffY % 2 == 0)
                {
                    // 差が偶数行 真上か真下に移動
                    if (diffY != 0)
                    {
                        var direction = diffY > 0 ? AreaDirection.Top : AreaDirection.Bottom;
                        return (new RouteAreaData(currentAreaIndex, direction), GetNextIndex(currentAreaIndex, direction, isEvenNumberRow));
                    }
                    else
                    {
                        return (new RouteAreaData(currentAreaIndex, null), null);
                    }
                }
                else
                {
                    // 差が奇数行
                    if (isEvenNumberRow)
                    {
                        // 今の位置が偶数行の場合は左にずらす
                        var direction = diffY > 0 ? AreaDirection.TopRight : AreaDirection.BottomRight;
                        return (new RouteAreaData(currentAreaIndex, direction), GetNextIndex(currentAreaIndex, direction, isEvenNumberRow));
                    }
                    else
                    {
                        // 今の位置が奇数行の場合は右にずらす
                        var direction = diffY > 0 ? AreaDirection.TopLeft : AreaDirection.BottomLeft;
                        return (new RouteAreaData(currentAreaIndex, direction), GetNextIndex(currentAreaIndex, direction, isEvenNumberRow));
                    }
                }
            }

            if (diffY > 0 || currentAreaIndex < MapSizeX)
            {
                // 上方向に左右移動
                var direction = diffX > 0 ? AreaDirection.TopRight : AreaDirection.TopLeft;
                return (new RouteAreaData(currentAreaIndex, direction), GetNextIndex(currentAreaIndex, direction, isEvenNumberRow));
            }
            else
            {
                // 下方向に左右移動
                var direction = diffX > 0 ? AreaDirection.BottomRight : AreaDirection.BottomLeft;
                return (new RouteAreaData(currentAreaIndex, direction), GetNextIndex(currentAreaIndex, direction, isEvenNumberRow));
            }
        }

        int GetNextIndex(int index, AreaDirection areaDirection, bool isEvenNumberRow)
        {
            switch (areaDirection)
            {
                case AreaDirection.Top:
                    return index + MapSizeX + MapSizeX;

                case AreaDirection.TopRight:
                    return index + MapSizeX + (isEvenNumberRow ? 0 : 1);

                case AreaDirection.TopLeft:
                    return index + MapSizeX + (isEvenNumberRow ? -1 : 0);

                case AreaDirection.BottomRight:
                    return index - MapSizeX + (isEvenNumberRow ? 0 : 1);

                case AreaDirection.BottomLeft:
                    return index - MapSizeX + (isEvenNumberRow ? -1 : 0);

                case AreaDirection.Bottom:
                    return index - MapSizeX - MapSizeX;

                default:
                    throw new ArgumentException();
            }
        }
    }
}
