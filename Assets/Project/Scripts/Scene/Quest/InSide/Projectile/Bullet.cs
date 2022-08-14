using UnityEngine;

namespace RoboQuest.Quest.InSide
{
    public class Bullet : Projectile
    {
        Vector3 direction;
        float speed;
        
        public override CollisionShape CollisionShape { get; protected set; }
        public override CollisionShape HitCollidePrediction { get; protected set; }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="itemVO">リソースデータ</param>
        /// <param name="target">ターゲット</param>
        /// <param name="condition">使用時の状態(1.0最高 ~ 0.0最低) Bulletではdirectionに影響</param>
        public override void Implement(WeaponData weaponData, ItemVO itemVO, ITarget target, float condition)
        {
            base.Implement(weaponData, itemVO, target, condition);
            
            transform.position = weaponData.Position;
            speed = 200.0f;
            LifeTime = 4;

            var relativePosition = target.TargetData.Position - weaponData.Position;
            var predictedPosition = target.TargetData.Position + (target.MoveDelta * (relativePosition.magnitude / speed));

            direction = (predictedPosition - weaponData.Position).normalized;

            CollisionShape = new CollisionShapeSphere(weaponData.Position, 1.0f);
            HitCollidePrediction = new CollisionShapeLine(weaponData.Position, direction, 1.0f);

            MessageBus.Instance.SendThreat.Broadcast(this, true);
            MessageBus.Instance.SendCollision.Broadcast(this, true);
            
            MessageBus.Instance.NoticeHitCollision.AddListener(NoticeHitCollision);
        }

        protected override void OnUpdate(float deltaTime)
        {
            transform.position += direction * speed * deltaTime;
            CollisionShape.Position = transform.position;
            HitCollidePrediction.Position = transform.position;
        }

        protected override void OnRelease()
        {
            MessageBus.Instance.SendThreat.Broadcast(this, false);
            MessageBus.Instance.SendCollision.Broadcast(this, false);
            
            MessageBus.Instance.NoticeHitCollision.RemoveListener(NoticeHitCollision);
        }
        
        void NoticeHitCollision(ICollision collision1, ICollision collision2)
        {
            ICollision otherCollision = null;
            if (collision1 == this)
            {
                otherCollision = collision2;
            }
            
            if (collision2 == this)
            {
                otherCollision = collision1;
            }

            if (otherCollision == null)
            {
                return;
            }
            
            if (otherCollision is IActor && otherCollision.PlayerInstanceId != PlayerInstanceId)
            {
                Release();
            }
        }
    }
}
