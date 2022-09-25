using System;
using UnityEngine;
using UnityEngine.AI;

namespace AloneSpace
{
    public static class EnumExtension
    {
        public static AreaDirection GetReverseAreaDirection(this AreaDirection self)
        {
            switch (self)
            {
                case AreaDirection.Top: return AreaDirection.Top;
                case AreaDirection.Bottom: return AreaDirection.Bottom;
                case AreaDirection.Front: return AreaDirection.Back;
                case AreaDirection.Back: return AreaDirection.Front;
                case AreaDirection.Right: return AreaDirection.Left;
                case AreaDirection.Left: return AreaDirection.Right;
                case AreaDirection.TopFrontLeft: return AreaDirection.BottomBackRight;
                case AreaDirection.TopFrontRight: return AreaDirection.BottomBackLeft;
                case AreaDirection.TopBackLeft: return AreaDirection.BottomFrontRight;
                case AreaDirection.TopBackRight: return AreaDirection.BottomFrontLeft;
                case AreaDirection.BottomFrontLeft: return AreaDirection.TopBackRight;
                case AreaDirection.BottomFrontRight: return AreaDirection.TopBackLeft;
                case AreaDirection.BottomBackLeft: return AreaDirection.TopFrontRight;
                case AreaDirection.BottomBackRight: return AreaDirection.TopFrontLeft;
            }

            throw new ArgumentException();
        }

        public static Color GetRarityColor(this Rarity self)
        {
            switch (self)
            {
                case Rarity.Common:
                    return new Color32(169, 169, 169, 255);
                case Rarity.Rare:
                    return new Color32(47, 139, 204, 255);
                case Rarity.SuperRare:
                    return new Color32(169, 57, 204, 255);
                case Rarity.Legend:
                    return new Color32(204, 61, 57, 255);
                case Rarity.Unique:
                    return new Color32(195, 195, 195, 255);
            }

            throw new ArgumentException();
        }
        
        public static Color GetRaritySubColor(this Rarity self)
        {
            switch (self)
            {
                case Rarity.Common:
                    return new Color32(219, 219, 219, 255);
                case Rarity.Rare:
                    return new Color32(218, 250, 255, 255);
                case Rarity.SuperRare:
                    return new Color32(233, 218, 255, 255);
                case Rarity.Legend:
                    return new Color32(255, 238, 230, 255);
                case Rarity.Unique:
                    return new Color32(0, 0, 0, 255);
            }

            throw new ArgumentException();
        }
    }
}
