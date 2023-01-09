using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public abstract class CollisionShape
    {
        /// <summary>位置</summary>
        public IPositionData PositionData { get; }

        public abstract bool CheckHit(CollisionShape hitCollision);
        
        public abstract Vector3 GetOutwardVector(IPositionData positionData);

        protected CollisionShape(IPositionData positionData)
        {
            PositionData = positionData;
        }

        // FIXME: すり抜け用のアレ
        public bool CheckHit(CollisionShape hitCollision, IPositionData tempPositionData)
        {
            /*
            var tmp = Position;
            Position = tempPosition;

            var result = CheckHit(hitCollision);
            Position = tmp;

            return result;
            */
            return false;
        }
    }

    public class CollisionShapeSphere : CollisionShape
    {
        /// <summary>球体の大きさ</summary>
        public float Range { get; set; }

        public CollisionShapeSphere(IPositionData positionData, float range) : base(positionData)
        {
            Range = range;
        }

        public override Vector3 GetOutwardVector(IPositionData positionData)
        {
            return (positionData.Position - PositionData.Position).normalized;
        }

        public override bool CheckHit(CollisionShape hitCollision)
        {
            // 当たり判定の相対ベクトル
            var vp = PositionData.Position - hitCollision.PositionData.Position;

            switch (hitCollision)
            {
                case CollisionShapeSphere hitCollisionSphere:
                    return (hitCollisionSphere.PositionData.Position - PositionData.Position).magnitude < (hitCollisionSphere.Range + Range);

                case CollisionShapeLine hitCollisionLine:
                    var lineDot = Vector3.Dot(hitCollisionLine.Directon, vp);

                    // 反対方向ならfalse(ただし当たり判定分は保証
                    if (lineDot < -(hitCollisionLine.Thickness + Range))
                    {
                        return false;
                    }

                    // hitCollisionCone.Directon方向への仕事分
                    var dotLineValue = hitCollisionLine.Directon * Vector3.Dot(hitCollisionLine.Directon, vp);
                    return (hitCollisionLine.PositionData.Position + dotLineValue - PositionData.Position).magnitude < (hitCollisionLine.Thickness + Range);

                case CollisionShapeCone hitCollisionCone:
                    var coneDot = Vector3.Dot(hitCollisionCone.Directon, vp);

                    // 反対方向ならfalse(ただし当たり判定分は保証
                    if (coneDot < -Range)
                    {
                        return false;
                    }

                    // hitCollisionCone.Directon方向への仕事分
                    var dotConeValue = hitCollisionCone.Directon * Vector3.Dot(hitCollisionCone.Directon, vp);
                    return (hitCollisionCone.PositionData.Position + dotConeValue - PositionData.Position).magnitude < (hitCollisionCone.ThicknessRatio * dotConeValue.magnitude) + Range;
            }

            return false;
        }
    }

    public class CollisionShapeLine : CollisionShape
    {
        /// <summary>直線の向き</summary>
        public Vector3 Directon { get; set; }

        /// <summary>太さ</summary>
        public float Thickness { get; set; }

        public CollisionShapeLine(IPositionData positionData, Vector3 directon, float thickness) : base(positionData)
        {
            Directon = directon;
            Thickness = thickness;
        }
        
        public override Vector3 GetOutwardVector(IPositionData positionData)
        {
            var relativeDirection = (positionData.Position - PositionData.Position).normalized;
            return Vector3.Cross(Vector3.Cross(relativeDirection, Directon), Directon) * -1;
        }

        public override bool CheckHit(CollisionShape hitCollision)
        {
            switch (hitCollision)
            {
                case CollisionShapeSphere hitCollisionSphere:
                    return hitCollisionSphere.CheckHit(this);
                case CollisionShapeLine hitCollisionLine:
                    throw new NotImplementedException();
                case CollisionShapeCone hitCollisionCone:
                    throw new NotImplementedException();
            }

            return false;
        }
    }

    public class CollisionShapeCone : CollisionShape
    {
        /// <summary>コーンの向き</summary>
        public Vector3 Directon { get; set; }

        /// <summary>コーンの角度（1を指定すると距離1に対して大きさ1で45度ぐらいになる？）</summary>
        public float ThicknessRatio { get; set; }

        public CollisionShapeCone(IPositionData positionData, Vector3 directon, float thicknessRatio) : base(positionData)
        {
            Directon = directon;
            ThicknessRatio = thicknessRatio;
        }
               
        public override Vector3 GetOutwardVector(IPositionData positionData)
        {
            var relativeDirection = (positionData.Position - PositionData.Position).normalized;
            return Vector3.Cross(Vector3.Cross(relativeDirection, Directon), Directon) * -1;
        }

        public override bool CheckHit(CollisionShape hitCollision)
        {
            switch (hitCollision)
            {
                case CollisionShapeSphere hitCollisionSphere:
                    return hitCollisionSphere.CheckHit(this);

                case CollisionShapeLine hitCollisionLine:
                    throw new NotImplementedException();
                case CollisionShapeCone hitCollisionCone:
                    throw new NotImplementedException();
            }

            return false;
        }
    }
}