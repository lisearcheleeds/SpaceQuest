using System;
using UnityEngine;

namespace AloneSpace.InSide
{
    public abstract class CollisionShape
    {
        /// <summary>位置</summary>
        public Vector3 Position { get; set; }

        public abstract bool CheckHit(CollisionShape hitCollision);
        
        public abstract Vector3 GetOutwardVector(Vector3 position);

        public bool CheckHit(CollisionShape hitCollision, Vector3 tempPosition)
        {
            var tmp = Position;
            Position = tempPosition;

            var result = CheckHit(hitCollision);
            Position = tmp;

            return result;
        }
    }

    public class CollisionShapeSphere : CollisionShape
    {
        /// <summary>球体の大きさ</summary>
        public float Range { get; set; }

        public CollisionShapeSphere(Vector3 position, float range)
        {
            Position = position;
            Range = range;
        }

        public override Vector3 GetOutwardVector(Vector3 position)
        {
            return (position - Position).normalized;
        }

        public override bool CheckHit(CollisionShape hitCollision)
        {
            // 当たり判定の相対ベクトル
            var vp = Position - hitCollision.Position;

            switch (hitCollision)
            {
                case CollisionShapeSphere hitCollisionSphere:
                    return (hitCollisionSphere.Position - Position).magnitude < (hitCollisionSphere.Range + Range);

                case CollisionShapeLine hitCollisionLine:
                    var lineDot = Vector3.Dot(hitCollisionLine.Directon, vp);

                    // 反対方向ならfalse(ただし当たり判定分は保証
                    if (lineDot < -(hitCollisionLine.Thickness + Range))
                    {
                        return false;
                    }

                    // hitCollisionCone.Directon方向への仕事分
                    var dotLineValue = hitCollisionLine.Directon * Vector3.Dot(hitCollisionLine.Directon, vp);
                    return (hitCollisionLine.Position + dotLineValue - Position).magnitude < (hitCollisionLine.Thickness + Range);

                case CollisionShapeCone hitCollisionCone:
                    var coneDot = Vector3.Dot(hitCollisionCone.Directon, vp);

                    // 反対方向ならfalse(ただし当たり判定分は保証
                    if (coneDot < -Range)
                    {
                        return false;
                    }

                    // hitCollisionCone.Directon方向への仕事分
                    var dotConeValue = hitCollisionCone.Directon * Vector3.Dot(hitCollisionCone.Directon, vp);
                    return (hitCollisionCone.Position + dotConeValue - Position).magnitude < (hitCollisionCone.ThicknessRatio * dotConeValue.magnitude) + Range;
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

        public CollisionShapeLine(Vector3 position, Vector3 directon, float thickness)
        {
            Position = position;
            Directon = directon;
            Thickness = thickness;
        }
        
        public override Vector3 GetOutwardVector(Vector3 position)
        {
            var relativeDirection = (position - Position).normalized;
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

        public CollisionShapeCone(Vector3 position, Vector3 directon, float thicknessRatio)
        {
            Position = position;
            Directon = directon;
            ThicknessRatio = thicknessRatio;
        }
               
        public override Vector3 GetOutwardVector(Vector3 position)
        {
            var relativeDirection = (position - Position).normalized;
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