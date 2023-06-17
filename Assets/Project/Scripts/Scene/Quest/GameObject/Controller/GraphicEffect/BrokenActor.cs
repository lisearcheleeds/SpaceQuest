using System.Linq;
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
        float smallTime;

        Vector3 movementVelocity;

        Rigidbody[] pieces;

        protected override void OnInit()
        {
            brokenActorGraphicEffectHandler = (BrokenActorGraphicEffectHandler)GraphicEffectHandler;
            actorGameObjectHandler = actorModel.Init(brokenActorGraphicEffectHandler.ActorData);

            movementVelocity = brokenActorGraphicEffectHandler.ActorData.MovingModule.MovementVelocity;

            pieces = GetComponentsInChildren<Rigidbody>();

            transform.position = GraphicEffectHandler.PositionData.Position;
            transform.rotation = GraphicEffectHandler.PositionData.Rotation;

            currentLifeTime = 0.0f;
            lifeTime = 15.0f;
            smallTime = 5.0f;

            foreach (var piece in pieces)
            {
                var mesh = piece.GetComponent<MeshFilter>().mesh;
                var centerOffset = mesh.bounds.center;
                mesh.SetVertices(mesh.vertices.Select(v => v - centerOffset).ToArray());
                mesh.RecalculateBounds();
                piece.transform.localPosition = piece.transform.localPosition + centerOffset;

                piece.AddForce(movementVelocity + Random.insideUnitSphere * 1.0f, ForceMode.Force);
                piece.AddTorque(Random.insideUnitSphere * 1f, ForceMode.Force);
            }
        }

        public override void OnLateUpdate(float deltaTime)
        {
            currentLifeTime += deltaTime;

            foreach (var piece in pieces)
            {
                piece.transform.localScale = Vector3.one * (1.0f - Mathf.Clamp01((currentLifeTime - smallTime) / (lifeTime - smallTime)));
            }

            if (currentLifeTime > lifeTime)
            {
                IsCompleted = true;
            }
        }
    }
}
