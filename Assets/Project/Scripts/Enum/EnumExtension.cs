using System;
using UnityEngine;
using UnityEngine.AI;

namespace AloneSpace
{
    public static class EnumExtension
    {
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
