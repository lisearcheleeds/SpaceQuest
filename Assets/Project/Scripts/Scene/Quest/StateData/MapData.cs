using System;
using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class MapData
    {
        public int MapSizeX => mapPresetVO.MapSizeX;
        public int MapSizeY => mapPresetVO.MapSizeY;
        public int MapSizeZ => mapPresetVO.MapSizeZ;
        public int MapSize => mapPresetVO.MapSize;
        public IAssetPath AmbientObjectAsset => mapPresetVO.AmbientObjectAsset;

        public float AreaSize => 1000.0f;

        public AreaData[] AreaData { get; }
        public (int X, int Y, int Z)[] MapPosition { get; }
        public Vector3[] MapLayout { get; }

        MapPresetVO mapPresetVO;

        public MapData(MapPresetVO mapPresetVO)
        {
            this.mapPresetVO = mapPresetVO;            
            AreaData = Enumerable.Range(0, MapSize)
                .Select(i => new AreaData(i, mapPresetVO.AreaAssetVOs[i], GetAdjacentIndexes(i)))
                .ToArray();

            MapPosition = Enumerable.Range(0, MapSize).Select(i => GetPosition(i)).ToArray();
            
            MapLayout = MapPosition.Select(v =>
            {
                var (x, y, z) = v;
                var isEvenNumberZ = z % 2 == 0;
                var evenNumberOffset = isEvenNumberZ ? 0.0f : 0.5f;
                return new Vector3(x + evenNumberOffset, y + evenNumberOffset, z / 2.0f);
            }).ToArray();
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

        int GetNextIndex(int index, AreaDirection areaDirection, bool isEvenNumberDepth)
        {
            switch (areaDirection)
            {
                case AreaDirection.Top: return index + MapSizeX;
                case AreaDirection.Bottom: return index - MapSizeX;
                case AreaDirection.Front: return index + MapSizeX * MapSizeY + MapSizeX * MapSizeY;
                case AreaDirection.Back: return index - MapSizeX * MapSizeY - MapSizeX * MapSizeY;
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
        
        (int x, int y, int z) GetPosition(int index)
        {
            var z = index / (MapSizeX * MapSizeY);
            var zi = index % (MapSizeX * MapSizeY);
            var y = zi / MapSizeX;
            var x = zi % MapSizeX;
            
            return (x, y, z);
        }
    }
}
