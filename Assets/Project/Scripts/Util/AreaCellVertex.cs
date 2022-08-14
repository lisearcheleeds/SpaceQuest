using UnityEngine;

namespace RoboQuest.Quest
{
    public static class AreaCellVertex
    {
        public static readonly Vector3[] Points =
        {
            new(-0.5f, -0.25f, 0.0f), // 0
            new(-0.25f, -0.5f, 0.0f), // 1
            new(0.0f, -0.5f, -0.25f), // 2
            new(0.0f, -0.25f, -0.5f), // 3
            new(-0.25f, 0.0f, -0.5f), // 4
            new(-0.5f, 0.0f, -0.25f), // 5
            new(0.25f, -0.5f, 0.0f), // 6
            new(0.5f, -0.25f, 0.0f), // 7
            new(0.5f, 0.0f, -0.25f), // 8
            new(0.25f, 0.0f, -0.5f), // 9
            new(0.0f, -0.5f, 0.25f), // 10
            new(0.0f, -0.25f, 0.5f), // 11
            new(0.25f, 0.0f, 0.5f), // 12
            new(0.5f, 0.0f, 0.25f), // 13
            new(-0.25f, 0.0f, 0.5f), // 14
            new(-0.5f, 0.0f, 0.25f), // 15
            new(-0.25f, 0.5f, 0.0f), // 16
            new(-0.5f, 0.25f, 0.0f), // 17
            new(0.0f, 0.25f, -0.5f), // 18
            new(0.0f, 0.5f, -0.25f), // 19
            new(0.5f, 0.25f, 0.0f), // 20
            new(0.25f, 0.5f, 0.0f), // 21
            new(0.0f, 0.25f, 0.5f), // 22
            new(0.0f, 0.5f, 0.25f), // 23
        };

        public static readonly Vector3[] Top = {Points[23], Points[21], Points[19], Points[16]};
        public static readonly Vector3[] Bottom = {Points[10], Points[1], Points[2], Points[6]};
        public static readonly Vector3[] Front = {Points[22], Points[14], Points[11], Points[12]};
        public static readonly Vector3[] Back = {Points[18], Points[9], Points[3], Points[4]};
        public static readonly Vector3[] Right = {Points[20], Points[13], Points[7], Points[8]};
        public static readonly Vector3[] Left = {Points[17], Points[5], Points[0], Points[15]};

        public static readonly Vector3[] TopFrontLeft = {Points[23], Points[16], Points[17], Points[15], Points[14], Points[22]};
        public static readonly Vector3[] TopFrontRight = {Points[21], Points[23], Points[22], Points[12], Points[13], Points[20]};
        public static readonly Vector3[] TopBackLeft = {Points[16], Points[19], Points[18], Points[4], Points[5], Points[17]};
        public static readonly Vector3[] TopBackRight = {Points[19], Points[21], Points[20], Points[8], Points[9], Points[18]};
        public static readonly Vector3[] BottomFrontLeft = {Points[10], Points[6], Points[7], Points[13], Points[12], Points[11]};
        public static readonly Vector3[] BottomFrontRight = {Points[1], Points[10], Points[11], Points[14], Points[15], Points[0]};
        public static readonly Vector3[] BottomBackLeft = {Points[2], Points[1], Points[0], Points[5], Points[4], Points[3]};
        public static readonly Vector3[] BottomBackRight = {Points[6], Points[2], Points[3], Points[9], Points[8], Points[7]};

        public static Vector3[] GetPrimitives(AreaDirection areaDirection)
        {
            switch (areaDirection)
            {
                case AreaDirection.Top: return Top;
                case AreaDirection.Bottom: return Bottom;
                case AreaDirection.Front: return Front;
                case AreaDirection.Back: return Back;
                case AreaDirection.Right: return Right;
                case AreaDirection.Left: return Left;
                case AreaDirection.TopFrontLeft: return TopFrontLeft;
                case AreaDirection.TopFrontRight: return TopFrontRight;
                case AreaDirection.TopBackLeft: return TopBackLeft;
                case AreaDirection.TopBackRight: return TopBackRight;
                case AreaDirection.BottomFrontLeft: return BottomFrontLeft;
                case AreaDirection.BottomFrontRight: return BottomFrontRight;
                case AreaDirection.BottomBackLeft: return BottomBackLeft;
                case AreaDirection.BottomBackRight: return BottomBackRight;
            }

            return null;
        }
    }
}
