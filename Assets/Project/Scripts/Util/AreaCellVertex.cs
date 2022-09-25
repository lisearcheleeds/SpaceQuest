using System;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
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

        static readonly Vector3[] Top = {Points[23], Points[21], Points[19], Points[16]};
        static readonly Vector3[] Bottom = {Points[10], Points[1], Points[2], Points[6]};
        static readonly Vector3[] Front = {Points[22], Points[14], Points[11], Points[12]};
        static readonly Vector3[] Back = {Points[18], Points[9], Points[3], Points[4]};
        static readonly Vector3[] Right = {Points[20], Points[13], Points[7], Points[8]};
        static readonly Vector3[] Left = {Points[17], Points[5], Points[0], Points[15]};

        static readonly Vector3[] TopFrontLeft = {Points[23], Points[16], Points[17], Points[15], Points[14], Points[22]};
        static readonly Vector3[] TopFrontRight = {Points[21], Points[23], Points[22], Points[12], Points[13], Points[20]};
        static readonly Vector3[] TopBackLeft = {Points[16], Points[19], Points[18], Points[4], Points[5], Points[17]};
        static readonly Vector3[] TopBackRight = {Points[19], Points[21], Points[20], Points[8], Points[9], Points[18]};
        static readonly Vector3[] BottomFrontLeft = {Points[10], Points[6], Points[7], Points[13], Points[12], Points[11]};
        static readonly Vector3[] BottomFrontRight = {Points[1], Points[10], Points[11], Points[14], Points[15], Points[0]};
        static readonly Vector3[] BottomBackLeft = {Points[2], Points[1], Points[0], Points[5], Points[4], Points[3]};
        static readonly Vector3[] BottomBackRight = {Points[6], Points[2], Points[3], Points[9], Points[8], Points[7]};
        
        static readonly Vector3 TopVector = Top.Aggregate((x, y) => x + y) / Top.Length;
        static readonly Vector3 BottomVector = Bottom.Aggregate((x, y) => x + y) / Bottom.Length;
        static readonly Vector3 FrontVector = Front.Aggregate((x, y) => x + y) / Front.Length;
        static readonly Vector3 BackVector = Back.Aggregate((x, y) => x + y) / Back.Length;
        static readonly Vector3 RightVector = Right.Aggregate((x, y) => x + y) / Right.Length;
        static readonly Vector3 LeftVector = Left.Aggregate((x, y) => x + y) / Left.Length;
        
        static readonly Vector3 TopFrontLeftVector = TopFrontLeft.Aggregate((x, y) => x + y) / TopFrontLeft.Length;
        static readonly Vector3 TopFrontRightVector = TopFrontRight.Aggregate((x, y) => x + y) / TopFrontRight.Length;
        static readonly Vector3 TopBackLeftVector = TopBackLeft.Aggregate((x, y) => x + y) / TopBackLeft.Length;
        static readonly Vector3 TopBackRightVector = TopBackRight.Aggregate((x, y) => x + y) / TopBackRight.Length;
        static readonly Vector3 BottomFrontLeftVector = BottomFrontLeft.Aggregate((x, y) => x + y) / BottomFrontLeft.Length;
        static readonly Vector3 BottomFrontRightVector = BottomFrontRight.Aggregate((x, y) => x + y) / BottomFrontRight.Length;
        static readonly Vector3 BottomBackLeftVector = BottomBackLeft.Aggregate((x, y) => x + y) / BottomBackLeft.Length;
        static readonly Vector3 BottomBackRightVector = BottomBackRight.Aggregate((x, y) => x + y) / BottomBackRight.Length;
        
        static readonly Vector3 TopDirection = TopVector.normalized;
        static readonly Vector3 BottomDirection = BottomVector.normalized;
        static readonly Vector3 FrontDirection = FrontVector.normalized;
        static readonly Vector3 BackDirection = BackVector.normalized;
        static readonly Vector3 RightDirection = RightVector.normalized;
        static readonly Vector3 LeftDirection = LeftVector.normalized;

        static readonly Vector3 TopFrontLeftDirection = TopFrontLeftVector.normalized;
        static readonly Vector3 TopFrontRightDirection = TopFrontRightVector.normalized;
        static readonly Vector3 TopBackLeftDirection = TopBackLeftVector.normalized;
        static readonly Vector3 TopBackRightDirection = TopBackRightVector.normalized;
        static readonly Vector3 BottomFrontLeftDirection = BottomFrontLeftVector.normalized;
        static readonly Vector3 BottomFrontRightDirection = BottomFrontRightVector.normalized;
        static readonly Vector3 BottomBackLeftDirection = BottomBackLeftVector.normalized;
        static readonly Vector3 BottomBackRightDirection = BottomBackRightVector.normalized;

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

            throw new NotImplementedException();
        }
        
        public static Vector3 GetVector(AreaDirection areaDirection)
        {
            switch (areaDirection)
            {
                case AreaDirection.Top: return TopVector;
                case AreaDirection.Bottom: return BottomVector;
                case AreaDirection.Front: return FrontVector;
                case AreaDirection.Back: return BackVector;
                case AreaDirection.Right: return RightVector;
                case AreaDirection.Left: return LeftVector;
                case AreaDirection.TopFrontLeft: return TopFrontLeftVector;
                case AreaDirection.TopFrontRight: return TopFrontRightVector;
                case AreaDirection.TopBackLeft: return TopBackLeftVector;
                case AreaDirection.TopBackRight: return TopBackRightVector;
                case AreaDirection.BottomFrontLeft: return BottomFrontLeftVector;
                case AreaDirection.BottomFrontRight: return BottomFrontRightVector;
                case AreaDirection.BottomBackLeft: return BottomBackLeftVector;
                case AreaDirection.BottomBackRight: return BottomBackRightVector;
            }

            throw new NotImplementedException();
        }
        
        public static Vector3 GetDirection(AreaDirection areaDirection)
        {
            switch (areaDirection)
            {
                case AreaDirection.Top: return TopDirection;
                case AreaDirection.Bottom: return BottomDirection;
                case AreaDirection.Front: return FrontDirection;
                case AreaDirection.Back: return BackDirection;
                case AreaDirection.Right: return RightDirection;
                case AreaDirection.Left: return LeftDirection;
                case AreaDirection.TopFrontLeft: return TopFrontLeftDirection;
                case AreaDirection.TopFrontRight: return TopFrontRightDirection;
                case AreaDirection.TopBackLeft: return TopBackLeftDirection;
                case AreaDirection.TopBackRight: return TopBackRightDirection;
                case AreaDirection.BottomFrontLeft: return BottomFrontLeftDirection;
                case AreaDirection.BottomFrontRight: return BottomFrontRightDirection;
                case AreaDirection.BottomBackLeft: return BottomBackLeftDirection;
                case AreaDirection.BottomBackRight: return BottomBackRightDirection;
            }

            throw new NotImplementedException();
        }
    }
}
