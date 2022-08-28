using System;
using System.Collections.Generic;
using System.Linq;
using RoboQuest;
using UnityEngine;

namespace AloneSpace
{
    public class MapData
    {
        public int MapSizeX => mapPresetVO.MapSizeX;
        public int MapSizeY => mapPresetVO.MapSizeY;
        public int MapSizeZ => mapPresetVO.MapSizeZ;
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
            // XYZは横縦奥で、奥が奇数であれば半分ずつ座標をずらす
            var depth = currentIndex / (MapSizeX * MapSizeY);
            var isEvenNumberDepth = depth % 2 == 0;

            if (GetNextIndex(currentIndex, AreaDirection.Top, isEvenNumberDepth) == nextIndex) return AreaDirection.Top;
            if (GetNextIndex(currentIndex, AreaDirection.Bottom, isEvenNumberDepth) == nextIndex) return AreaDirection.Bottom;
            if (GetNextIndex(currentIndex, AreaDirection.Front, isEvenNumberDepth) == nextIndex) return AreaDirection.Front;
            if (GetNextIndex(currentIndex, AreaDirection.Back, isEvenNumberDepth) == nextIndex) return AreaDirection.Back;
            if (GetNextIndex(currentIndex, AreaDirection.Right, isEvenNumberDepth) == nextIndex) return AreaDirection.Right;
            if (GetNextIndex(currentIndex, AreaDirection.Left, isEvenNumberDepth) == nextIndex) return AreaDirection.Left;
            if (GetNextIndex(currentIndex, AreaDirection.TopFrontLeft, isEvenNumberDepth) == nextIndex) return AreaDirection.TopFrontLeft;
            if (GetNextIndex(currentIndex, AreaDirection.TopFrontRight, isEvenNumberDepth) == nextIndex) return AreaDirection.TopFrontRight;
            if (GetNextIndex(currentIndex, AreaDirection.TopBackLeft, isEvenNumberDepth) == nextIndex) return AreaDirection.TopBackLeft;
            if (GetNextIndex(currentIndex, AreaDirection.TopBackRight, isEvenNumberDepth) == nextIndex) return AreaDirection.TopBackRight;
            if (GetNextIndex(currentIndex, AreaDirection.BottomFrontLeft, isEvenNumberDepth) == nextIndex) return AreaDirection.BottomFrontLeft;
            if (GetNextIndex(currentIndex, AreaDirection.BottomFrontRight, isEvenNumberDepth) == nextIndex) return AreaDirection.BottomFrontRight;
            if (GetNextIndex(currentIndex, AreaDirection.BottomBackLeft, isEvenNumberDepth) == nextIndex) return AreaDirection.BottomBackLeft;
            if (GetNextIndex(currentIndex, AreaDirection.BottomBackRight, isEvenNumberDepth) == nextIndex) return AreaDirection.BottomBackRight;
            
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
                    (AreaDirection.Bottom, GetAdjacentIndex(index, AreaDirection.Bottom)), 
                    (AreaDirection.Front, GetAdjacentIndex(index, AreaDirection.Front)), 
                    (AreaDirection.Back, GetAdjacentIndex(index, AreaDirection.Back)), 
                    (AreaDirection.Right, GetAdjacentIndex(index, AreaDirection.Right)), 
                    (AreaDirection.Left, GetAdjacentIndex(index, AreaDirection.Left)), 
                    (AreaDirection.TopFrontLeft, GetAdjacentIndex(index, AreaDirection.TopFrontLeft)), 
                    (AreaDirection.TopFrontRight, GetAdjacentIndex(index, AreaDirection.TopFrontRight)), 
                    (AreaDirection.TopBackLeft, GetAdjacentIndex(index, AreaDirection.TopBackLeft)), 
                    (AreaDirection.TopBackRight, GetAdjacentIndex(index, AreaDirection.TopBackRight)), 
                    (AreaDirection.BottomFrontLeft, GetAdjacentIndex(index, AreaDirection.BottomFrontLeft)), 
                    (AreaDirection.BottomFrontRight, GetAdjacentIndex(index, AreaDirection.BottomFrontRight)), 
                    (AreaDirection.BottomBackLeft, GetAdjacentIndex(index, AreaDirection.BottomBackLeft)), 
                    (AreaDirection.BottomBackRight, GetAdjacentIndex(index, AreaDirection.BottomBackRight)), 
                }
                .Where(x => x.Item2.HasValue)
                .Select(x => (x.Item1, x.Item2.Value))
                .ToArray();
        }

        int? GetAdjacentIndex(int currentIndex, AreaDirection areaDirection)
        {
            // XYZは横縦奥で、奥が奇数であれば半分ずつ座標をずらす
            var depth = currentIndex / (MapSizeX * MapSizeY);
            var isEvenNumberDepth = depth % 2 == 0;
            var adjacentIndex = (int?)GetNextIndex(currentIndex, areaDirection, isEvenNumberDepth);

            // マップの外周判定
            // indexから各Directionの行数を予想して一致していなかったらマップ端として判定する
            var xPos = currentIndex % MapSizeX;
            var xyPos = currentIndex % (MapSizeX * MapSizeY);
            switch (areaDirection)
            {
                case AreaDirection.Top when MapSizeX * MapSizeY - MapSizeX < xyPos : return null;
                case AreaDirection.Bottom when xyPos - MapSizeX < 0 : return null;
                case AreaDirection.Front when depth == MapSizeZ : return null;
                case AreaDirection.Back when depth == 0 : return null;
                case AreaDirection.Right when xPos == MapSizeX : return null;
                case AreaDirection.Left when xPos == 0 : return null;
                case AreaDirection.TopFrontLeft when (isEvenNumberDepth && xPos == 0) || depth == MapSizeZ || MapSizeX * MapSizeY - MapSizeX < xyPos : return null;
                case AreaDirection.TopFrontRight when (!isEvenNumberDepth && xPos == MapSizeX) || depth == MapSizeZ || MapSizeX * MapSizeY - MapSizeX < xyPos : return null;
                case AreaDirection.TopBackLeft when (isEvenNumberDepth && xPos == 0) || depth == 0 || MapSizeX * MapSizeY - MapSizeX < xyPos : return null;
                case AreaDirection.TopBackRight when (!isEvenNumberDepth && xPos == MapSizeX) || depth == 0 || MapSizeX * MapSizeY - MapSizeX < xyPos : return null;
                case AreaDirection.BottomFrontLeft when (isEvenNumberDepth && xPos == 0) || depth == MapSizeZ || xyPos - MapSizeX < 0 : return null;
                case AreaDirection.BottomFrontRight when (!isEvenNumberDepth && xPos == MapSizeX) || depth == MapSizeZ || xyPos - MapSizeX < 0 : return null;
                case AreaDirection.BottomBackLeft when (isEvenNumberDepth && xPos == 0) || depth == 0 || xyPos - MapSizeX < 0 : return null;
                case AreaDirection.BottomBackRight when (!isEvenNumberDepth && xPos == MapSizeX) || depth == 0 || xyPos - MapSizeX < 0 : return null;
            }

            return adjacentIndex;
        }

        (RouteAreaData RouteAreaData, int? NextIndex) GetRouteCell(int currentAreaIndex, int toAreaIndex)
        {
            var diffX = toAreaIndex % MapSizeX - currentAreaIndex % MapSizeX;
            var diffY = toAreaIndex / MapSizeX - currentAreaIndex / MapSizeX;
            var diffZ = toAreaIndex / (MapSizeX * MapSizeY) - currentAreaIndex / (MapSizeX * MapSizeY);

            // 偶数行かどうか
            // XYZは横縦奥で、奥が奇数であれば半分ずつ座標をずらす
            var depth = currentAreaIndex / (MapSizeX * MapSizeY);
            var isEvenNumberDepth = depth % 2 == 0;

            AreaDirection areaDirection = default;

            // 基本縦横奥にはまっすぐいける
            if (diffX == 0 && diffY == 0 && diffZ == 0) return (new RouteAreaData(currentAreaIndex, null), null);

            if (diffX == 0 && diffY == 0 && diffZ % 2 == 0 && diffZ > 0) return GetTuple(AreaDirection.Front);
            if (diffX == 0 && diffY == 0 && diffZ % 2 == 0 && diffZ < 0) return GetTuple(AreaDirection.Back);
            if (diffX == 0 && diffY == 0 && diffZ % 2 != 0 && diffZ > 0) return GetTuple(AreaDirection.TopFrontRight);
            if (diffX == 0 && diffY == 0 && diffZ % 2 != 0 && diffZ < 0) return GetTuple(AreaDirection.TopBackRight);
            if (diffX == 0 && diffY > 0 && diffZ == 0) return GetTuple(AreaDirection.Top);
            if (diffX == 0 && diffY < 0 && diffZ == 0) return GetTuple(AreaDirection.Bottom);
            if (diffX == 0 && diffY > 0 && diffZ > 0) return GetTuple(AreaDirection.TopFrontRight);
            if (diffX == 0 && diffY > 0 && diffZ < 0) return GetTuple(AreaDirection.TopBackRight);
            if (diffX == 0 && diffY < 0 && diffZ > 0) return GetTuple(AreaDirection.BottomFrontRight);
            if (diffX == 0 && diffY < 0 && diffZ < 0) return GetTuple(AreaDirection.BottomBackRight);

            if (diffX > 0 && diffY == 0 && diffZ == 0) return GetTuple(AreaDirection.Right);
            if (diffX > 0 && diffY == 0 && diffZ > 0) return GetTuple(AreaDirection.TopFrontRight);
            if (diffX > 0 && diffY == 0 && diffZ < 0) return GetTuple(AreaDirection.TopBackRight);
            if (diffX > 0 && diffY > 0 && diffZ == 0) return GetTuple(AreaDirection.Top);
            if (diffX > 0 && diffY > 0 && diffZ > 0) return GetTuple(AreaDirection.TopFrontRight);
            if (diffX > 0 && diffY > 0 && diffZ < 0) return GetTuple(AreaDirection.TopBackRight);
            if (diffX > 0 && diffY < 0 && diffZ == 0) return GetTuple(AreaDirection.Bottom);
            if (diffX > 0 && diffY < 0 && diffZ > 0) return GetTuple(AreaDirection.BottomFrontRight);
            if (diffX > 0 && diffY < 0 && diffZ < 0) return GetTuple(AreaDirection.BottomBackRight);
            
            if (diffX < 0 && diffY == 0 && diffZ == 0) return GetTuple(AreaDirection.Left);
            if (diffX < 0 && diffY == 0 && diffZ > 0) return GetTuple(AreaDirection.TopFrontLeft);
            if (diffX < 0 && diffY == 0 && diffZ < 0) return GetTuple(AreaDirection.TopBackLeft);
            if (diffX < 0 && diffY > 0 && diffZ == 0) return GetTuple(AreaDirection.Top);
            if (diffX < 0 && diffY > 0 && diffZ > 0) return GetTuple(AreaDirection.TopFrontLeft);
            if (diffX < 0 && diffY > 0 && diffZ < 0) return GetTuple(AreaDirection.TopBackLeft);
            if (diffX < 0 && diffY < 0 && diffZ == 0) return GetTuple(AreaDirection.Bottom);
            if (diffX < 0 && diffY < 0 && diffZ > 0) return GetTuple(AreaDirection.BottomFrontLeft);
            if (diffX < 0 && diffY < 0 && diffZ < 0) return GetTuple(AreaDirection.BottomBackLeft);

            throw new NotImplementedException();
            
            (RouteAreaData RouteAreaData, int? NextIndex) GetTuple(AreaDirection direction)
            {
                return (new RouteAreaData(currentAreaIndex, direction), GetNextIndex(currentAreaIndex, direction, isEvenNumberDepth));
            }
        }

        int GetNextIndex(int index, AreaDirection areaDirection, bool isEvenNumberDepth)
        {
            switch (areaDirection)
            {
                case AreaDirection.Top: return index + MapSizeX;
                case AreaDirection.Bottom: return index - MapSizeX;
                case AreaDirection.Front: return index + MapSizeX * MapSizeY;
                case AreaDirection.Back: return index - MapSizeX * MapSizeY;
                case AreaDirection.Right: return index + 1;
                case AreaDirection.Left: return index - 1;
                case AreaDirection.TopFrontLeft: return index + MapSizeX * MapSizeY + (isEvenNumberDepth ? -1 : 0);
                case AreaDirection.TopFrontRight: return index + MapSizeX * MapSizeY + (isEvenNumberDepth ? 0 : 1);
                case AreaDirection.TopBackLeft: return index - MapSizeX * MapSizeY + (isEvenNumberDepth ? -1 : 0);
                case AreaDirection.TopBackRight: return index - MapSizeX * MapSizeY + (isEvenNumberDepth ? 0 : 1);
                case AreaDirection.BottomFrontLeft: return index + MapSizeX * MapSizeY - MapSizeX + (isEvenNumberDepth ? -1 : 0);
                case AreaDirection.BottomFrontRight: return index + MapSizeX * MapSizeY - MapSizeX + (isEvenNumberDepth ? 0 : 1);
                case AreaDirection.BottomBackLeft: return index - MapSizeX * MapSizeY - MapSizeX + (isEvenNumberDepth ? -1 : 0);
                case AreaDirection.BottomBackRight: return index - MapSizeX * MapSizeY - MapSizeX + (isEvenNumberDepth ? 0 : 1);
                default:
                    throw new ArgumentException();
            }
        }
    }
}
