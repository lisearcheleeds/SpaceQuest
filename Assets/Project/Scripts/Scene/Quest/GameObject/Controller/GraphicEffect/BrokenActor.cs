using UnityEngine;

namespace AloneSpace
{
    public class BrokenActor : GraphicEffect
    {
        [SerializeField] ActorModel actorModel;
        BrokenActorGraphicEffectHandler brokenActorGraphicEffectHandler;
        ActorGameObjectHandler actorGameObjectHandler;

        float lifeTime;
        float currentLifeTime;

        Vector3 movementVelocity;

        protected override void OnInit()
        {
            brokenActorGraphicEffectHandler = (BrokenActorGraphicEffectHandler)GraphicEffectHandler;
            actorGameObjectHandler = actorModel.Init(brokenActorGraphicEffectHandler.ActorData);

            movementVelocity = brokenActorGraphicEffectHandler.ActorData.MovingModule.MovementVelocity;

            transform.position = GraphicEffectHandler.PositionData.Position;
            transform.rotation = GraphicEffectHandler.PositionData.Rotation;

            currentLifeTime = 0.0f;
            lifeTime = 1.0f;
        }

        public override void OnLateUpdate(float deltaTime)
        {
            currentLifeTime += deltaTime;
            if (currentLifeTime > lifeTime)
            {
                IsCompleted = true;
                return;
            }

            transform.position += movementVelocity;
            transform.localScale = Vector3.one * (1.0f - Mathf.Clamp01(currentLifeTime / lifeTime));
        }
    }
}
