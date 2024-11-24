using UnityEngine;

namespace AloneSpace
{
    public class StandardExplosion : GraphicEffect
    {
        float lifeTime;
        float currentLifeTime;

        protected override void OnInit()
        {
            transform.position = GraphicEffectHandler.PositionData.Position;
            transform.rotation = GraphicEffectHandler.PositionData.Rotation;

            currentLifeTime = 0.0f;
            lifeTime = 4.0f;
        }

        public override void OnLateUpdate(float deltaTime)
        {
            currentLifeTime += deltaTime;
            if (currentLifeTime > lifeTime)
            {
                IsCompleted = true;
                return;
            }

            transform.position = GraphicEffectHandler.PositionData.Position;
            transform.rotation = GraphicEffectHandler.PositionData.Rotation;
        }
    }
}